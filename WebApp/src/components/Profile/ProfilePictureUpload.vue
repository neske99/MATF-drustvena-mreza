<template>
  <v-dialog 
    v-model="isVisible" 
    max-width="500px" 
    persistent
    transition="dialog-transition"
  >
    <v-card class="profile-picture-modal" rounded="xl">
      <!-- Header -->
      <v-card-title class="modal-header pa-6 pb-4">
        <div class="d-flex align-center">
          <v-avatar size="40" color="matf-red" class="mr-3">
            <v-icon color="white">mdi-camera</v-icon>
          </v-avatar>
          <div>
            <h3 class="text-h6 font-weight-bold mb-0">Update Profile Picture</h3>
            <p class="text-caption text--secondary mb-0">Choose a new photo for your profile</p>
          </div>
        </div>
      </v-card-title>

      <v-divider></v-divider>

      <v-card-text class="pa-6">
        <!-- Current profile picture preview -->
        <div class="current-preview mb-6 text-center">
          <h4 class="text-subtitle-1 font-weight-medium mb-4 text--secondary">Current Profile Picture</h4>
          <v-avatar size="100" color="matf-red" class="current-avatar">
            <img v-if="currentProfilePicture" :src="currentProfilePicture" alt="Current Profile" />
            <v-icon v-else size="50" color="white">mdi-account</v-icon>
          </v-avatar>
        </div>

        <!-- File upload area -->
        <div class="upload-area mb-4">
          <div 
            class="upload-dropzone"
            :class="{ 'dragover': isDragOver, 'has-file': selectedFile }"
            @drop="handleDrop"
            @dragover.prevent="isDragOver = true"
            @dragleave="isDragOver = false"
            @click="triggerFileInput"
          >
            <!-- New file preview -->
            <div v-if="selectedFile && imagePreview" class="new-preview">
              <img :src="imagePreview" alt="New Profile Picture" class="preview-image" />
              <div class="overlay">
                <v-icon size="28" color="white">mdi-camera</v-icon>
                <p class="text-body-2 text-white mt-2 mb-0">Click to change</p>
              </div>
            </div>
            
            <!-- Upload prompt -->
            <div v-else class="upload-prompt text-center pa-6">
              <v-icon size="48" :color="isDragOver ? 'matf-red' : 'grey-lighten-1'" class="mb-3">
                {{ isDragOver ? 'mdi-cloud-upload' : 'mdi-camera-plus' }}
              </v-icon>
              <h4 class="text-subtitle-1 font-weight-medium mb-2">
                {{ isDragOver ? 'Drop your image here' : 'Choose Profile Picture' }}
              </h4>
              <p class="text-body-2 text--secondary mb-3">
                Drag and drop an image, or click to browse
              </p>
              <v-chip color="matf-red" variant="outlined" size="small">
                PNG, JPG, WEBP up to 5MB
              </v-chip>
            </div>
          </div>
          
          <!-- Hidden file input -->
          <input
            ref="fileInput"
            type="file"
            style="display: none"
            accept="image/*"
            @change="handleFileSelect"
          />
        </div>

        <!-- File info -->
        <div v-if="selectedFile" class="file-info">
          <v-card class="info-card" variant="outlined" rounded="lg">
            <v-card-text class="pa-4">
              <div class="d-flex align-center">
                <v-icon color="matf-red" class="mr-3">mdi-information</v-icon>
                <div class="flex-grow-1">
                  <p class="text-subtitle-2 font-weight-medium mb-1">{{ selectedFile.name }}</p>
                  <p class="text-caption text--secondary mb-0">
                    {{ formatFileSize(selectedFile.size) }} • {{ selectedFile.type }}
                  </p>
                </div>
                <v-btn 
                  icon 
                  variant="text" 
                  size="small"
                  color="error"
                  @click="removeFile"
                >
                  <v-icon>mdi-close</v-icon>
                </v-btn>
              </div>
            </v-card-text>
          </v-card>
        </div>

        <!-- Error message -->
        <v-alert
          v-if="errorMessage"
          type="error"
          variant="outlined"
          class="mt-4"
          closable
          @click:close="errorMessage = ''"
        >
          {{ errorMessage }}
        </v-alert>
      </v-card-text>

      <v-divider></v-divider>

      <!-- Actions -->
      <v-card-actions class="pa-6 pt-4">
        <v-spacer></v-spacer>
        
        <v-btn
          color="grey"
          variant="outlined"
          size="large"
          class="text-none mr-3"
          @click="closeModal"
          :disabled="uploading"
        >
          Cancel
        </v-btn>
        
        <v-btn
          color="matf-red"
          size="large"
          class="text-none px-6"
          @click="uploadProfilePicture"
          :disabled="!selectedFile || uploading"
          :loading="uploading"
          elevation="2"
        >
          <v-icon class="mr-2">mdi-upload</v-icon>
          Update Picture
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { userStore } from '@/stores/user';
import { authStore } from '@/stores/auth';

export default defineComponent({
  name: 'ProfilePictureUpload',
  props: {
    show: {
      type: Boolean,
      default: false
    }
  },
  emits: ['close', 'profile-picture-updated'],
  data() {
    return {
      selectedFile: null as File | null,
      imagePreview: null as string | null,
      uploading: false,
      errorMessage: '',
      isDragOver: false
    };
  },
  computed: {
    isVisible: {
      get() {
        return this.show;
      },
      set(value: boolean) {
        if (!value) {
          this.closeModal();
        }
      }
    },
    
    currentProfilePicture() {
      return this.getUserProfilePictureUrl(authStore().profilePictureUrl);
    }
  },
  methods: {
    closeModal() {
      this.selectedFile = null;
      this.imagePreview = null;
      this.errorMessage = '';
      this.isDragOver = false;
      this.$emit('close');
    },

    triggerFileInput() {
      (this.$refs.fileInput as HTMLInputElement).click();
    },

    handleFileSelect(event: Event) {
      const target = event.target as HTMLInputElement;
      const file = target.files?.[0];
      this.processFile(file);
    },

    handleDrop(event: DragEvent) {
      event.preventDefault();
      this.isDragOver = false;
      
      const file = event.dataTransfer?.files[0];
      this.processFile(file);
    },

    processFile(file: File | undefined) {
      if (!file) return;

      // Validate file type
      if (!file.type.startsWith('image/')) {
        this.errorMessage = 'Please select an image file (PNG, JPG, WEBP)';
        return;
      }

      // Validate file size (5MB limit)
      if (file.size > 5 * 1024 * 1024) {
        this.errorMessage = 'File size must be less than 5MB';
        return;
      }

      this.selectedFile = file;
      this.errorMessage = '';

      // Create preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    },

    removeFile() {
      this.selectedFile = null;
      this.imagePreview = null;
      this.errorMessage = '';
      
      // Reset file input
      const fileInput = this.$refs.fileInput as HTMLInputElement;
      if (fileInput) fileInput.value = '';
    },

    async uploadProfilePicture() {
      if (!this.selectedFile) return;

      try {
        this.uploading = true;
        this.errorMessage = '';

        const result = await userStore().UploadProfilePicture(this.selectedFile);
        
        // Update auth store with new profile picture
        authStore().updateProfilePicture(result);
        
        this.$emit('profile-picture-updated', result);
        this.closeModal();
        
      } catch (error) {
        console.error('Error uploading profile picture:', error);
        this.errorMessage = 'Failed to upload profile picture. Please try again.';
      } finally {
        this.uploading = false;
      }
    },

    formatFileSize(bytes: number): string {
      if (bytes === 0) return '0 Bytes';
      const k = 1024;
      const sizes = ['Bytes', 'KB', 'MB', 'GB'];
      const i = Math.floor(Math.log(bytes) / Math.log(k));
      return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    },

    getUserProfilePictureUrl(url: string) {
      if (!url) return null;
      
      if (url.startsWith('/uploads/profile-pictures/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${url}`
          : url;
      }
      
      if (url.startsWith('/uploads/')) {
        return import.meta.env.DEV
          ? `http://localhost:8094${url}`
          : url;
      }
      
      return url;
    },
  }
});
</script>

<style scoped>
.profile-picture-modal {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
}

.modal-header {
  background: linear-gradient(135deg, #FAFAFA 0%, #F0F0F0 100%);
  border-bottom: 1px solid rgba(139, 0, 0, 0.1);
}

.current-avatar {
  border: 4px solid white;
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.upload-dropzone {
  border: 2px dashed rgba(139, 0, 0, 0.3);
  border-radius: 16px;
  min-height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  background: linear-gradient(135deg, #FAFAFA 0%, #F5F5F5 100%);
}

.upload-dropzone:hover {
  border-color: rgba(139, 0, 0, 0.5);
  background: linear-gradient(135deg, #F0F0F0 0%, #EEEEEE 100%);
}

.upload-dropzone.dragover {
  border-color: #8B0000;
  background: linear-gradient(135deg, rgba(139, 0, 0, 0.05) 0%, rgba(139, 0, 0, 0.1) 100%);
  transform: scale(1.01);
}

.upload-dropzone.has-file {
  border-color: #4CAF50;
  background: linear-gradient(135deg, rgba(76, 175, 80, 0.05) 0%, rgba(76, 175, 80, 0.1) 100%);
}

.new-preview {
  position: relative;
  width: 100%;
  height: 100%;
  min-height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.preview-image {
  max-width: 150px;
  max-height: 150px;
  border-radius: 12px;
  object-fit: cover;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.4);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: 12px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.new-preview:hover .overlay {
  opacity: 1;
}

.upload-prompt {
  transition: all 0.3s ease;
}

.info-card {
  background: linear-gradient(135deg, #F8F8F8 0%, #F0F0F0 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
}

/* Profile Picture Styles */
.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

/* Button styling */
.v-btn {
  text-transform: none;
  font-weight: 600;
}

.v-btn:not(.v-btn--disabled) {
  opacity: 1 !important;
}

/* Animations */
.dialog-transition-enter-active,
.dialog-transition-leave-active {
  transition: all 0.3s ease;
}

.dialog-transition-enter-from,
.dialog-transition-leave-to {
  opacity: 0;
  transform: scale(0.9);
}

/* Mobile responsiveness */
@media (max-width: 600px) {
  .current-avatar {
    width: 80px !important;
    height: 80px !important;
  }
  
  .upload-dropzone {
    min-height: 140px;
  }
  
  .preview-image {
    max-width: 120px;
    max-height: 120px;
  }
  
  .upload-prompt {
    padding: 1rem !important;
  }
}
</style>