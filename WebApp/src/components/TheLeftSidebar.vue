<template>
  <v-navigation-drawer
    permanent
    class="left-sidebar"
    width="280"
  >
    <div class="sidebar-content">
      <!-- Sidebar Header -->
      <div class="sidebar-header pa-4">
        <h3 class="text-h6 font-weight-bold matf-red--text mb-1">
          <v-icon class="mr-2" color="matf-red">mdi-chat</v-icon>
          Messages
        </h3>
        <p class="text-caption text--secondary mb-0">Connect with your colleagues</p>
      </div>

      <v-divider />

      <!-- Search for Friends -->
      <div class="search-section pa-4">
        <v-text-field
          v-model.trim="searchedUser"
          placeholder="Search conversations..."
          prepend-inner-icon="mdi-magnify"
          variant="outlined"
          density="compact"
          color="matf-red"
          hide-details
          class="search-field"
        />
      </div>

      <!-- Friends/Chat List -->
      <div class="friends-list">
        <!-- No Friends State -->
        <div v-if="filteredFriends.length === 0" class="no-friends-state pa-4 text-center">
          <v-icon size="48" color="grey-lighten-1" class="mb-3">mdi-account-search</v-icon>
          <h4 class="text-subtitle-1 font-weight-medium text--secondary mb-2">
            No conversations yet
          </h4>
          <p class="text-caption text--secondary mb-3">
            Start chatting with your MATF colleagues
          </p>
          <v-btn
            color="matf-red"
            variant="outlined"
            size="small"
            class="text-none"
            @click="openAddFriendDialog"
          >
            <v-icon class="mr-1" size="16">mdi-account-plus</v-icon>
            Find People
          </v-btn>
          <div class="mt-2" v-if="debugInfo">
            <p class="text-caption text--secondary">Debug: {{ debugInfo }}</p>
          </div>
        </div>

        <!-- Friends List -->
        <v-list v-else class="friends-list-container" lines="two">
          <template v-for="friend in filteredFriends" :key="friend.chatId">
            <v-list-item
              @click="openChatWithFriend(friend)"
              :class="{
                'active-chat': friend.chatId === currentChatGroupId,
                'has-new-messages': friend.hasNewMessages
              }"
              class="friend-item"
            >
              <!-- Friend Avatar -->
              <template v-slot:prepend>
                <v-avatar size="40" color="matf-red">
                  <img
                    v-if="getUserProfilePicture(friend)"
                    :src="getUserProfilePicture(friend)"
                    alt="Profile Picture"
                    @error="() => {}"
                    class="avatar-image"
                  />
                  <v-icon v-else color="white">mdi-account</v-icon>
                </v-avatar>
              </template>

              <!-- Friend Info -->
              <v-list-item-title
                :class="{
                  'font-weight-bold': friend.hasNewMessages,
                  'text--primary': !friend.hasNewMessages
                }"
                class="friend-name"
              >
                {{ friend.username }}
              </v-list-item-title>

              <v-list-item-subtitle class="text--secondary">
                {{ getFriendDisplayName(friend) }}
              </v-list-item-subtitle>

              <!-- New Message Indicator -->
              <template v-slot:append v-if="friend.hasNewMessages">
                <v-badge color="matf-red" dot />
              </template>
            </v-list-item>

            <v-divider class="mx-4" />
          </template>
        </v-list>
      </div>

      <!-- Quick Actions - Removed, keeping only Find People button -->
    </div>

    <!-- Add Friend Dialog -->
    <v-dialog v-model="addFriendDialog" max-width="600px">
      <v-card class="add-friend-dialog">
        <v-card-title class="pa-4">
          <h3 class="text-h6 font-weight-bold matf-red--text">
            <v-icon class="mr-2" color="matf-red">mdi-account-search</v-icon>
            Find People
          </h3>
        </v-card-title>

        <v-card-text class="pa-4">
          <v-text-field
            v-model="addFriendSearchText"
            placeholder="Search for MATF students..."
            prepend-inner-icon="mdi-magnify"
            variant="outlined"
            color="matf-red"
            class="mb-4"
            hide-details
            @input="searchAllUsers"
            :loading="searchingAllUsers"
          />

          <!-- Search Results -->
          <div v-if="allUsersResults.length > 0" class="search-results">
            <p class="text-subtitle-2 font-weight-medium mb-3">MATF Students ({{ allUsersResults.length }} found):</p>
            <v-list class="search-results-list" max-height="400" style="overflow-y: auto;">
              <v-list-item
                v-for="user in allUsersResults"
                :key="user.id"
                class="search-result-item"
                @click="goToUserProfile(user.username)"
              >
                <template v-slot:prepend>
                  <v-avatar size="40" color="matf-red">
                    <img
                      v-if="getUserProfilePicture(user)"
                      :src="getUserProfilePicture(user)"
                      alt="Profile Picture"
                      @error="() => {}"
                      class="avatar-image"
                    />
                    <v-icon v-else color="white">mdi-account</v-icon>
                  </v-avatar>
                </template>

                <v-list-item-title>{{ user.firstName }} {{ user.lastName }}</v-list-item-title>
                <v-list-item-subtitle>@{{ user.username }} � MATF Student</v-list-item-subtitle>

                <template v-slot:append>
                  <v-icon color="grey-lighten-1">mdi-chevron-right</v-icon>
                </template>
              </v-list-item>
            </v-list>
          </div>

          <!-- No Results -->
          <div v-else-if="addFriendSearchText.length > 0 && !searchingAllUsers" class="text-center py-4">
            <v-icon size="48" color="grey-lighten-2" class="mb-2">mdi-account-search</v-icon>
            <p class="text-body-2 text--secondary">No users found matching "{{ addFriendSearchText }}"</p>
            <p class="text-caption text--secondary mt-2">Try searching with different terms</p>
          </div>

          <!-- Initial State -->
          <div v-else-if="addFriendSearchText.length === 0" class="text-center py-4">
            <v-icon size="48" color="matf-red" class="mb-2">mdi-account-search</v-icon>
            <p class="text-body-2 text--secondary">Search for MATF students to view their profiles</p>
            <p class="text-caption text--secondary mt-2">Click on any user to view their profile page</p>
          </div>

          <!-- Loading State -->
          <div v-else-if="searchingAllUsers" class="text-center py-4">
            <v-progress-circular
              indeterminate
              color="matf-red"
              size="32"
              class="mb-3"
            />
            <p class="text-body-2 text--secondary">Searching...</p>
          </div>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-spacer />
          <v-btn
            color="grey"
            variant="text"
            @click="closeAddFriendDialog"
          >
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-navigation-drawer>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import { chatStore } from '../stores/chat'
import { userStore } from '../stores/user'
import { authStore } from '../stores/auth'
import { userCacheService } from '../services/userCacheService'
import type { HubConnection } from '@microsoft/signalr';
import type { ChatGroupDTO } from '../dtos/chat/chatGroupDTO';
import type { UserPreviewDTO } from '../dtos/user/userPreviewDTO';
import { addCallback, getSignalRConnection } from '../plugin/signalr'

export default defineComponent({
  name: 'TheLeftSidebar',
  data() {
    return {
      searchedUser: "",
      connection: null as HubConnection | null,

      // Find People Dialog
      addFriendDialog: false,
      addFriendSearchText: "",
      allUsersResults: [] as UserPreviewDTO[],
      searchingAllUsers: false,
      addFriendTimeout: null as NodeJS.Timeout | null,

      // User profile picture cache
      userProfilePictures: new Map<string, string>(),
      friendsUserInfo: new Map<string, any>(),

      // Debug info
      debugInfo: null as string | null
    }
  },
  async created() {
    try {
      const store = chatStore();
      console.log('Loading chat groups for user:', authStore().userId);

      await store.getChatGroupsForUser();
      console.log('Chat groups loaded:', store.currentChatGroups);

      this.debugInfo = `Loaded ${store.currentChatGroups.length} chat groups`;

      await this.loadUserProfilePictures();

      addCallback("ReceiveMessage",async (chatGroupId: number, userId: number, username: string) => {
          const store = chatStore();
          store.currentChatGroups.unshift({
            username: username,
            chatId: chatGroupId,
            userId: userId,
            hasNewMessages: true
          });
          (await getSignalRConnection())?.invoke("RegisterToGroup", chatGroupId);
        });
   } catch (error) {
      console.error('Error in LeftSidebar created:', error);
      this.debugInfo = `Error: ${error}`;
    }
  },
  async mounted() {
    // Listen for friendship changes from other components
  },

  beforeUnmount() {
    // Clean up event listener

    // Clean up SignalR connection
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  },
  computed: {
    filteredFriends() {
      return this.friends.filter(x =>
        x.username.toLowerCase().includes(this.searchedUser.toLowerCase())
      );
    },

    friends() {
      return chatStore().currentChatGroups;
    },

    currentChatGroupId() {
      return chatStore().currentChatGroupId;
    }
  },
  methods: {
    openChatWithFriend(friend: ChatGroupDTO) {
      console.log('Opening chat with friend:', friend);
      const store = chatStore();
      store.switchUserChat(friend);
      friend.hasNewMessages = false;
      console.log('Friend new messages set to false');
    },

    // Open Add Friend Dialog - same functionality as before
    openAddFriendDialog() {
      this.addFriendDialog = true;
    },

    // Search all users (like navbar search)
    searchAllUsers() {
      if (this.addFriendTimeout) {
        clearTimeout(this.addFriendTimeout);
      }

      this.addFriendTimeout = setTimeout(async () => {
        if (this.addFriendSearchText.trim().length < 2) {
          this.allUsersResults = [];
          return;
        }

        try {
          this.searchingAllUsers = true;
          const userstore = userStore();
          const results = await userstore.GetSearchedUsers(this.addFriendSearchText.trim());

          // Show all users (no filtering by relationship status)
          this.allUsersResults = results.filter(user =>
            user.username !== authStore().username // Only exclude current user
          );

        } catch (error) {
          console.error('Error searching all users:', error);
          this.allUsersResults = [];
        } finally {
          this.searchingAllUsers = false;
        }
      }, 500); // 500ms debounce
    },

    // Navigate to user profile
    goToUserProfile(username: string) {
      this.closeAddFriendDialog();
      this.$router.push(`/userdetail/${username}`);
    },

    // Close Add Friend dialog and reset state
    closeAddFriendDialog() {
      this.addFriendDialog = false;
      this.addFriendSearchText = "";
      this.allUsersResults = [];
      if (this.addFriendTimeout) {
        clearTimeout(this.addFriendTimeout);
        this.addFriendTimeout = null;
      }
    },

    // Load profile pictures for chat friends
    async loadUserProfilePictures() {
      const friends = chatStore().currentChatGroups;
      for (const friend of friends) {
        if (friend.username && !this.userProfilePictures.has(friend.username)) {
          try {
            const userInfo = await userCacheService.getUserByUsername(friend.username);
            if (userInfo) {
              this.friendsUserInfo.set(friend.username, userInfo);
              if (userInfo.profilePictureUrl) {
                this.userProfilePictures.set(friend.username, userInfo.profilePictureUrl);
              }
            }
          } catch (error) {
            console.log(`Could not fetch profile picture for ${friend.username}:`, error);
          }
        }
      }
    },

    // Get friend display name
    getFriendDisplayName(friend: any) {
      const userInfo = this.friendsUserInfo.get(friend.username);
      if (userInfo && userInfo.firstName && userInfo.lastName) {
        return `${userInfo.firstName} ${userInfo.lastName}`;
      }
      return 'MATF Student';
    },

    // Get profile picture URL for a user
    getUserProfilePicture(user: any) {
      const profilePictureUrl = user.profilePictureUrl || this.userProfilePictures.get(user.username);
      if (!profilePictureUrl) return null;

      if (profilePictureUrl.startsWith('/uploads/profile-pictures/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${profilePictureUrl}`
          : profilePictureUrl;
      }

      if (profilePictureUrl.startsWith('/uploads/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${profilePictureUrl}`
          : profilePictureUrl;
      }

      return profilePictureUrl;
    },

    // Refresh chat groups when friendship changes
    async refreshChatGroups() {
      try {
        console.log('Refreshing chat groups due to friendship change');
        const store = chatStore();
        await store.getChatGroupsForUser();
        await this.loadUserProfilePictures();
      } catch (error) {
        console.error('Error refreshing chat groups:', error);
      }
    }
  }
})
</script>

<style scoped>
.left-sidebar {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border-right: 1px solid rgba(139, 0, 0, 0.1);
}

.sidebar-content {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.sidebar-header {
  background: rgba(139, 0, 0, 0.03);
  border-bottom: 1px solid rgba(139, 0, 0, 0.1);
}

.search-field :deep(.v-field) {
  border-radius: 8px;
  background: #F5F5F5;
}

.search-field :deep(.v-field--focused) {
  background: #FFFFFF;
}

.friends-list {
  flex: 1;
  overflow-y: auto;
}

.friends-list-container {
  padding: 0;
}

.friend-item {
  border-radius: 8px;
  margin: 4px 8px;
  transition: all 0.2s ease;
  cursor: pointer;
}

.friend-item:hover {
  background: rgba(139, 0, 0, 0.05);
  transform: translateY(-1px);
}

.active-chat {
  background: rgba(139, 0, 0, 0.1) !important;
  border-left: 3px solid #8B0000;
}

.has-new-messages {
  background: rgba(139, 0, 0, 0.05);
}

.has-new-messages .friend-name {
  color: #8B0000 !important;
}

.no-friends-state {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  min-height: 200px;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
  border-radius: 8px;
}

/* Dialog Styles */
.add-friend-dialog {
  border-radius: 8px;
  border: 1px solid rgba(139, 0, 0, 0.1);
}

.search-results-list {
  border: 1px solid rgba(139, 0, 0, 0.1);
  border-radius: 8px;
  background: #FAFAFA;
}

.search-result-item {
  border-radius: 8px;
  margin: 4px 8px;
  transition: all 0.2s ease;
  cursor: pointer;
}

.search-result-item:hover {
  background: rgba(139, 0, 0, 0.05);
  transform: translateY(-1px);
}

/* Mobile responsiveness */
@media (max-width: 768px) {
  .left-sidebar {
    display: none !important;
  }
}

/* Profile Picture Styles - Fix image fitting */
.avatar-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}
</style>
