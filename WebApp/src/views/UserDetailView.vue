<template>
  <div class="profile-detail-page">
    <!-- User Profile Header -->
    <v-card class="profile-header-card mb-6" elevation="2" rounded="lg">
      <!-- Cover Photo Area -->
      <div class="cover-photo">
        <div class="cover-overlay"></div>
      </div>
      <!-- Profile Content -->
      <v-card-text class="profile-content pa-6">
        <div class="d-flex align-start">
          <!-- Profile Avatar -->
          <div class="profile-avatar-container mr-6 avatar-hover-group" style="position:relative;">
            <v-avatar size="120" color="matf-red" class="profile-avatar mb-2">
              <img v-if="formattedProfilePictureUrl" :src="formattedProfilePictureUrl" alt="Profile Picture" @error="handleImageError" />
              <v-icon v-else size="60" color="white">mdi-account</v-icon>
            </v-avatar>
            <v-btn
              v-if="isOwnProfile"
              icon
              class="avatar-edit-btn"
              color="white"
              style="position:absolute;top:8px;left:8px;z-index:2;background:white;width:32px;height:32px;min-width:32px;min-height:32px;"
              @click="showProfilePictureModal = true"
            >
              <v-icon color="matf-red" size="16">mdi-pencil</v-icon>
            </v-btn>
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
                    class="text-none"
                    prepend-icon="mdi-cog"
                    to="/settings"
                  >
                    Settings
                  </v-btn>
                </div>

                <!-- Other Profile Actions -->
                <div v-else class="d-flex align-center">
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
                      prepend-icon="mdi-message"
                      @click="openChat"
                    >
                      Message
                    </v-btn>
                  </template>

                  <!-- More Options Menu - only show when there are menu options available -->
                  <v-menu v-if="relation === 'FRIEND_WITH'" offset-y>
                    <template v-slot:activator="{ props }">
                      <v-btn
                        icon
                        variant="text"
                        size="large"
                        v-bind="props"
                        class="ml-2"
                      >
                        <v-icon>mdi-dots-vertical</v-icon>
                      </v-btn>
                    </template>
                    <v-list>
                      <!-- Remove Friend option - only when you are friends -->
                      <v-list-item @click="confirmRemoveFriend">
                        <template v-slot:prepend>
                          <v-icon color="red-darken-1">mdi-account-minus</v-icon>
                        </template>
                        <v-list-item-title>Remove Friend</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-menu>
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
                :firstName="post.user.firstName"
                :lastName="post.user.lastName"
                :userId="post.user.id"
                :createdDate="post.createdDate"
                :userProfilePictureUrl="post.user.profilePictureUrl"
                :comments="post.comments"
                :likes="post.likes"
                :fileUrl="post.fileUrl"
                :fileName="post.fileName"
                :fileType="post.fileType"
                :key="post.id"
                @comment-added="refreshUserPosts"
                @like-toggled="refreshUserPosts"
                @post-deleted="refreshUserPosts"
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
                          <img
                              v-if="getUserProfilePictureUrl(friend.profilePictureUrl)"
                              :src="getUserProfilePictureUrl(friend.profilePictureUrl)"
                              alt="Profile Picture"
                              @error="() => {}"
                          />
                          <v-icon v-else color="white">mdi-account</v-icon>
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


    <!-- Profile picture upload modal -->
    <ProfilePictureUpload
      :show="showProfilePictureModal"
      @close="showProfilePictureModal = false"
      @profile-picture-updated="onProfilePictureUpdated"
    />
  </div>
</template>

<script lang="ts">
import { authStore } from '../stores/auth';
import { postStore } from '../stores/post';
import { userStore } from '../stores/user';
import { chatStore } from '../stores/chat';
import axiosAuthenticated from '../plugin/axios';
import { userCacheService } from '../services/userCacheService';
import PostComponent from '../components/Post/PostComponent.vue';
import ProfilePictureUpload from '../components/Profile/ProfilePictureUpload.vue';
import { defineComponent } from 'vue';
import type { UserDetailDTO } from '../dtos/user/userDetailDTO';
import type { postDTO } from '../dtos/post/postDTO';

export default defineComponent({
  name: 'UserDetailView',
  props: ['username'],
  components: {
    PostComponent,
    ProfilePictureUpload
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
      showProfilePictureModal: false,
      imageError: false,
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
    },

    formattedProfilePictureUrl() {
      return this.getUserProfilePictureUrl(this.userDetail.profilePictureUrl);
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
        await this.fetchUserDataForPosts();
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

        // Emit event to refresh chat groups when accepting friend request
        this.$root.$emit('friendship-changed');

        await this.loadFriendsList();
      } catch (error) {
        // Handle errors silently
      }
    },

    async removeFriend() {
      try {
        await userStore().RemoveFriend(authStore().userId, this.userDetail.id);
        this.relation = 'NONE';

        // Clear the user from chat groups when unfriending
        const chatGroupsStore = chatStore();
        const existingGroupIndex = chatGroupsStore.currentChatGroups.findIndex(
          group => group.username === this.userDetail.username
        );

        if (existingGroupIndex !== -1) {
          chatGroupsStore.currentChatGroups.splice(existingGroupIndex, 1);
          console.log('Removed unfriended user from chat groups');

          // If this was the active chat, reset it
          if (chatGroupsStore.currentChatGroupId === chatGroupsStore.currentChatGroups[existingGroupIndex]?.chatId) {
            chatGroupsStore.currentChatGroupId = 0;
            chatGroupsStore.currentChatMessages = [];
          }
        }

        // Emit event to refresh chat groups in left sidebar
        this.$root.$emit('friendship-changed');

        await this.loadFriendsList();
      } catch (error) {
        // Handle errors silently
      }
    },

    async confirmRemoveFriend() {
      if (confirm(`Are you sure you want to remove ${this.userDetail.firstName} ${this.userDetail.lastName} from your friends? This action cannot be undone.`)) {
        await this.removeFriend();
      }
    },

    async openChat() {
      console.log('=== STARTING OPENCHAT ===');
      console.log('Current user ID:', authStore().userId);
      console.log('Target user:', this.userDetail);

      // First test if Chat API is reachable
      try {
        console.log('Testing Chat API connectivity...');
        const testResponse = await axiosAuthenticated.get('http://localhost:8095/api/v1/Chat');
        console.log('Chat API test response:', testResponse.data);
      } catch (error) {
        console.error('Chat API test failed:', error);
        console.error('Chat service might be down or unreachable');
      }

      try {
        const chatGroupsStore = chatStore();
        console.log('Getting chat groups for user...');
        await chatGroupsStore.getChatGroupsForUser();
        console.log('Current chat groups:', chatGroupsStore.currentChatGroups);

        let existingChatGroup = chatGroupsStore.currentChatGroups.find(
          group => group.username === this.userDetail.username
        );
        console.log('Existing chat group found:', existingChatGroup);

        if (!existingChatGroup) {
          console.log('Creating new chat group...');
          try {
            const response = await axiosAuthenticated.post(
              `http://localhost:8095/api/v1/Chat/CreateChatGroup?userAId=${authStore().userId}&userBId=${this.userDetail.id}`
            );
            console.log('Create chat group response:', response.data);

            const newChatGroup = response.data;

            existingChatGroup = {
              chatId: newChatGroup.id,
              username: this.userDetail.username,
              userId: this.userDetail.id,
              hasNewMessages: false
            };

            chatGroupsStore.currentChatGroups.unshift(existingChatGroup);
            console.log('New chat group created:', existingChatGroup);
          } catch (error) {
            console.error('Error creating chat group - API response:', error.response);
            console.error('Error creating chat group - full error:', error);

            // Create a fallback chat group to allow UI to work
            existingChatGroup = {
              chatId: Date.now(),
              username: this.userDetail.username,
              userId: this.userDetail.id,
              hasNewMessages: false
            };

            chatGroupsStore.currentChatGroups.unshift(existingChatGroup);
            console.log('Fallback chat group created:', existingChatGroup);
          }
        }

        console.log('Switching to chat group:', existingChatGroup);
        await chatGroupsStore.switchUserChat(existingChatGroup);
        console.log('Chat switched successfully');
        console.log('=== OPENCHAT COMPLETED ===');
      } catch (error) {
        console.error('=== OPENCHAT ERROR ===');
        console.error('Error in openChat:', error);
        console.error('Error details:', error.response || error.message);
      }
    },

    viewFriendProfile(username: string) {
      this.$router.push({ name: 'userDetail', params: { username } });
    },
    async onProfilePictureUpdated(newUrl: string) {
      this.userDetail.profilePictureUrl = newUrl;

      // If this is the current user's profile, update auth store too
      if (this.isOwnProfile) {
        authStore().updateProfilePicture(newUrl);
      }

      this.loadUserData(); // Refresh user data after upload
    },
    async refreshUserPosts() {
      try {
        this.userPosts = await postStore().GetPostsForUser(this.userDetail.id);
        await this.fetchUserDataForPosts();
      } catch (error) {
        console.error('Error refreshing user posts:', error);
      }
    },

    async fetchUserDataForPosts() {
      // Get unique user IDs from posts, comments, and likes
      const userIds = new Set<number>();
      const userIdToUsernameMap = new Map<number, string>();

      this.userPosts.forEach(post => {
        userIds.add(post.user.id);
        userIdToUsernameMap.set(post.user.id, post.user.username);

        if (post.comments) {
          post.comments.forEach(comment => {
            if (comment.user?.id && comment.user?.username) {
              userIds.add(comment.user.id);
              userIdToUsernameMap.set(comment.user.id, comment.user.username);
            }
          });
        }

        if (post.likes) {
          post.likes.forEach(like => {
            if (like.user?.id && like.user?.username) {
              userIds.add(like.user.id);
              userIdToUsernameMap.set(like.user.id, like.user.username);
            }
          });
        }
      });

      // Fetch user data for all unique user IDs
      const usersMap = new Map();

      for (const userId of userIds) {
        try {
          const username = userIdToUsernameMap.get(userId);
          if (username) {
            const userData = await userCacheService.getUserByUsername(username);
            if (userData) {
              usersMap.set(userId, userData);
            }
          }
        } catch (error) {
          console.log(`Could not fetch user data for ID ${userId}:`, error);
        }
      }

      // Update posts with complete user data
      this.userPosts.forEach(post => {
        const userData = usersMap.get(post.user.id);
        if (userData) {
          post.user = {
            ...post.user,
            firstName: userData.firstName,
            lastName: userData.lastName,
            profilePictureUrl: userData.profilePictureUrl
          };
        }

        // Update comment users
        if (post.comments) {
          post.comments.forEach(comment => {
            if (comment.user?.id) {
              const userData = usersMap.get(comment.user.id);
              if (userData) {
                comment.user = {
                  ...comment.user,
                  firstName: userData.firstName,
                  lastName: userData.lastName,
                  profilePictureUrl: userData.profilePictureUrl
                };
              }
            }
          });
        }

        // Update like users
        if (post.likes) {
          post.likes.forEach(like => {
            if (like.user?.id) {
              const userData = usersMap.get(like.user.id);
              if (userData) {
                like.user = {
                  ...like.user,
                  firstName: userData.firstName,
                  lastName: userData.lastName,
                  profilePictureUrl: userData.profilePictureUrl
                };
              }
            }
          });
        }
      });
    },

    getUserProfilePictureUrl(url: string) {
      // Handle profile pictures which should be served from identity service
      if (!url) return null;

      // Check if this is a profile picture path (correct path)
      if (url.startsWith('/uploads/profile-pictures/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${url}`
          : url;
      }

      // Handle any other uploads path - default to identity service for profile pics
      if (url.startsWith('/uploads/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${url}`
          : url;
      }

      // If it's already a full URL, return as is
      return url;
    },

    handleImageError() {
      console.error('Profile image failed to load:', this.formattedProfilePictureUrl);
      console.error('Original profile picture URL:', this.userDetail.profilePictureUrl);
      this.imageError = true;
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

/* Fix friend avatar images */
.friend-card .v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
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

  .profile-actions .d-flex {
    flex-direction: column;
    width: 100%;
    gap: 8px;
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

/* Menu styling */
.v-list-item {
  transition: background-color 0.2s ease;
}

.v-list-item:hover {
  background-color: rgba(139, 0, 0, 0.05);
}

.v-list-item[role="menuitem"]:hover .v-icon {
  color: #d32f2f !important; /* Bright red on hover */
}
.avatar-edit-btn {
  box-shadow: 0 2px 8px rgba(139, 0, 0, 0.15);
  border-radius: 50%;
  padding: 0;
  opacity: 0;
  transition: opacity 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}
.avatar-hover-group:hover .avatar-edit-btn {
  opacity: 1;
}
.profile-avatar img {
    width: 100%;
    height: 100%;
    object-fit: contain;
    border-radius: 50%;
    display: block;
}
</style>
