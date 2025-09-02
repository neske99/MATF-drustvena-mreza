<template>
    <v-card class="upload-post-card"
            elevation="2"
            rounded="lg">
        <v-card-text class="pa-6">
            <div class="d-flex align-start">
                <v-avatar size="56" color="matf-red" class="mr-4 flex-shrink-0">
                    <img 
                        v-if="currentUserProfilePicture" 
                        :src="currentUserProfilePicture" 
                        alt="Your Profile Picture"
                        @error="() => {}"
                    />
                    <v-icon v-else color="white" size="28">mdi-account</v-icon>
                </v-avatar>

                <div class="flex-grow-1">
                    <v-textarea v-model.trim="newPost"
                                placeholder="What's on your mind? Share your thoughts with the MATF community..."
                                variant="outlined"
                                color="matf-red"
                                rows="4"
                                auto-grow
                                hide-details
                                class="post-input mb-4"
                                :disabled="loading"></v-textarea>

                    <!-- File attachment preview -->
                    <div v-if="attachedFile" class="file-preview mb-4">
                        <v-card class="preview-card" elevation="1" rounded="lg">
                            <v-card-text class="pa-4">
                                <div class="d-flex align-center">
                                    <!-- File icon based on type -->
                                    <v-avatar size="48" :color="getFileColor()" class="mr-3">
                                        <v-icon color="white" size="24">{{ getFileIcon() }}</v-icon>
                                    </v-avatar>
                                    
                                    <!-- File info -->
                                    <div class="flex-grow-1">
                                        <h4 class="text-subtitle-1 font-weight-bold mb-1">
                                            {{ attachedFile.name }}
                                        </h4>
                                        <p class="text-caption text--secondary mb-0">
                                            {{ getFileTypeLabel() }} • {{ formatFileSize(attachedFile.size) }}
                                        </p>
                                    </div>
                                    
                                    <!-- Remove file button -->
                                    <v-btn 
                                        icon 
                                        variant="text" 
                                        size="small"
                                        color="error"
                                        @click="removeFile"
                                        :disabled="loading"
                                    >
                                        <v-icon>mdi-close</v-icon>
                                    </v-btn>
                                </div>
                                
                                <!-- Image preview for image files -->
                                <div v-if="isImageFile() && imagePreview" class="image-preview mt-3">
                                    <img 
                                        :src="imagePreview" 
                                        alt="Preview" 
                                        class="preview-image"
                                    />
                                </div>
                            </v-card-text>
                        </v-card>
                    </div>

                    <!-- Action bar -->
                    <div class="action-bar d-flex align-center">
                        <!-- File attachment button -->
                        <v-btn
                            variant="text"
                            color="matf-red"
                            size="large"
                            class="text-none mr-3"
                            @click="triggerFileInput"
                            :disabled="loading"
                        >
                            <v-icon class="mr-2">mdi-paperclip</v-icon>
                            Attach File
                        </v-btn>
                        
                        <!-- Hidden file input -->
                        <input
                            ref="fileInput"
                            type="file"
                            style="display: none"
                            accept="*/*"
                            @change="handleFileSelect"
                        />

                        <v-spacer></v-spacer>

                        <!-- Action buttons -->
                        <div class="action-buttons d-flex">
                            <v-btn variant="outlined"
                                   color="grey"
                                   size="large"
                                   class="text-none px-4 mr-4"
                                   @click="clearPost"
                                   :disabled="(!newPost && !attachedFile) || loading">
                                <v-icon class="mr-2">mdi-close</v-icon>
                                Clear
                            </v-btn>

                            <v-btn color="matf-red"
                                   size="large"
                                   class="text-none px-8"
                                   @click="uploadPost"
                                   :disabled="(!newPost.trim() && !attachedFile) || loading"
                                   :loading="loading"
                                   elevation="2">
                                <v-icon class="mr-2">mdi-send</v-icon>
                                Share Post
                            </v-btn>
                        </div>
                    </div>
                </div>
            </div>
        </v-card-text>
    </v-card>
</template>

<script lang='ts'>
    import { authStore } from '@/stores/auth';
    import { postStore } from '@/stores/post';
    import { defineComponent } from 'vue'

    export default defineComponent({
        name: 'UploadPostComponent',
        data() {
            return {
                newPost: '',
                attachedFile: null as File | null,
                imagePreview: null as string | null,
                loading: false
            }
        },
        computed: {
            currentUserProfilePicture() {
                return this.getUserProfilePictureUrl(authStore().profilePictureUrl);
            }
        },
        methods: {
            async uploadPost() {
                if (!this.newPost.trim() && !this.attachedFile) return;
                try {
                    this.loading = true;
                    const userId = authStore().userId;
                    const formData = new FormData();
                    formData.append('text', this.newPost);
                    formData.append('userId', userId);
                    if (this.attachedFile) {
                        formData.append('file', this.attachedFile);
                    }
                    await postStore().UploadPost(formData);
                    this.newPost = '';
                    this.attachedFile = null;
                    this.$emit('post-uploaded');
                } catch (error) {
                    console.error('Error uploading post:', error);
                } finally {
                    this.loading = false;
                }
            },
            clearPost() {
                this.newPost = '';
                this.attachedFile = null;
                this.imagePreview = null;
            },

            triggerFileInput() {
                (this.$refs.fileInput as HTMLInputElement).click();
            },

            handleFileSelect(event: Event) {
                const target = event.target as HTMLInputElement;
                const file = target.files?.[0];
                
                if (file) {
                    this.attachedFile = file;
                    
                    // Create image preview for image files
                    if (this.isImageFile()) {
                        const reader = new FileReader();
                        reader.onload = (e) => {
                            this.imagePreview = e.target?.result as string;
                        };
                        reader.readAsDataURL(file);
                    } else {
                        this.imagePreview = null;
                    }
                }
            },

            removeFile() {
                this.attachedFile = null;
                this.imagePreview = null;
                // Reset file input
                const fileInput = this.$refs.fileInput as HTMLInputElement;
                if (fileInput) fileInput.value = '';
            },

            // File type detection methods
            isImageFile(): boolean {
                if (!this.attachedFile) return false;
                const imageTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/bmp', 'image/webp'];
                return imageTypes.includes(this.attachedFile.type);
            },

            isPdfFile(): boolean {
                if (!this.attachedFile) return false;
                return this.attachedFile.type === 'application/pdf';
            },

            isWordFile(): boolean {
                if (!this.attachedFile) return false;
                const wordTypes = [
                    'application/msword',
                    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
                ];
                return wordTypes.includes(this.attachedFile.type);
            },

            isExcelFile(): boolean {
                if (!this.attachedFile) return false;
                const excelTypes = [
                    'application/vnd.ms-excel',
                    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                ];
                return excelTypes.includes(this.attachedFile.type);
            },

            isPowerPointFile(): boolean {
                if (!this.attachedFile) return false;
                const ppTypes = [
                    'application/vnd.ms-powerpoint',
                    'application/vnd.openxmlformats-officedocument.presentationml.presentation'
                ];
                return ppTypes.includes(this.attachedFile.type);
            },

            getFileIcon(): string {
                if (!this.attachedFile) return 'mdi-file';
                
                if (this.isImageFile()) return 'mdi-image';
                if (this.isPdfFile()) return 'mdi-file-pdf-box';
                if (this.isWordFile()) return 'mdi-file-word-box';
                if (this.isExcelFile()) return 'mdi-file-excel-box';
                if (this.isPowerPointFile()) return 'mdi-file-powerpoint-box';
                
                const fileName = this.attachedFile.name.toLowerCase();
                if (fileName.endsWith('.txt')) return 'mdi-file-document-outline';
                if (fileName.match(/\.(zip|rar|7z)$/)) return 'mdi-zip-box';
                if (fileName.match(/\.(mp3|wav|ogg)$/)) return 'mdi-music-box';
                if (fileName.match(/\.(mp4|avi|mov|wmv)$/)) return 'mdi-video-box';
                
                return 'mdi-file-outline';
            },

            getFileColor(): string {
                if (!this.attachedFile) return 'grey';
                
                if (this.isImageFile()) return 'green';
                if (this.isPdfFile()) return 'red';
                if (this.isWordFile()) return 'blue';
                if (this.isExcelFile()) return 'green';
                if (this.isPowerPointFile()) return 'orange';
                
                return 'grey';
            },

            getFileTypeLabel(): string {
                if (!this.attachedFile) return 'File';
                
                if (this.isImageFile()) return 'Image';
                if (this.isPdfFile()) return 'PDF Document';
                if (this.isWordFile()) return 'Word Document';
                if (this.isExcelFile()) return 'Excel Spreadsheet';
                if (this.isPowerPointFile()) return 'PowerPoint Presentation';
                
                const fileName = this.attachedFile.name.toLowerCase();
                if (fileName.endsWith('.txt')) return 'Text Document';
                if (fileName.match(/\.(zip|rar|7z)$/)) return 'Archive';
                if (fileName.match(/\.(mp3|wav|ogg)$/)) return 'Audio';
                if (fileName.match(/\.(mp4|avi|mov|wmv)$/)) return 'Video';
                
                return 'Document';
            },

            formatFileSize(bytes: number): string {
                if (bytes === 0) return '0 Bytes';
                const k = 1024;
                const sizes = ['Bytes', 'KB', 'MB', 'GB'];
                const i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
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
        }
    })
</script>

<style scoped>
/* Upload Post Card */
.upload-post-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: all 0.3s ease;
  animation: slideInUp 0.5s ease-out;
}

.upload-post-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.post-input .v-field {
  border-radius: 16px;
  background: #FAFAFA;
  font-size: 16px;
}

.post-input .v-field:hover {
  background: #F5F5F5;
}

.post-input .v-field--focused {
  background: #FFFFFF;
}

/* File Preview Styles */
.file-preview {
  border: 2px solid rgba(139, 0, 0, 0.1);
  border-radius: 16px;
  background: linear-gradient(135deg, #FAFAFA 0%, #F0F0F0 100%);
}

.preview-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
}

.preview-image {
  max-width: 100%;
  max-height: 120px;
  border-radius: 12px;
  object-fit: cover;
  box-shadow: 0 2px 12px rgba(139, 0, 0, 0.1);
  cursor: pointer;
  transition: transform 0.2s ease;
}

.preview-image:hover {
  transform: scale(1.02);
}

.action-bar {
  border-top: 1px solid rgba(139, 0, 0, 0.1);
  padding-top: 1.5rem;
}

/* Button Styling */
.v-btn {
  text-transform: none;
  font-weight: 600;
}

.v-btn:not(.v-btn--disabled) {
  opacity: 1 !important;
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

/* Profile Picture Styles */
.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

/* Mobile Responsiveness */
@media (max-width: 600px) {
  .upload-post-card .v-card-text {
    padding: 1.5rem;
  }
  
  .d-flex.align-start {
    flex-direction: column;
  }
  
  .mr-4 {
    margin-right: 0 !important;
    margin-bottom: 1rem;
    align-self: center;
  }
  
  .action-bar {
    flex-direction: column;
    gap: 1rem;
  }
  
  .action-bar .v-btn {
    width: 100%;
  }
}
</style>