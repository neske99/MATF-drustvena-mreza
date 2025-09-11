<template>
  <div class="user-search-page">
    <!-- Search Header -->
    <v-card
      class="search-header-card mb-6"
      elevation="2"
      rounded="lg"
    >
      <v-card-text class="pa-6">
        <div class="d-flex align-center">
          <v-avatar size="60" color="matf-red" class="mr-4">
            <v-icon size="32" color="white">mdi-account-search</v-icon>
          </v-avatar>
          <div class="flex-grow-1">
            <h1 class="text-h4 font-weight-bold matf-red--text mb-2">
              Search Results
            </h1>
            <p class="text-body-1 text--secondary mb-0">
              {{ searchResultsText }}
            </p>
          </div>

          <!-- Search Stats -->
          <div class="d-none d-md-flex align-center">
            <div class="text-center">
              <h3 class="text-h6 font-weight-bold matf-red--text">{{ currentSearchedUsers.length }}</h3>
              <p class="text-caption text--secondary mb-0">Results</p>
            </div>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Search Results -->
    <div class="search-results">
      <!-- Loading State -->
      <div v-if="loading" class="loading-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <v-progress-circular
            indeterminate
            color="matf-red"
            size="48"
            class="mb-4"
          />
          <h4 class="text-h6 font-weight-medium text--secondary mb-2">Searching...</h4>
          <p class="text-body-2 text--secondary">Finding MATF community members for "{{ userSearchName }}"</p>
        </v-card>
      </div>

      <!-- Empty State -->
      <div v-else-if="currentSearchedUsers.length === 0 && !loading" class="empty-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <div class="empty-icon mb-4">
            <v-icon size="80" color="grey-lighten-1">mdi-account-search</v-icon>
          </div>
          <h3 class="text-h5 font-weight-bold text--secondary mb-3">No users found</h3>
          <p class="text-body-1 text--secondary mb-4">
            We couldn't find anyone matching "{{ userSearchName }}"
          </p>
          <p class="text-body-2 text--secondary mb-4">
            Try searching with different terms or check the spelling.
          </p>
          <v-btn
            color="matf-red"
            variant="outlined"
            @click="goToAllUsers"
          >
            <v-icon class="mr-2">mdi-account-group</v-icon>
            Browse All Users
          </v-btn>
        </v-card>
      </div>

      <!-- Results Grid -->
      <div v-else-if="!loading" class="results-grid">
        <h3 class="text-h6 font-weight-bold text--primary mb-4">
          <v-icon class="mr-2" color="matf-red">mdi-account-group</v-icon>
          MATF Community Members
        </h3>

        <v-row>
          <v-col
            v-for="user in currentSearchedUsers"
            :key="user.id"
            cols="12"
            sm="12"
            lg="6"
            class="mb-4"
          >
            <UserCardComponent
              :text="user.username"
              :firstName="user.firstName"
              :lastName="user.lastName"
              :userId="user.id"
              :relation="user.relation"
              :profilePictureUrl="user.profilePictureUrl"
              @request-sent="onRequestSent"
              @request-accepted="onRequestAccepted"
              @request-declined="onRequestDeclined"
              @friendship-removed="onFriendshipRemoved"
            />
          </v-col>
        </v-row>

      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { userStore } from '../stores/user';
import { authStore } from '../stores/auth';
import { userCacheService } from '../services/userCacheService';
import UserCardComponent from '../components/UserSearch/UserCard.vue';
import type { UserPreviewDTO } from '../dtos/user/userPreviewDTO';

export default defineComponent({
  name: 'UserSearchView',
  props: ['userSearchName'],
  components: {
    UserCardComponent
  },
  data() {
    return {
      currentSearchedUsers: [] as UserPreviewDTO[],
      loading: false,
      loadingMore: false,
      searchPerformed: false
    }
  },
  computed: {
    searchResultsText() {
      if (this.loading) return 'Searching...';
      if (!this.searchPerformed) return 'Enter a search term to find users';

      const count = this.currentSearchedUsers.length;
      if (count === 0) {
        return `No results found for "${this.userSearchName}"`;
      } else {
        return `Found ${count} ${count === 1 ? 'person' : 'people'}`;
      }
    }
  },
  async created() {
    if (this.userSearchName && this.userSearchName.trim()) {
      await this.performSearch(this.userSearchName);
    }
  },
  methods: {
    async performSearch(searchTerm: string) {
      if (!searchTerm || searchTerm.trim().length === 0) {
        this.currentSearchedUsers = [];
        this.searchPerformed = false;
        return;
      }

      try {
        this.loading = true;
        this.searchPerformed = true;
        const userstore = userStore();

        console.log('Searching for:', searchTerm);
        console.log('Current user ID:', authStore().userId);

        const results = await userstore.GetSearchedUsers(searchTerm.trim());

        // Filter out current user from results and enhance with profile pictures
        const filteredResults = results.filter(user =>
          user.username !== authStore().username
        );

        // Enhance each result with profile picture data
        const enhancedResults = await Promise.all(
          filteredResults.map(async (user) => {
            try {
              const userDetails = await userCacheService.getUserByUsername(user.username);
              return {
                ...user,
                profilePictureUrl: userDetails?.profilePictureUrl || undefined
              };
            } catch (error) {
              console.error(`Error fetching details for ${user.username}:`, error);
              return {
                ...user,
                profilePictureUrl: undefined
              };
            }
          })
        );

        this.currentSearchedUsers = enhancedResults;

        console.log('Search results with relations and profile pictures:', this.currentSearchedUsers);
      } catch (error) {
        console.error('Error searching users:', error);
        this.currentSearchedUsers = [];
      } finally {
        this.loading = false;
      }
    },


    async goToAllUsers() {
      try {
        this.loading = true;

        const userstore = userStore();
        const allUsers = await userstore.GetUsers();

        // Filter out current user and get relationship status for each user
        const filteredUsers = allUsers.filter(user =>
          user.username !== authStore().username
        );

        // Fetch relationship status and profile pictures for each user
        const enhancedUsers = await Promise.all(
          filteredUsers.map(async (user) => {
            try {
              // Get relationship status
              const relation = await userstore.GetRelation(authStore().userId, user.id);

              // Get profile picture
              const userDetails = await userCacheService.getUserByUsername(user.username);

              return {
                ...user,
                relation: relation,
                profilePictureUrl: userDetails?.profilePictureUrl || undefined
              };
            } catch (error) {
              console.error(`Error getting data for user ${user.id}:`, error);
              return {
                ...user,
                relation: 'NONE', // Default relation if error
                profilePictureUrl: undefined
              };
            }
          })
        );

        this.currentSearchedUsers = enhancedUsers;
        this.searchPerformed = true;

        console.log('All users with relations and profile pictures:', this.currentSearchedUsers);
      } catch (error) {
        console.error('Error loading all users:', error);
      } finally {
        this.loading = false;
      }
    },

    onRequestSent(userId: number) {
      // Update the user's status in the list
      const user = this.currentSearchedUsers.find(u => u.id === userId);
      if (user) {
        user.relation = 'REQUESTED_FRIENDSHIP_WITH';
        console.log(`Updated relation for user ${userId} to: REQUESTED_FRIENDSHIP_WITH`);
      }
    },

    onRequestAccepted(userId: number) {
      // Update the user's status in the list
      const user = this.currentSearchedUsers.find(u => u.id === userId);
      if (user) {
        user.relation = 'FRIEND_WITH';
        console.log(`Updated relation for user ${userId} to: FRIEND_WITH`);
      }
    },

    onRequestDeclined(userId: number) {
      // Update the user's status in the list
      const user = this.currentSearchedUsers.find(u => u.id === userId);
      if (user) {
        user.relation = 'NONE';
        console.log(`Updated relation for user ${userId} to: NONE`);
      }
    },

    onFriendshipRemoved(userId: number) {
      // Update the user's status in the list
      const user = this.currentSearchedUsers.find(u => u.id === userId);
      if (user) {
        user.relation = 'NONE';
        console.log(`Updated relation for user ${userId} to: NONE`);
      }
    }
  },
  watch: {
    '$route.params.userSearchName': {
      immediate: true,
      async handler(newValue) {
        if (newValue && typeof newValue === 'string') {
          await this.performSearch(newValue);
        }
      }
    }
  }
});
</script>

<style scoped>
.user-search-page {
  animation: fadeInUp 0.6s ease-out;
}

.search-header-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.search-header-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.loading-state,
.empty-state {
  animation: slideInUp 0.5s ease-out;
}

.empty-icon {
  animation: float 3s ease-in-out infinite;
}

.results-grid {
  animation: slideInUp 0.5s ease-out 0.2s both;
}

/* Animations */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes float {
  0%, 100% { transform: translateY(0px); }
  50% { transform: translateY(-10px); }
}

/* Mobile Responsiveness */
@media (max-width: 768px) {
  .search-header-card .v-card-text {
    padding: 1.5rem;
  }

  .user-search-page {
    animation: none; /* Reduce animations on mobile */
  }
}

@media (max-width: 600px) {
  .search-header-card .d-flex {
    flex-direction: column;
    text-align: center;
  }

  .search-header-card .mr-4 {
    margin-right: 0 !important;
    margin-bottom: 1rem;
  }
}
</style>
