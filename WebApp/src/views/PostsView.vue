<template>
  <div class="posts-page">
    <!-- Welcome Section -->
    <v-card 
      class="welcome-card mb-6" 
      elevation="2"
      rounded="lg"
    >
      <v-card-text class="pa-6">
        <div class="d-flex align-center">
          <v-avatar size="60" color="matf-red" class="mr-4">
            <img 
                v-if="currentUserProfilePicture" 
                :src="currentUserProfilePicture" 
                alt="Your Profile Picture"
                @error="() => {}"
            />
            <v-icon v-else size="32" color="white">mdi-account</v-icon>
          </v-avatar>
          <div class="flex-grow-1">
            <h2 class="text-h5 font-weight-bold matf-red--text mb-1">
              Welcome back, {{ username }}!
            </h2>
            <p class="text-body-1 text--secondary mb-0">
              Share your thoughts with the MATF community
            </p>
          </div>
          
          <!-- Quick Stats -->
          <div class="d-none d-md-flex align-center">
            <div class="text-center mr-4">
              <h3 class="text-h6 font-weight-bold matf-red--text">{{ posts.length }}</h3>
              <p class="text-caption text--secondary mb-0">Posts Today</p>
            </div>
            <div class="text-center">
              <h3 class="text-h6 font-weight-bold matf-red--text">{{ totalLikes }}</h3>
              <p class="text-caption text--secondary mb-0">Total Likes</p>
            </div>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Create Post Section -->
    <UploadPostComponent @post-uploaded="refreshPosts" class="mb-6" />

    <!-- Posts Feed Header -->
    <div class="feed-header mb-4">
      <h3 class="text-h6 font-weight-bold text--primary">
        <v-icon class="mr-2" color="matf-red">mdi-forum</v-icon>
        Community Feed
      </h3>
      <p class="text-body-2 text--secondary mb-0">
        Latest posts from the MATF community
      </p>
    </div>

    <!-- Posts Feed -->
    <div class="posts-feed">
      <!-- Loading State -->
      <div v-if="loading" class="loading-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <v-progress-circular 
            indeterminate 
            color="matf-red" 
            size="48"
            class="mb-4"
          />
          <h4 class="text-h6 font-weight-medium text--secondary mb-2">Loading posts...</h4>
          <p class="text-body-2 text--secondary">Getting the latest updates from your community</p>
        </v-card>
      </div>

      <!-- Empty State -->
      <div v-else-if="posts.length === 0" class="empty-state">
        <v-card class="pa-8 text-center" elevation="1" rounded="lg">
          <div class="empty-icon mb-4">
            <v-icon size="80" color="grey-lighten-1">mdi-post-outline</v-icon>
          </div>
          <h3 class="text-h5 font-weight-bold text--secondary mb-3">No posts yet</h3>
          <p class="text-body-1 text--secondary mb-4">
            Be the first to share something with the MATF community!
          </p>
          <v-btn 
            color="matf-red" 
            size="large"
            class="text-none px-6"
            @click="scrollToCreatePost"
          >
            <v-icon class="mr-2">mdi-plus</v-icon>
            Create First Post
          </v-btn>
        </v-card>
      </div>

      <!-- Posts List -->
      <div v-else class="posts-list">
        <TransitionGroup name="post" tag="div">
          <PostComponent 
            v-for="post in posts" 
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
            @comment-added="refreshPosts"
            @like-toggled="refreshPosts"
            @post-deleted="refreshPosts"
            class="mb-4"
          />
        </TransitionGroup>
        
        <!-- Load More Section -->
        <div class="load-more-section text-center mt-6">
          <v-card class="pa-4" elevation="1" rounded="lg">
            <p class="text-body-2 text--secondary mb-3">
              You've reached the end of the feed
            </p>
            <v-btn 
              color="matf-red" 
              variant="outlined"
              class="text-none"
              @click="scrollToTop"
            >
              <v-icon class="mr-2">mdi-arrow-up</v-icon>
              Back to Top
            </v-btn>
          </v-card>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { authStore } from '@/stores/auth';
import { postStore } from '@/stores/post';
import { userStore } from '@/stores/user';
import { userCacheService } from '@/services/userCacheService';
import PostComponent from '@/components/Post/PostComponent.vue';
import UploadPostComponent from '@/components/Post/UploadPostComponent.vue';
import { defineComponent } from 'vue';
import type { postDTO } from '../dtos/post/postDTO.ts';

export default defineComponent({
  name: 'PostsView',
  components: {
    PostComponent,
    UploadPostComponent
  },
  data() {
    return {
      username: '',
      posts: [] as postDTO[],
      loading: true
    };
  },
  async created() {
    await this.loadInitialData();
  },
  mounted() {
    this.isLoading = true;
    try {
      postStore().GetPosts().then((posts) => {
        this.posts = posts;
        this.fetchUserDataForPosts();
      });
    } catch (error) {
      console.error('Error loading posts:', error);
    } finally {
      this.isLoading = false;
    }
  },
  computed: {
    totalLikes() {
      return this.posts.reduce((sum, post) => sum + (post.likes?.length || 0), 0);
    },

    currentUserProfilePicture() {
      return this.getUserProfilePictureUrl(authStore().profilePictureUrl);
    }
  },
  methods: {
    async loadInitialData() {
      try {
        this.loading = true;
        this.username = authStore().username;
        
        // Load posts and friend requests in parallel
        const [posts] = await Promise.all([
          postStore().GetPosts(),
          this.loadFriendRequests()
        ]);
        
        this.posts = posts;
        console.log('Posts loaded:', this.posts);
      } catch (error) {
        console.error('Error loading initial data:', error);
      } finally {
        this.loading = false;
      }
    },
    
    async loadFriendRequests() {
      try {
        await userStore().GetFriendRequests();
      } catch (error) {
        console.error('Error loading friend requests:', error);
      }
    },
    
    async loadPosts() {
      try {
        this.loading = true;
        this.username = authStore().username;
        this.posts = await postStore().GetPosts();
        console.log('Posts refreshed:', this.posts);
      } catch (error) {
        console.error('Error loading posts:', error);
      } finally {
        this.loading = false;
      }
    },
    
    async refreshPosts() {
      try {
        this.posts = await postStore().GetPosts();
        await this.fetchUserDataForPosts();
      } catch (error) {
        console.error('Error refreshing posts:', error);
      }
    },

    async fetchUserDataForPosts() {
      // Get unique user IDs from posts, comments, and likes
      const userIds = new Set<number>();
      const userIdToUsernameMap = new Map<number, string>();
      
      this.posts.forEach(post => {
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
      this.posts.forEach(post => {
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
    
    scrollToCreatePost() {
      const createPostElement = document.querySelector('.upload-post-card');
      if (createPostElement) {
        createPostElement.scrollIntoView({ behavior: 'smooth' });
      }
    },
    
    scrollToTop() {
      window.scrollTo({ top: 0, behavior: 'smooth' });
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
    }
  }
});
</script>

<style scoped>
.posts-page {
  animation: fadeInUp 0.6s ease-out;
}

.welcome-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.welcome-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.feed-header {
  padding: 0 8px;
  margin-bottom: 1rem;
}

.loading-state,
.empty-state {
  animation: slideInUp 0.5s ease-out;
}

.empty-icon {
  animation: float 3s ease-in-out infinite;
}

.load-more-section {
  animation: slideInUp 0.5s ease-out;
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

/* Profile Picture Styles */
.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

/* Mobile Responsiveness */
@media (max-width: 768px) {
  .welcome-card .v-card-text {
    padding: 1.5rem;
  }
  
  .feed-header {
    padding: 0 4px;
  }
  
  .posts-page {
    animation: none; /* Reduce animations on mobile */
  }
}

@media (max-width: 600px) {
  .welcome-card .d-flex {
    flex-direction: column;
    text-align: center;
  }
  
  .welcome-card .mr-4 {
    margin-right: 0 !important;
    margin-bottom: 1rem;
  }
}

/* Scroll behavior */
html {
  scroll-behavior: smooth;
}
</style>
