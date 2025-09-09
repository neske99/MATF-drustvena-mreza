<template>
  <div class="friend-requests-page">
    <!-- Friend Requests Header -->
    <v-card
      class="requests-header-card mb-6"
      elevation="2"
      rounded="lg"
    >
      <v-card-text class="pa-6">
        <div class="d-flex align-center">
          <v-avatar size="60" color="matf-red" class="mr-4">
            <v-icon size="32" color="white">mdi-account-clock</v-icon>
          </v-avatar>
          <div class="flex-grow-1">
            <h1 class="text-h4 font-weight-bold matf-red--text mb-2">
              Friend Requests
            </h1>
            <p class="text-body-1 text--secondary mb-0">
              {{ requestsText }}
            </p>
          </div>

          <!-- Request Stats -->
          <div class="d-none d-md-flex align-center">
            <div class="text-center">
              <h3 class="text-h6 font-weight-bold matf-red--text">{{ friendRequests.length }}</h3>
              <p class="text-caption text--secondary mb-0">Pending</p>
            </div>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Friend Requests List -->
    <div class="friend-requests-content">
      <!-- Loading State -->
      <div v-if="loading" class="loading-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <v-progress-circular
            indeterminate
            color="matf-red"
            size="48"
            class="mb-4"
          />
          <h4 class="text-h6 font-weight-medium text--secondary mb-2">Loading requests...</h4>
          <p class="text-body-2 text--secondary">Getting your friend requests</p>
        </v-card>
      </div>

      <!-- Empty State -->
      <div v-else-if="friendRequests.length === 0" class="empty-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <div class="empty-icon mb-4">
            <v-icon size="80" color="grey-lighten-1">mdi-account-clock-outline</v-icon>
          </div>
          <h3 class="text-h5 font-weight-bold text--secondary mb-3">No pending requests</h3>
          <p class="text-body-1 text--secondary mb-4">
            You don't have any friend requests at the moment.
          </p>
          <v-btn
            color="matf-red"
            @click="goToUserSearch"
          >
            <v-icon class="mr-2">mdi-account-search</v-icon>
            Find People
          </v-btn>
        </v-card>
      </div>

      <!-- Requests Grid -->
      <div v-else class="requests-grid">
        <h3 class="text-h6 font-weight-bold text--primary mb-4">
          <v-icon class="mr-2" color="matf-red">mdi-account-clock</v-icon>
          Pending Friend Requests
        </h3>

        <v-row>
          <v-col
            v-for="request in friendRequests"
            :key="request.id"
            cols="12"
            sm="12"
            lg="6"
            class="mb-4"
          >
            <userCardComponent
              :text="request.username"
              :firstName="request.firstName"
              :lastName="request.lastName"
              :userId="request.id"
              :relation="request.relation"
              :profilePictureUrl="request.profilePictureUrl"
              @request-accepted="onRequestAccepted"
              @request-declined="onRequestDeclined"
            />
          </v-col>
        </v-row>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import PostComponent from '../components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth'
import { userStore } from '../stores/user'
import { userCacheService } from '../services/userCacheService';
import userCardComponent from '../components/UserSearch/UserCard.vue'
import type { UserPreviewDTO } from '../dtos/user/userPreviewDTO.ts';

export default defineComponent({
  name: 'FriendRequestsView',
  components: {
    userCardComponent
  },
  data() {
    return {
      friendRequests: [] as UserPreviewDTO[],
      loading: false
    }
  },
  computed: {
    requestsText() {
      if (this.loading) return 'Loading requests...';

      const count = this.friendRequests.length;
      if (count === 0) {
        return 'No pending friend requests';
      } else {
        return `${count} ${count === 1 ? 'person wants' : 'people want'} to be your friend`;
      }
    }
  },
  async created() {
    await this.loadFriendRequests();
  },
  methods: {
    async loadFriendRequests() {
      try {
        this.loading = true;

        // Get friend requests
        const requests = await userStore().GetFriendRequests();

        // Enhance each request with profile picture data
        const enhancedRequests = await Promise.all(
          requests.map(async (request) => {
            try {
              const userDetails = await userCacheService.getUserByUsername(request.username);
              return {
                ...request,
                profilePictureUrl: userDetails?.profilePictureUrl || null
              };
            } catch (error) {
              console.error(`Error fetching details for ${request.username}:`, error);
              return {
                ...request,
                profilePictureUrl: null
              };
            }
          })
        );

        this.friendRequests = enhancedRequests;
        console.log('Enhanced friend requests:', this.friendRequests);
      } catch (error) {
        console.error('Error loading friend requests:', error);
        this.friendRequests = [];
      } finally {
        this.loading = false;
      }
    },

    onRequestAccepted(userId: number) {
      // Remove the accepted request from the list
      const requestIndex = this.friendRequests.findIndex(r => r.id === userId);
      if (requestIndex !== -1) {
        this.friendRequests.splice(requestIndex, 1);
        console.log(`Removed accepted request for user ${userId}`);
      }
    },

    onRequestDeclined(userId: number) {
      // Remove the declined request from the list
      const requestIndex = this.friendRequests.findIndex(r => r.id === userId);
      if (requestIndex !== -1) {
        this.friendRequests.splice(requestIndex, 1);
        console.log(`Removed declined request for user ${userId}`);
      }
    },

    goToUserSearch() {
      this.$router.push('/userSearch/');
    }
  }
});
</script>

<style scoped>
.friend-requests-page {
  animation: fadeInUp 0.6s ease-out;
}

.requests-header-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.requests-header-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.loading-state,
.empty-state {
  animation: slideInUp 0.5s ease-out;
}

.empty-icon {
  animation: float 3s ease-in-out infinite;
}

.requests-grid {
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
  .requests-header-card .v-card-text {
    padding: 1.5rem;
  }

  .friend-requests-page {
    animation: none; /* Reduce animations on mobile */
  }
}

@media (max-width: 600px) {
  .requests-header-card .d-flex {
    flex-direction: column;
    text-align: center;
  }

  .requests-header-card .mr-4 {
    margin-right: 0 !important;
    margin-bottom: 1rem;
  }
}
</style>
