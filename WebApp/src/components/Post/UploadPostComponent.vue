<template>
  <v-card 
    class="upload-post-card" 
    elevation="2" 
    rounded="lg"
  >
    <v-card-text class="pa-6">
      <div class="d-flex align-start">
        <v-avatar size="48" color="matf-red" class="mr-4 flex-shrink-0">
          <v-icon color="white">mdi-account</v-icon>
        </v-avatar>
        
        <div class="flex-grow-1">
          <v-textarea
            v-model.trim="newPost"
            placeholder="What's on your mind? Share your thoughts with the MATF community..."
            variant="outlined"
            color="matf-red"
            rows="3"
            auto-grow
            hide-details
            class="post-input"
          ></v-textarea>
          
          <v-card-actions class="px-0 pt-4 pb-0">
            <v-spacer></v-spacer>
            
            <div class="d-flex gap-2">
              <v-btn
                variant="outlined"
                color="grey"
                size="large"
                class="text-none"
                @click="clearPost"
                :disabled="!newPost || loading"
              >
                <v-icon class="mr-2">mdi-close</v-icon>
                Clear
              </v-btn>
              
              <v-btn
                color="matf-red"
                size="large"
                class="text-none px-6"
                @click="uploadPost"
                :disabled="!newPost.trim() || loading"
                :loading="loading"
                elevation="2"
              >
                <v-icon class="mr-2">mdi-send</v-icon>
                Share Post
              </v-btn>
            </div>
          </v-card-actions>
          
 
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
      loading: false
    }
  },
  methods: {
    async uploadPost() {
      if (this.newPost.trim() === '') return;
      
      try {
        this.loading = true;
        const userId = authStore().userId;
        await postStore().UploadPost(this.newPost, userId);
        this.newPost = '';
        this.$emit('post-uploaded');
      } catch (error) {
        console.error('Error uploading post:', error);
        // TODO: Show error message to user
      } finally {
        this.loading = false;
      }
    },
    clearPost() {
      this.newPost = '';
    }
  }
})
</script>

<style scoped>
.upload-post-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.upload-post-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.15);
}

.post-input .v-field {
  border-radius: 12px;
  background: #FAFAFA;
}

.post-input .v-field:hover {
  background: #F5F5F5;
}

.post-input .v-field--focused {
  background: #FFFFFF;
}

.post-guidelines {
  border-left: 3px solid #8B0000;
  padding-left: 12px;
  margin-left: 4px;
  background: rgba(139, 0, 0, 0.05);
  border-radius: 0 8px 8px 0;
  padding: 8px 12px;
}

.v-btn {
  border-radius: 8px;
  font-weight: 600;
  text-transform: none;
  letter-spacing: normal;
}

.gap-2 > * + * {
  margin-left: 8px;
}

/* Animation */
.upload-post-card {
  animation: slideInUp 0.5s ease-out;
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

/* Mobile styles */
@media (max-width: 600px) {
  .v-card-text {
    padding: 1rem;
  }
  
  .d-flex.gap-2 {
    flex-direction: column;
    gap: 8px;
  }
  
  .gap-2 > * + * {
    margin-left: 0;
  }
}
</style>
