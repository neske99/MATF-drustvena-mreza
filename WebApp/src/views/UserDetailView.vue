<template>
  <div class="user-detail-page">
    <!-- User Profile Header -->
    <v-card 
      class="profile-header-card mb-6" 
      elevation="2"
      rounded="lg"
    >
      <!-- Cover Photo Area -->
      <div class="cover-photo">
        <div class="cover-overlay"></div>
      </div>
      
      <!-- Profile Content -->
      <v-card-text class="profile-content pa-6">
        <div class="d-flex align-start">
          <!-- Profile Avatar -->
          <div class="profile-avatar-container mr-6">
            <v-avatar size="120" color="matf-red" class="profile-avatar">
              <v-icon size="60" color="white">mdi-account</v-icon>
            </v-avatar>
          </div>
          
          <!-- Profile Info -->
          <div class="flex-grow-1">
            <div class="d-flex justify-space-between align-start mb-4">
              <div>
                <h1 class="text-h4 font-weight-bold matf-red--text mb-2">
                  {{ userDetail.firstName }} {{ userDetail.lastName }}
                </h1>
                <h2 class="text-h6 text--secondary mb-2">
                  @{{ userDetail.username }}
                </h2>
              </div>
              
              <!-- Action Buttons -->
              <div class="profile-actions">
                <!-- Own Profile Actions -->
                <div v-if="isOwnProfile">
                  <v-btn
                    color="matf-red"
                    size="large"
                    class="text-none mr-2"
                    prepend-icon="mdi-pencil"
                    @click="editProfile = true"
                  >
                    Edit Profile
                  </v-btn>
                  
                  <v-btn
                    color="matf-red"
                    size="large"
                    class="text-none"
                    prepend-icon="mdi-cog"
                    to="/settings"
                  >
                    Settings
                  </v-btn>
                </div>

                <!-- Other Profile Actions -->
                <div v-else>
                  <!-- Send Friend Request - only when no relationship exists -->
                  <v-btn
                    v-if="relation === 'NONE' || relation === '' || !relation"
                    color="matf-red"
                    size="large"
                    class="text-none mr-2"
                    @click="sendRequest"
                    prepend-icon="mdi-account-plus"
                  >
                    Send Friend Request
                  </v-btn>
                  
                  <!-- Accept Request - when you received a request -->
                  <v-btn
                    v-if="relation === 'RECEIVED_FRIENDSHIP_REQUEST_FROM'"
                    color="matf-red"
                    size="large"
                    class="text-none mr-2"
                    @click="acceptRequest"
                    prepend-icon="mdi-check"
                  >
                    Accept Request
                  </v-btn>
                  
                  <!-- Request Sent - when you sent a request -->
                  <v-btn
                    v-if="relation === 'REQUESTED_FRIENDSHIP_WITH'"
                    color="matf-red"
                    size="large"
                    class="text-none mr-2"
                    disabled
                  >
                    <v-icon class="mr-2">mdi-clock</v-icon>
                    Request Sent
                  </v-btn>
                  
                  <!-- Friends Actions - when you are friends -->
                  <template v-if="relation === 'FRIEND_WITH'">
                    <v-btn
                      color="matf-red"
                      size="large"
                      class="text-none mr-2"
                      prepend-icon="mdi-account-minus"
                      @click="removeFriend"
                    >
                      Remove Friend
                    </v-btn>
                    
                    <v-btn
                      color="matf-red"
                      size="large"
                      class="text-none"
                      prepend-icon="mdi-message"
                      @click="openChat"
                    >
                      Message
                    </v-btn>
                  </template>
                </div>
              </div>
            </div>
            
            <!-- Profile Stats -->
            <div class="profile-stats">
              <v-row>
                <v-col cols="auto">
                  <div class="stat-item">
                    <h3 class="text-h6 font-weight-bold matf-red--text">{{ userPosts.length }}</h3>
                    <p class="text-caption text--secondary mb-0">Posts</p>
                  </div>
                </v-col>
                <v-col cols="auto">
                  <div class="stat-item">
                    <h3 class="text-h6 font-weight-bold matf-red--text">{{ totalLikes }}</h3>
                    <p class="text-caption text--secondary mb-0">Likes</p>
                  </div>
                </v-col>
                <v-col cols="auto">
                  <div class="stat-item">
                    <h3 class="text-h6 font-weight-bold matf-red--text">{{ friendsCount }}</h3>
                    <p class="text-caption text--secondary mb-0">Friends</p>
                  </div>
                </v-col>
                <v-col cols="auto">
                  <div class="stat-item">
                    <h3 class="text-h6 font-weight-bold matf-red--text">2024</h3>
                    <p class="text-caption text--secondary mb-0">Member Since</p>
                  </div>
                </v-col>
              </v-row>
            </div>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Profile Navigation -->
    <v-card 
      class="profile-nav-card mb-6" 
      elevation="1"
      rounded="lg"
    >
      <v-card-text class="pa-4">
        <v-tabs 
          v-model="activeTab" 
          color="matf-red"
          align-tabs="start"
          class="profile-tabs"
        >
          <v-tab value="posts">
            <v-icon class="mr-2">mdi-post</v-icon>
            Posts
          </v-tab>
          <v-tab value="about">
            <v-icon class="mr-2">mdi-information</v-icon>
            About
          </v-tab>
          <v-tab value="friends" v-if="isOwnProfile || relation === 'FRIEND_WITH'">
            <v-icon class="mr-2">mdi-account-group</v-icon>
            Friends
          </v-tab>
        </v-tabs>
      </v-card-text>
    </v-card>

    <!-- Content Sections -->
    <v-window v-model="activeTab">
      <!-- Posts Tab -->
      <v-window-item value="posts">
        <div class="posts-section">
          <!-- Posts Header -->
          <div class="section-header mb-4">
            <h3 class="text-h6 font-weight-bold text--primary">
              <v-icon class="mr-2" color="matf-red">mdi-post</v-icon>
              {{ isOwnProfile ? 'Your Posts' : `${userDetail.firstName}'s Posts` }}
            </h3>
            <p class="text-body-2 text--secondary mb-0">
              {{ userPosts.length }} posts shared
            </p>
          </div>
          
          <!-- Posts List -->
          <div v-if="loading" class="loading-posts">
            <v-card class="pa-8 text-center" elevation="1" rounded="lg">
              <v-progress-circular 
                indeterminate 
                color="matf-red" 
                size="48"
                class="mb-4"
              />
              <h4 class="text-h6 font-weight-medium text--secondary mb-2">Loading posts...</h4>
              <p class="text-body-2 text--secondary">Getting latest posts</p>
            </v-card>
          </div>
          
          <div v-else-if="userPosts.length === 0" class="empty-posts">
            <v-card class="pa-8 text-center" elevation="1" rounded="lg">
              <v-icon size="64" color="grey-lighten-2" class="mb-4">mdi-post-outline</v-icon>
              <h3 class="text-h6 font-weight-bold text--secondary mb-2">No posts yet</h3>
              <p class="text-body-2 text--secondary">
                {{ isOwnProfile ? "You haven't shared any posts yet." : `${userDetail.firstName} hasn't shared any posts yet.` }}
              </p>
            </v-card>
          </div>
          
          <div v-else class="posts-list">
            <TransitionGroup name="post" tag="div">
              <PostComponent 
                v-for="post in userPosts" 
                :id="post.id" 
                :text="post.text" 
                :username="post.user.username" 
                :comments="post.comments" 
                :likes="post.likes"
                :key="post.id"
                @comment-added="refreshUserPosts"
                @like-toggled="refreshUserPosts"
                class="mb-4"
              />
            </TransitionGroup>
          </div>
        </div>
      </v-window-item>

      <!-- About Tab -->
      <v-window-item value="about">
        <v-card class="about-card" elevation="1" rounded="lg">
          <v-card-text class="pa-6">
            <h3 class="text-h6 font-weight-bold text--primary mb-4">
              <v-icon class="mr-2" color="matf-red">mdi-information</v-icon>
              About {{ isOwnProfile ? 'You' : userDetail.firstName }}
            </h3>
            
            <div class="about-info">
              <div class="info-item mb-3">
                <v-icon color="matf-red" class="mr-3">mdi-account</v-icon>
                <div>
                  <p class="text-subtitle-2 font-weight-medium mb-1">Full Name</p>
                  <p class="text-body-2 text--secondary">{{ userDetail.firstName }} {{ userDetail.lastName }}</p>
                </div>
              </div>
              
              <div class="info-item mb-3">
                <v-icon color="matf-red" class="mr-3">mdi-at</v-icon>
                <div>
                  <p class="text-subtitle-2 font-weight-medium mb-1">Username</p>
                  <p class="text-body-2 text--secondary">@{{ userDetail.username }}</p>
                </div>
              </div>
              
              <div class="info-item mb-3">
                <v-icon color="matf-red" class="mr-3">mdi-school</v-icon>
                <div>
                  <p class="text-subtitle-2 font-weight-medium mb-1">Institution</p>
                  <p class="text-body-2 text--secondary">Faculty of Mathematics, University of Belgrade</p>
                </div>
              </div>
              
              <div class="info-item mb-3">
                <v-icon color="matf-red" class="mr-3">mdi-account-group</v-icon>
                <div>
                  <p class="text-subtitle-2 font-weight-medium mb-1">Friends</p>
                  <p class="text-body-2 text--secondary">{{ friendsCount }} {{ friendsCount === 1 ? 'friend' : 'friends' }}</p>
                </div>
              </div>
              
              <div class="info-item mb-3">
                <v-icon color="matf-red" class="mr-3">mdi-calendar</v-icon>
                <div>
                  <p class="text-subtitle-2 font-weight-medium mb-1">Member Since</p>
                  <p class="text-body-2 text--secondary">January 2024</p>
                </div>
              </div>
            </div>
          </v-card-text>
        </v-card>
      </v-window-item>

      <!-- Friends Tab -->
      <v-window-item value="friends">
        <v-card class="friends-card" elevation="1" rounded="lg">
          <v-card-text class="pa-6">
            <h3 class="text-h6 font-weight-bold text--primary mb-4">
              <v-icon class="mr-2" color="matf-red">mdi-account-group</v-icon>
              {{ isOwnProfile ? 'Your Friends' : `${userDetail.firstName}'s Friends` }}
            </h3>
            
            <!-- Friends List -->
            <div v-if="loadingFriends" class="loading-friends text-center py-6">
              <v-progress-circular 
                indeterminate 
                color="matf-red" 
                size="48"
                class="mb-4"
              />
              <p class="text-body-2 text--secondary">Loading friends...</p>
            </div>
            
            <div v-else-if="friendsList.length === 0" class="text-center py-6">
              <v-icon size="64" color="grey-lighten-2" class="mb-4">mdi-account-group-outline</v-icon>
              <h4 class="text-subtitle-1 font-weight-medium text--secondary mb-2">
                {{ isOwnProfile ? 'No friends yet' : `${userDetail.firstName} has no friends yet` }}
              </h4>
              <p class="text-body-2 text--secondary">
                {{ isOwnProfile ? 'Start connecting with people to build your network!' : 'This user hasn\'t connected with anyone yet.' }}
              </p>
            </div>
            
            <div v-else class="friends-grid">
              <v-row>
                <v-col 
                  v-for="friend in friendsList" 
                  :key="friend.id"
                  cols="12" sm="12" md="6"
                  class="mb-4"
                >
                  <v-card 
                    class="friend-card"
                    elevation="1"
                    rounded="lg"
                    hover
                    @click="viewFriendProfile(friend.username)"
                  >
                    <v-card-text class="pa-4">
                      <div class="d-flex align-center">
                        <v-avatar size="48" color="matf-red" class="mr-3">
                          <v-icon color="white">mdi-account</v-icon>
                        </v-avatar>
                        
                        <div class="flex-grow-1">
                          <h4 class="text-subtitle-1 font-weight-bold text--primary mb-1">
                            {{ friend.firstName }} {{ friend.lastName }}
                          </h4>
                          <p class="text-caption text--secondary mb-0">
                            @{{ friend.username }}
                          </p>
                        </div>
                        
                        <v-icon color="matf-red">mdi-chevron-right</v-icon>
                      </div>
                    </v-card-text>
                  </v-card>
                </v-col>
              </v-row>
            </div>
          </v-card-text>
        </v-card>
      </v-window-item>
    </v-window>

    <!-- Edit Profile Dialog -->
    <v-dialog v-model="editProfile" max-width="500px">
      <v-card>
        <v-card-title class="text-h5 matf-red--text">Edit Profile</v-card-title>
        <v-card-text>
          <v-form ref="editForm">
            <v-text-field
              v-model="editedFirstName"
              label="First Name"
              variant="outlined"
              color="matf-red"
              class="mb-4"
              :rules="[rules.required]"
            />
            <v-text-field
              v-model="editedLastName"
              label="Last Name"
              variant="outlined"
              color="matf-red"
              :rules="[rules.required]"
            />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="grey" variant="text" @click="editProfile = false">
            Cancel
          </v-btn>
          <v-btn color="matf-red" @click="saveProfile">
            Save Changes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import { chatStore } from '@/stores/chat.ts';
import { postStore } from '@/stores/post.ts';
import axiosAuthenticated from '@/plugin/axios';
import { getSignalRConnection } from '@/plugin/signalr.ts';
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO.ts';
import type { postDTO } from '@/dtos/post/postDTO.ts';

export default defineComponent({
  name: 'UserDetailView',
  props: ['username'],
  components: {
    PostComponent
  },
  data() {
    return {
      userDetail: {} as UserDetailDTO,
      relation: '',
      friendshipButtonClicked: false,
      userPosts: [] as postDTO[],
      activeTab: 'posts',
      friendsCount: 0,
      friendsList: [] as UserDetailDTO[],
      loadingFriends: false,
      loading: true,
      editProfile: false,
      editedFirstName: '',
      editedLastName: '',
      rules: {
        required: (v: string) => !!v || 'This field is required'
      }
    };
  },
  async created() {
    await this.loadUserData();
  },
  computed: {
    totalLikes() {
      return this.userPosts.reduce((sum, post) => sum + (post.likes?.length || 0), 0);
    },
    
    isOwnProfile() {
      return this.username === authStore().username;
    }
  },
  methods: {
    async loadUserData() {
      try {
        this.loading = true;
        const userstore = userStore();
        
        this.userDetail = await userstore.GetUser(this.username);
        
        if (!this.isOwnProfile) {
          this.relation = await userstore.GetRelation(authStore().userId, this.userDetail.id);
        }
        
        this.userPosts = await postStore().GetPostsForUser(this.userDetail.id);
        await this.loadFriendsList();
        
        this.editedFirstName = this.userDetail.firstName;
        this.editedLastName = this.userDetail.lastName;
      } catch (error) {
        // Handle errors silently
      } finally {
        this.loading = false;
      }
    },

    async loadFriendsList() {
      try {
        this.loadingFriends = true;
        const userstore = userStore();
        
        const friends = await userstore.GetUserFriends(this.userDetail.id);
        this.friendsCount = friends.length;
        
        if (friends.length === 0) {
          this.friendsList = [];
          return;
        }
        
        const allUsers = await userstore.GetUsers();
        
        const friendDetailsPromises = friends
          .filter(friend => friend && friend.id)
          .map(async (friend) => {
            const friendUser = allUsers.find(u => u.id === friend.id);
            if (friendUser) {
              try {
                return await userstore.GetUser(friendUser.username);
              } catch (error) {
                return null;
              }
            }
            return null;
          });
        
        const friendDetails = await Promise.all(friendDetailsPromises);
        
        this.friendsList = friendDetails
          .filter((friend): friend is UserDetailDTO => friend !== null)
          .filter((friend, index, self) => 
            self.findIndex(f => f.id === friend.id) === index
          );
      } catch (error) {
        this.friendsCount = 0;
        this.friendsList = [];
      } finally {
        this.loadingFriends = false;
      }
    },

    async sendRequest() {
      try {
        await userStore().SendFriendRequest(authStore().userId, this.userDetail.id);
        this.relation = 'REQUESTED_FRIENDSHIP_WITH';
      } catch (error) {
        // Handle errors silently
      }
    },
    
    async acceptRequest() {
      try {
        await userStore().AcceptFriendRequest(authStore().userId, this.userDetail.id);
        this.relation = 'FRIEND_WITH';
        await this.loadFriendsList();
      } catch (error) {
        // Handle errors silently
      }
    },

    async removeFriend() {
      try {
        await userStore().RemoveFriend(authStore().userId, this.userDetail.id);
        this.relation = 'NONE';
        await this.loadFriendsList();
      } catch (error) {
        // Handle errors silently
      }
    },
    async openChat() {
      try {
        const chatGroupsStore = chatStore();
        await chatGroupsStore.getChatGroupsForUser();
        
        let existingChatGroup = chatGroupsStore.currentChatGroups.find(
          group => group.username === this.userDetail.username
        );
        
        if (!existingChatGroup) {
          try {
            const response = await axiosAuthenticated.post(
              `http://localhost:8095/api/v1/Chat/CreateChatGroup?userAId=${authStore().userId}&userBId=${this.userDetail.id}`
            );
            
            const newChatGroup = response.data;
            
            existingChatGroup = {
              chatId: newChatGroup.id,
              username: this.userDetail.username,
              userId: this.userDetail.id,
              hasNewMessages: false
            };
            
            chatGroupsStore.currentChatGroups.unshift(existingChatGroup);
          } catch (error) {
            existingChatGroup = {
              chatId: Date.now(),
              username: this.userDetail.username,
              userId: this.userDetail.id,
              hasNewMessages: false
            };
            
            chatGroupsStore.currentChatGroups.unshift(existingChatGroup);
          }
        }
        
        await chatGroupsStore.switchUserChat(existingChatGroup);
      } catch (error) {
        // Handle errors silently
      }
    },
    
    viewFriendProfile(username: string) {
      this.$router.push({ name: 'UserDetail', params: { username } });
    },
  },
  watch: {
    username: {
      immediate: true,
      async handler() {
        if (this.username) {
          await this.loadUserData();
        }
      }
    }
  }
});
</script>

<style scoped>
/* Same styles as before, but with updated responsive design */
.user-detail-page {
  animation: fadeInUp 0.6s ease-out;
}

.profile-header-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  position: relative;
  overflow: hidden;
}

.cover-photo {
  height: 200px;
  background: linear-gradient(135deg, #8B0000 0%, #660000 100%);
  position: relative;
}

.cover-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="university-pattern" width="40" height="40" patternUnits="userSpaceOnUse"><circle cx="20" cy="20" r="1" fill="white" opacity="0.1"/><circle cx="10" cy="30" r="0.5" fill="white" opacity="0.05"/><circle cx="30" cy="10" r="0.5" fill="white" opacity="0.05"/></pattern></defs><rect width="100" height="100" fill="url(%23university-pattern)"/></svg>');
}

.profile-content {
  margin-top: -60px;
  position: relative;
  z-index: 1;
}

.profile-avatar-container {
  position: relative;
}

.profile-avatar {
  border: 4px solid white;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.profile-actions {
  display: flex;
  align-items: flex-start;
}

.profile-stats .stat-item {
  text-align: center;
  padding: 0 16px;
  border-right: 1px solid rgba(139, 0, 0, 0.1);
}

.profile-stats .stat-item:last-child {
  border-right: none;
}

.profile-nav-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
}

.profile-tabs {
  border-bottom: 1px solid rgba(139, 0, 0, 0.1);
}

.section-header {
  padding: 0 8px;
}

.info-item {
  display: flex;
  align-items: flex-start;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
}

.v-btn:not(.v-btn--disabled) {
  opacity: 1 !important;
}

.profile-actions .v-btn {
  transition: all 0.2s ease;
}

.profile-actions .v-btn:hover:not(.v-btn--disabled) {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(139, 0, 0, 0.2);
}

/* Post Transitions */
.post-enter-active,
.post-leave-active {
  transition: all 0.5s ease;
}

.post-enter-from {
  opacity: 0;
  transform: translateY(30px);
}

.post-leave-to {
  opacity: 0;
  transform: translateY(-30px);
}

.post-move {
  transition: transform 0.5s ease;
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

/* Friends Grid Styling */
.friends-grid {
  margin-top: 1rem;
}

.friend-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: all 0.3s ease;
  cursor: pointer;
}

.friend-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(139, 0, 0, 0.1);
}

.friend-card:hover .v-icon {
  color: #8B0000 !important;
}

/* Loading states */
.loading-friends {
  min-height: 200px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

/* Mobile Responsiveness */
@media (max-width: 768px) {
  .cover-photo {
    height: 150px;
  }
  
  .profile-content {
    margin-top: -40px;
  }
  
  .profile-avatar-container .profile-avatar {
    width: 80px !important;
    height: 80px !important;
  }
  
  .profile-actions {
    flex-direction: column;
    width: 100%;
    gap: 8px;
  }
  
  .profile-actions .v-btn {
    width: 100%;
    margin-right: 0 !important;
  }
  
  .profile-stats .v-row {
    justify-content: center;
  }
  
  .profile-stats .stat-item {
    padding: 0 12px;
  }
  
  .friends-grid .v-col {
    padding: 4px;
  }
}

@media (max-width: 600px) {
  .d-flex.align-start {
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
  
  .profile-avatar-container {
    margin-right: 0 !important;
    margin-bottom: 1rem;
  }
  
  .profile-stats {
    margin-top: 1rem;
  }
}
</style>
