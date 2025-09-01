<template>
    <v-card class="post-card" elevation="2" rounded="lg">
        <!-- Post Header -->
        <v-card-title class="post-header pa-4">
            <div class="d-flex align-center w-100">
                <v-avatar size="48" color="matf-red" class="mr-3">
                    <img 
                        v-if="formattedProfilePictureUrl && !imageError" 
                        :src="formattedProfilePictureUrl" 
                        alt="Profile Picture"
                        @error="handleImageError"
                    />
                    <v-icon v-else color="white">mdi-account</v-icon>
                </v-avatar>

                <div class="flex-grow-1">
                    <router-link :to="'/userdetail/' + username" class="username-link text-decoration-none">
                        <h3 class="text-h6 font-weight-bold matf-red--text mb-0">{{ username }}</h3>
                    </router-link>
                    <p class="text-caption text--secondary mb-0">
                        {{ getDisplayName() }} - {{ getTimeAgo(createdDate) }}
                    </p>
                </div>

                <v-menu offset-y>
                    <template v-slot:activator="{ props }">
                        <v-btn icon variant="text" size="small" v-bind="props">
                            <v-icon>mdi-dots-vertical</v-icon>
                        </v-btn>
                    </template>
                    <v-list>
                        <v-list-item v-if="canDeletePost" @click="confirmDeletePost">
                            <v-list-item-title>
                                <v-icon class="mr-2" color="error">mdi-delete</v-icon>
                                Delete Post
                            </v-list-item-title>
                        </v-list-item>
                    </v-list>
                </v-menu>
            </div>
        </v-card-title>

        <!-- Post Content -->
        <v-card-text class="post-content px-4 pb-2">
            <p class="text-body-1 post-text">{{ text }}</p>
            
            <!-- File attachment display -->
            <div v-if="fileUrl" class="post-file-attachment mt-3">
                <!-- Image files -->
                <div v-if="isImageFile(fileUrl, fileType)" class="attachment-image">
                    <img :src="getFileUrl(fileUrl)" alt="Attachment" class="attachment-preview-image" @click="openImageModal" />
                    <div class="attachment-info mt-2">
                        <v-chip size="small" color="success" variant="outlined">
                            <v-icon size="14" class="mr-1">mdi-image</v-icon>
                            Image
                        </v-chip>
                        <v-btn variant="text" size="small" @click="downloadFile">
                            <v-icon size="16" class="mr-1">mdi-download</v-icon>
                            Download
                        </v-btn>
                    </div>
                </div>

                <!-- Other file types -->
                <div v-else class="attachment-document">
                    <v-card class="attachment-card" elevation="1" rounded="lg">
                        <v-card-text class="pa-4">
                            <div class="d-flex align-center">
                                <v-avatar size="48" :color="getFileColor(fileUrl, fileType)" class="mr-3">
                                    <v-icon color="white" size="24">{{ getFileIcon(fileUrl, fileType) }}</v-icon>
                                </v-avatar>
                                <div class="flex-grow-1">
                                    <h4 class="text-subtitle-1 font-weight-bold mb-1">
                                        {{ fileName || getFileTypeLabel(fileUrl, fileType) }}
                                    </h4>
                                    <p class="text-caption text--secondary mb-0">{{ getFileTypeLabel(fileUrl, fileType) }}</p>
                                </div>
                                <div class="attachment-actions">
                                    <v-btn v-if="isPdfFile(fileUrl, fileType)" variant="outlined" size="small" class="mr-2" @click="viewFile">
                                        <v-icon size="16" class="mr-1">mdi-eye</v-icon>
                                        View
                                    </v-btn>
                                    <v-btn variant="outlined" size="small" @click="downloadFile">
                                        <v-icon size="16" class="mr-1">mdi-download</v-icon>
                                        Download
                                    </v-btn>
                                </div>
                            </div>
                        </v-card-text>
                    </v-card>
                </div>
            </div>

            <!-- Image Modal -->
            <v-dialog v-model="showImageModal" max-width="80%" max-height="80%">
                <v-card>
                    <v-card-title class="d-flex justify-space-between align-center">
                        <span>{{ fileName || 'Image' }}</span>
                        <v-btn icon variant="text" @click="showImageModal = false">
                            <v-icon>mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="text-center">
                        <img :src="getFileUrl(fileUrl)" alt="Full size image" style="max-width: 100%; max-height: 70vh; object-fit: contain;" />
                    </v-card-text>
                    <v-card-actions class="justify-center">
                        <v-btn color="matf-red" @click="downloadFile">
                            <v-icon class="mr-2">mdi-download</v-icon>
                            Download
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-card-text>

        <!-- Post Actions -->
        <v-card-actions class="post-actions px-4 py-2">
            <div class="d-flex align-center w-100">
                <v-btn :color="userHasLiked ? 'red' : 'grey'" variant="text" size="small" class="text-none mr-2" @click="toggleLike" :loading="likeLoading">
                    <v-icon :class="userHasLiked ? 'mdi-heart' : 'mdi-heart-outline'" size="20" class="mr-1"></v-icon>
                    {{ userHasLiked ? 'Liked' : 'Like' }}
                </v-btn>

                <v-btn color="grey" variant="text" size="small" class="text-none mr-2" @click="toggleCommentInput">
                    <v-icon class="mr-1" size="20">mdi-comment-outline</v-icon>
                    Comment
                </v-btn>

                <v-spacer></v-spacer>

                <v-menu v-if="likes && likes.length > 0" offset-y>
                    <template v-slot:activator="{ props }">
                        <v-chip size="small" color="red" variant="text" class="likes-chip" v-bind="props">
                            <v-icon size="16" class="mr-1">mdi-heart</v-icon>
                            {{ likes.length }}
                        </v-chip>
                    </template>
                    <v-list dense max-width="200">
                        <v-list-subheader>Liked by</v-list-subheader>
                        <v-list-item v-for="like in likes" :key="like.id">
                            <v-list-item-title v-if="like.user">
                                <router-link :to="'/userdetail/'+ like.user.username" class="text-decoration-none matf-red--text">
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
                        <img v-if="getCurrentUserProfilePicture()" :src="getCurrentUserProfilePicture()" alt="Your Profile Picture" @error="() => {}" />
                        <v-icon v-else color="white" size="18">mdi-account</v-icon>
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
                            <v-btn color="matf-red" size="small" class="text-none" @click="addComment" :disabled="!newComment.trim()" :loading="commentLoading">
                                <v-icon class="mr-1" size="16">mdi-send</v-icon>
                                Post
                            </v-btn>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Comments List -->
            <div v-if="comments && comments.length > 0" class="comments-list pa-4 pt-2">
                <h4 class="text-subtitle-2 text--secondary mb-3">Comments ({{ comments.length }})</h4>

                <div v-for="comment in comments" :key="comment.id" class="comment-item mb-3">
                    <div class="d-flex align-start">
                        <v-avatar size="32" color="matf-red" class="mr-3 flex-shrink-0">
                            <img v-if="comment.user && getUserProfilePictureUrl(comment.user.profilePictureUrl)" 
                                 :src="getUserProfilePictureUrl(comment.user.profilePictureUrl)" 
                                 alt="Profile Picture" 
                                 @error="() => {}" />
                            <v-icon v-else color="white" size="18">mdi-account</v-icon>
                        </v-avatar>

                        <div class="comment-content flex-grow-1">
                            <div class="comment-bubble">
                                <div class="d-flex align-center mb-1">
                                    <router-link v-if="comment.user" :to="'/userdetail/'+ comment.user.username" class="text-decoration-none">
                                        <span class="text-subtitle-2 font-weight-bold matf-red--text">{{ comment.user.username }}</span>
                                    </router-link>
                                    <span class="text-caption text--secondary ml-2">{{ getTimeAgo(comment.createdDate) }}</span>
                                </div>
                                <p class="text-body-2 mb-0">{{ comment.text }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- No Comments State -->
            <div v-if="(!comments || comments.length === 0) && !showCommentInput" class="no-comments pa-4 pt-2 text-center">
                <p class="text-body-2 text--secondary mb-2">No comments yet</p>
                <v-btn color="matf-red" variant="outlined" size="small" class="text-none" @click="toggleCommentInput">
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
    props: [
        'text', 'username', 'comments', 'likes', 'id',
        'fileUrl', 'fileName', 'fileType', 'createdDate', 'userId', 'userProfilePictureUrl',
        'firstName', 'lastName'
    ],
    data() {
        return {
            newComment: '',
            showCommentInput: false,
            commentLoading: false,
            likeLoading: false,
            showImageModal: false,
            imageError: false
        }
    },
    computed: {
        userHasLiked(): boolean {
            if (!this.likes || !Array.isArray(this.likes)) return false;
            const currentUserId = authStore().userId;
            return this.likes.some((like: likeDTO) => like.user?.id === currentUserId);
        },

        canDeletePost(): boolean {
            const currentUserId = authStore().userId;
            return this.userId === currentUserId;
        },

        formattedProfilePictureUrl(): string | null {
            // Use the profilePictureUrl passed from parent (fetched from Identity Service)
            return this.getUserProfilePictureUrl(this.userProfilePictureUrl);
        },

        profileImageError(): boolean {
            return this.imageError;
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
                alert('Failed to add comment. Please try again.');
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
        },

        getFileUrl(url: string) {
            if (url && url.startsWith('/uploads')) {
                return import.meta.env.DEV ? `http://localhost:8080${url}` : url;
            }
            return url;
        },

        getUserProfilePictureUrl(url: string) {
            if (!url) return null;
            
            if (url.startsWith('/uploads/profile-pictures/')) {
                return import.meta.env.DEV ? `http://localhost:8094${url}` : url;
            }
            
            if (url.startsWith('/uploads/')) {
                return import.meta.env.DEV ? `http://localhost:8094${url}` : url;
            }
            
            return url;
        },

        isImageFile(url: string, fileType?: string): boolean {
            const imageTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/bmp', 'image/webp', 'image/svg+xml'];
            const imageExtensions = /\.(jpg|jpeg|png|gif|bmp|webp|svg)$/i;
            return (fileType && imageTypes.includes(fileType)) || imageExtensions.test(url);
        },

        isPdfFile(url: string, fileType?: string): boolean {
            return fileType === 'application/pdf' || /\.pdf$/i.test(url);
        },

        getFileColor(url: string, fileType?: string): string {
            if (this.isPdfFile(url, fileType)) return 'red';
            if (fileType?.includes('word')) return 'blue';
            if (fileType?.includes('excel')) return 'green';
            if (fileType?.includes('powerpoint')) return 'orange';
            return 'grey';
        },

        getFileIcon(url: string, fileType?: string): string {
            if (this.isPdfFile(url, fileType)) return 'mdi-file-pdf-box';
            if (fileType?.includes('word')) return 'mdi-file-word-box';
            if (fileType?.includes('excel')) return 'mdi-file-excel-box';
            if (fileType?.includes('powerpoint')) return 'mdi-file-powerpoint-box';
            return 'mdi-file-outline';
        },

        getFileTypeLabel(url: string, fileType?: string): string {
            if (this.isPdfFile(url, fileType)) return 'PDF Document';
            if (fileType?.includes('word')) return 'Word Document';
            if (fileType?.includes('excel')) return 'Excel Spreadsheet';
            if (fileType?.includes('powerpoint')) return 'PowerPoint Presentation';
            return 'Document';
        },

        openImageModal() {
            this.showImageModal = true;
        },

        viewFile() {
            const fullUrl = this.getFileUrl(this.fileUrl);
            window.open(fullUrl, '_blank');
        },

        downloadFile() {
            const fullUrl = this.getFileUrl(this.fileUrl);
            const link = document.createElement('a');
            link.href = fullUrl;
            link.download = this.fileName || 'download';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },

        getTimeAgo(dateString: string): string {
            if (!dateString) return 'Unknown time';
            
            const now = new Date();
            const postDate = new Date(dateString + (dateString.includes('Z') ? '' : 'Z'));
            const diffMs = now.getTime() - postDate.getTime();
            const diffMins = Math.floor(diffMs / (1000 * 60));
            const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
            const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

            if (diffMins < 1) return 'Just now';
            if (diffMins < 60) return `${diffMins}m ago`;
            if (diffHours < 24) return `${diffHours}h ago`;
            if (diffDays < 7) return `${diffDays}d ago`;
            
            return postDate.toLocaleDateString();
        },

        async confirmDeletePost() {
            if (confirm('Are you sure you want to delete this post? This action cannot be undone.')) {
                await this.deletePost();
            }
        },

        async deletePost() {
            try {
                await postStore().DeletePost(this.id);
                this.$emit('post-deleted');
            } catch (error) {
                console.error('Error deleting post:', error);
                alert('Failed to delete post. Please try again.');
            }
        },

        handleImageError() {
            this.imageError = true;
        },

        getCurrentUserProfilePicture() {
            const currentUser = authStore();
            return this.getUserProfilePictureUrl(currentUser.profilePictureUrl);
        },

        getDisplayName() {
            // This will be populated by PostsView with data from Identity Service
            return (this.firstName && this.lastName) 
                ? `${this.firstName} ${this.lastName}` 
                : 'MATF Student';
        },
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

.username-link .matf-red--text {
    color: #000000 !important;
    transition: color 0.2s;
}

.username-link:hover .matf-red--text {
    color: #660000 !important;
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

.no-comments {
  background: rgba(139, 0, 0, 0.02);
  border-radius: 0 0 16px 16px;
}

.post-file-attachment {
  border: 1px solid rgba(139, 0, 0, 0.1);
  border-radius: 12px;
  padding: 12px;
  background: linear-gradient(135deg, #FAFAFA 0%, #F5F5F5 100%);
}

.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

.attachment-preview-image {
  max-width: 100%;
  max-height: 300px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(139, 0, 0, 0.08);
  cursor: pointer;
  transition: transform 0.2s ease;
}

.attachment-preview-image:hover {
  transform: scale(1.02);
}

.attachment-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: all 0.2s ease;
}

.attachment-card:hover {
  box-shadow: 0 2px 12px rgba(139, 0, 0, 0.1);
  transform: translateY(-1px);
}

.attachment-info {
  display: flex;
  align-items: center;
  gap: 8px;
}

.attachment-actions {
  display: flex;
  gap: 8px;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
}

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

@media (max-width: 600px) {
  .post-header,
  .post-content,
  .post-actions,
  .comment-input,
  .comments-list {
    padding-left: 12px;
    padding-right: 12px;
  }
  
  .attachment-actions {
    flex-direction: column;
    gap: 4px;
  }

  .attachment-actions .v-btn {
    width: 100%;
  }
}
</style>