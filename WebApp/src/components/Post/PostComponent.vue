<template>
  <v-card 
    class="post-card" 
    elevation="2" 
    rounded="lg"
  >
    <!-- Post Header -->
    <v-card-title class="post-header pa-4">
      <div class="d-flex align-center w-100">
        <v-avatar size="48" color="matf-red" class="mr-3">
          <v-icon color="white">mdi-account</v-icon>
        </v-avatar>
        
        <div class="flex-grow-1">
          <router-link 
            :to="'/userdetail/' + username" 
            class="username-link text-decoration-none"
          >
            <h3 class="text-h6 font-weight-bold matf-red--text mb-0">
              {{ username }}
            </h3>
          </router-link>
          <p class="text-caption text--secondary mb-0">
            MATF Student • Just now
          </p>
        </div>
        
        <v-menu offset-y>
          <template v-slot:activator="{ props }">
            <v-btn 
              icon 
              variant="text" 
              size="small"
              v-bind="props"
            >
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-item>
              <v-list-item-title>
                <v-icon class="mr-2">mdi-bookmark</v-icon>
                Save Post
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title>
                <v-icon class="mr-2">mdi-share</v-icon>
                Share
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </div>
    </v-card-title>

    <!-- Post Content -->
    <v-card-text class="post-content px-4 pb-2">
      <p class="text-body-1 post-text">
        {{ text }}
      </p>
    </v-card-text>

    <!-- Post Actions -->
    <v-card-actions class="post-actions px-4 py-2">
      <div class="d-flex align-center w-100">
        <!-- Like Button -->
        <v-btn 
          :color="userHasLiked ? 'red' : 'grey'" 
          variant="text"
          size="small"
          class="text-none mr-2"
          @click="toggleLike"
          :loading="likeLoading"
        >
          <v-icon 
            :class="userHasLiked ? 'mdi-heart' : 'mdi-heart-outline'" 
            size="20"
            class="mr-1"
          ></v-icon>
          {{ userHasLiked ? 'Liked' : 'Like' }}
        </v-btn>
        
        <!-- Comment Button -->
        <v-btn 
          color="grey" 
          variant="text"
          size="small"
          class="text-none mr-2"
          @click="toggleCommentInput"
        >
          <v-icon class="mr-1" size="20">mdi-comment-outline</v-icon>
          Comment
        </v-btn>
        
        <v-spacer></v-spacer>
        
        <!-- Like Count -->
        <v-menu v-if="likes && likes.length > 0" offset-y>
          <template v-slot:activator="{ props }">
            <v-chip 
              size="small" 
              color="red" 
              variant="text"
              class="likes-chip"
              v-bind="props"
            >
              <v-icon size="16" class="mr-1">mdi-heart</v-icon>
              {{ likes.length }}
            </v-chip>
          </template>
          <v-list dense max-width="200">
            <v-list-subheader>Liked by</v-list-subheader>
            <v-list-item v-for="like in likes" :key="like.id">
              <v-list-item-title v-if="like.user">
                <router-link 
                  :to="'/userdetail/'+ like.user.username"
                  class="text-decoration-none matf-red--text"
                >
                  {{ like.user.username }}
                </router-link>
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </div>
    </v-card-actions>

    <v-divider class="mx-4"></v-divider>

    <!-- Comments Section -->
    <div class="comments-section">
      <!-- Comment Input -->
      <div v-if="showCommentInput" class="comment-input pa-4">
        <div class="d-flex align-start">
          <v-avatar size="32" color="matf-red" class="mr-3 flex-shrink-0">
            <v-icon color="white" size="18">mdi-account</v-icon>
          </v-avatar>
          
          <div class="flex-grow-1">
            <v-text-field
              v-model.trim="newComment"
              placeholder="Write a comment..."
              variant="outlined"
              color="matf-red"
              density="compact"
              hide-details
              class="comment-field"
              @keydown.enter="addComment"
            ></v-text-field>
            
            <div class="d-flex justify-end mt-2">
              <v-btn
                color="matf-red"
                size="small"
                class="text-none"
                @click="addComment"
                :disabled="!newComment.trim()"
                :loading="commentLoading"
              >
                <v-icon class="mr-1" size="16">mdi-send</v-icon>
                Post
              </v-btn>
            </div>
          </div>
        </div>
      </div>

      <!-- Comments List -->
      <div v-if="comments && comments.length > 0" class="comments-list pa-4 pt-2">
        <h4 class="text-subtitle-2 text--secondary mb-3">
          Comments ({{ comments.length }})
        </h4>
        
        <div v-for="comment in comments" :key="comment.id" class="comment-item mb-3">
          <div class="d-flex align-start">
            <v-avatar size="32" color="matf-red" class="mr-3 flex-shrink-0">
              <v-icon color="white" size="18">mdi-account</v-icon>
            </v-avatar>
            
            <div class="comment-content flex-grow-1">
              <div class="comment-bubble">
                <div class="d-flex align-center mb-1">
                  <router-link 
                    v-if="comment.user" 
                    :to="'/userdetail/'+ comment.user.username"
                    class="text-decoration-none"
                  >
                    <span class="text-subtitle-2 font-weight-bold matf-red--text">
                      {{ comment.user.username }}
                    </span>
                  </router-link>
                  <span class="text-caption text--secondary ml-2">
                    Just now
                  </span>
                </div>
                <p class="text-body-2 mb-0">{{ comment.text }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- No Comments State -->
      <div v-else-if="!showCommentInput" class="no-comments pa-4 pt-2 text-center">
        <p class="text-body-2 text--secondary mb-2">No comments yet</p>
        <v-btn 
          color="matf-red" 
          variant="outlined" 
          size="small"
          class="text-none"
          @click="toggleCommentInput"
        >
          Be the first to comment
        </v-btn>
      </div>
    </div>
  </v-card>
</template>

<script lang='ts'>
import { postStore } from '@/stores/post';
import { authStore } from '@/stores/auth';
import { defineComponent } from 'vue'
import type { likeDTO } from '@/dtos/post/likeDTO';

export default defineComponent({
  name: 'PostComponent',
  props: ['text', 'username', 'comments', 'likes', 'id'],
  data() {
    return {
      newComment: '',
      showCommentInput: false,
      commentLoading: false,
      likeLoading: false
    }
  },
  computed: {
    userHasLiked(): boolean {
      if (!this.likes || !Array.isArray(this.likes)) return false;
      const currentUserId = authStore().userId;
      return this.likes.some((like: likeDTO) => like.user?.id === currentUserId);
    }
  },
  methods: {
    toggleCommentInput() {
      this.showCommentInput = !this.showCommentInput;
      if (!this.showCommentInput) {
        this.newComment = '';
      }
    },
    
    async addComment() {
      if (this.newComment.trim() === '') return;
      
      try {
        this.commentLoading = true;
        await postStore().AddComment(this.newComment, this.id);
        this.newComment = '';
        this.$emit('comment-added');
      } catch (error) {
        console.error('Error adding comment:', error);
      } finally {
        this.commentLoading = false;
      }
    },
    
    async toggleLike() {
      try {
        this.likeLoading = true;
        if (this.userHasLiked) {
          await postStore().RemoveLike(this.id);
        } else {
          await postStore().AddLike(this.id);
        }
        this.$emit('like-toggled');
      } catch (error) {
        console.error('Error toggling like:', error);
      } finally {
        this.likeLoading = false;
      }
    }
  }
})
</script>

<style scoped>
.post-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: all 0.3s ease;
  animation: slideInUp 0.5s ease-out;
}

.post-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.15);
}

.post-header {
  border-bottom: 1px solid rgba(0, 0, 0, 0.05);
  background: rgba(255, 255, 255, 0.5);
}


.post-text {
  line-height: 1.6;
  color: #212121;
}

.post-actions {
  background: rgba(139, 0, 0, 0.02);
}

.likes-chip {
  cursor: pointer;
}

.likes-chip:hover {
  background: rgba(244, 67, 54, 0.1) !important;
}

.comment-field .v-field {
  border-radius: 20px;
  background: #F5F5F5;
}

.comment-field .v-field:hover {
  background: #EEEEEE;
}

.comment-field .v-field--focused {
  background: #FFFFFF;
}

.comment-bubble {
  background: #F5F5F5;
  border-radius: 16px;
  padding: 12px 16px;
  border: 1px solid rgba(0, 0, 0, 0.05);
}

.comment-item:last-child {
  margin-bottom: 0;
}

.no-comments {
  background: rgba(139, 0, 0, 0.02);
  border-radius: 0 0 16px 16px;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
}

/* Animations */
@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.comment-input {
  animation: slideDown 0.3s ease-out;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Mobile responsiveness */
@media (max-width: 600px) {
  .post-header,
  .post-content,
  .post-actions,
  .comment-input,
  .comments-list {
    padding-left: 12px;
    padding-right: 12px;
  }
  
  .post-actions {
    flex-wrap: wrap;
  }
  
  .likes-chip {
    margin-top: 8px;
  }
}

.username-link {
    text-decoration: none;
    color: inherit;
}

.username-link .matf-red--text {
    color: #000000 !important;
    transition: color 0.2s;
}

.username-link:hover .matf-red--text {
    color: #660000 !important;
}
</style>
