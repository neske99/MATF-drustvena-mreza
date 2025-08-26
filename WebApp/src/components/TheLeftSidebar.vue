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
                  <v-icon color="white">mdi-account</v-icon>
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
                MATF Student
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
                    <v-icon color="white">mdi-account</v-icon>
                  </v-avatar>
                </template>
                
                <v-list-item-title>{{ user.firstName }} {{ user.lastName }}</v-list-item-title>
                <v-list-item-subtitle>@{{ user.username }} • MATF Student</v-list-item-subtitle>
                
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
import { chatStore } from '../stores/chat.ts'
import { userStore } from '../stores/user.ts'
import { authStore } from '../stores/auth.ts'
import type { HubConnection } from '@microsoft/signalr';
import type { ChatGroupDTO } from '@/dtos/chat/chatGroupDTO.ts';
import type { UserPreviewDTO } from '@/dtos/user/userPreviewDTO.ts';
import { getSignalRConnection } from '../plugin/signalr.ts'

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
      addFriendTimeout: null as NodeJS.Timeout | null
    }
  },
  async created() {
    try {
      await chatStore().getChatGroupsForUser();

      this.connection = await getSignalRConnection();
      if (this.connection) {
        this.connection.on("ReceiveMessage", (chatGroupId: number, userId: number, username: string) => {
          this.friends.unshift({
            username: username,
            chatId: chatGroupId,
            userId: userId, 
            hasNewMessages: true
          });
          this.connection?.invoke("RegisterToGroup", chatGroupId);
        });
      }
    } catch (error) {
      // Ignore connection errors
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
      chatStore().switchUserChat(friend);
      friend.hasNewMessages = false;
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

/* Custom scrollbar */
.friends-list::-webkit-scrollbar,
.search-results-list::-webkit-scrollbar {
  width: 4px;
}

.friends-list::-webkit-scrollbar-track,
.search-results-list::-webkit-scrollbar-track {
  background: transparent;
}

.friends-list::-webkit-scrollbar-thumb,
.search-results-list::-webkit-scrollbar-thumb {
  background: rgba(139, 0, 0, 0.3);
  border-radius: 2px;
}

.friends-list::-webkit-scrollbar-thumb:hover,
.search_RESULTS_list::-webkit-scrollbar-thumb:hover {
  background: rgba(139, 0, 0, 0.5);
}

/* Animations */
.friend-item {
  animation: slideInLeft 0.3s ease-out;
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

/* Mobile responsiveness */
@media (max-width: 768px) {
  .left-sidebar {
    display: none !important;
  }
}
</style>
