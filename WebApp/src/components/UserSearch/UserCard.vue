<template>
  <v-card 
    class="user-card" 
    elevation="2"
    rounded="lg"
    hover
    @click="viewProfile"
  >
    <!-- Card Header -->
    <v-card-text class="pa-4">
      <div class="d-flex align-center mb-3">
        <v-avatar size="56" color="matf-red" class="mr-3">
          <v-icon size="28" color="white">mdi-account</v-icon>
        </v-avatar>
        
        <div class="flex-grow-1">
          <h3 class="text-h6 font-weight-bold text--primary mb-1">
            {{ firstName }} {{ lastName }}
          </h3>
          <p class="text-body-2 text--secondary mb-1">
            @{{ text }}
          </p>
          <p class="text-caption text--secondary">
            MATF Student
          </p>
        </div>
        
        <!-- Status Badge -->
        <v-chip 
          size="small" 
          :color="getStatusColor()"
          variant="outlined"
          prepend-icon="mdi-school"
        >
          {{ getStatusText() }}
        </v-chip>
      </div>
      
      <!-- User Stats (Mock data for now) -->
      <div class="user-stats mb-3">
        <v-row dense>
          <v-col cols="4" class="text-center">
            <div class="stat-item">
              <h4 class="text-subtitle-2 font-weight-bold matf-red--text">{{ randomPosts }}</h4>
              <p class="text-caption text--secondary mb-0">Posts</p>
            </div>
          </v-col>
          <v-col cols="4" class="text-center">
            <div class="stat-item">
              <h4 class="text-subtitle-2 font-weight-bold matf-red--text">{{ randomFriends }}</h4>
              <p class="text-caption text--secondary mb-0">Friends</p>
            </div>
          </v-col>
          <v-col cols="4" class="text-center">
            <div class="stat-item">
              <h4 class="text-subtitle-2 font-weight-bold matf-red--text">{{ randomLikes }}</h4>
              <p class="text-caption text--secondary mb-0">Likes</p>
            </div>
          </v-col>
        </v-row>
      </div>
    </v-card-text>
    
    <!-- Card Actions -->
    <v-card-actions class="px-4 pb-4">
      <div class="w-100">
        <!-- Friend Request Actions -->
        <div v-if="showFriendRequestButton" class="mb-2">
          <v-btn
            color="matf-red"
            block
            class="text-none"
            @click.stop="sendRequest"
            prepend-icon="mdi-account-plus"
            :loading="sendingRequest"
          >
            Send Friend Request
          </v-btn>
        </div>
        
        <div v-if="showAcceptButton" class="mb-2">
          <div class="d-flex gap-2">
            <v-btn
              color="success"
              class="text-none flex-grow-1"
              @click.stop="acceptRequest"
              prepend-icon="mdi-check"
              :loading="acceptingRequest"
              :disabled="decliningRequest"
            >
              Accept
            </v-btn>
            <v-btn
              color="error"
              variant="outlined"
              class="text-none flex-grow-1"
              @click.stop="declineRequest"
              prepend-icon="mdi-close"
              :loading="decliningRequest"
              :disabled="acceptingRequest"
            >
              Decline
            </v-btn>
          </div>
        </div>
        
        <div v-if="showFriendsStatus" class="mb-2">
          <v-btn
            color="error"
            variant="outlined"
            block
            class="text-none"
            @click.stop="removeFriend"
            prepend-icon="mdi-account-minus"
            :loading="removingFriend"
          >
            Remove Friend
          </v-btn>
        </div>
        
        <div v-if="showRequestSentStatus" class="mb-2">
          <v-btn
            color="grey"
            variant="outlined"
            block
            class="text-none"
            disabled
          >
            <v-icon class="mr-2">mdi-clock</v-icon>
            Request Sent
          </v-btn>
        </div>
        
        <!-- Secondary Actions -->
        <div class="d-flex gap-2">
          <v-btn
            variant="outlined"
            color="matf-red"
            class="text-none flex-grow-1"
            @click.stop="viewProfile"
            prepend-icon="mdi-account"
          >
            Profile
          </v-btn>
          
          <!-- Only show Message button when users are friends -->
          <v-btn
            v-if="localRelation === 'FRIENDS'"
            color="matf-red"
            class="text-none flex-grow-1"
            @click.stop="startMessage"
            prepend-icon="mdi-message"
          >
            Message
          </v-btn>
        </div>
      </div>
    </v-card-actions>
  </v-card>
</template>

<script lang='ts'>
import { authStore } from '@/stores/auth';
import { userStore } from '@/stores/user';
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'UserCardComponent',
  props: {
    text: {
      type: String,
      required: true
    },
    firstName: {
      type: String,
      required: true
    },
    lastName: {
      type: String,
      required: true
    },
    userId: {
      type: Number,
      required: true
    },
    relation: {
      type: String,
      required: true
    }
  },
  emits: ['request-sent', 'request-accepted', 'request-declined', 'friendship-removed'],
  data() {
    return {
      localRelation: this.relation,
      sendingRequest: false,
      acceptingRequest: false,
      decliningRequest: false,
      removingFriend: false,
      randomPosts: Math.floor(Math.random() * 50) + 1,
      randomFriends: Math.floor(Math.random() * 100) + 5,
      randomLikes: Math.floor(Math.random() * 200) + 10
    }
  },
  computed: {
    showFriendRequestButton() {
      return this.localRelation === 'NONE' || this.localRelation === '' || !this.localRelation;
    },
    
    showAcceptButton() {
      return this.localRelation === 'RECEIVED_FRIENDSHIP_REQUEST_FROM';
    },
    
    showFriendsStatus() {
      return this.localRelation === 'FRIEND_WITH';
    },
    
    showRequestSentStatus() {
      return this.localRelation === 'REQUESTED_FRIENDSHIP_WITH';
    },
    
    canMessage() {
      return this.localRelation === 'FRIEND_WITH';
    }
  },
  watch: {
    relation: {
      immediate: true,
      handler(newVal) {
        this.localRelation = newVal;
      }
    }
  },
  methods: {
    async sendRequest() {
      try {
        this.sendingRequest = true;
        await userStore().SendFriendRequest(authStore().userId, this.userId);
        this.localRelation = 'REQUESTED_FRIENDSHIP_WITH';
        this.$emit('request-sent', this.userId);
        console.log('Friend request sent to:', this.text);
      } catch (error) {
        console.error('Error sending friend request:', error);
      } finally {
        this.sendingRequest = false;
      }
    },
    
    async acceptRequest() {
      try {
        this.acceptingRequest = true;
        await userStore().AcceptFriendRequest(authStore().userId, this.userId);
        this.localRelation = 'FRIENDS';
        this.$emit('request-accepted', this.userId);
        console.log('Friend request accepted from:', this.text);
      } catch (error) {
        console.error('Error accepting friend request:', error);
        // TODO: Show error message to user
      } finally {
        this.acceptingRequest = false;
      }
    },
    
    async declineRequest() {
      try {
        this.decliningRequest = true;
        await userStore().DeclineFriendRequest(authStore().userId, this.userId);
        this.localRelation = 'NONE';
        this.$emit('request-declined', this.userId);
        console.log('Friend request declined from:', this.text);
      } catch (error) {
        console.error('Error declining friend request:', error);
        // TODO: Show error message to user
      } finally {
        this.decliningRequest = false;
      }
    },
    
    async removeFriend() {
      try {
        this.removingFriend = true;
        await userStore().RemoveFriend(authStore().userId, this.userId);
        this.localRelation = 'NONE';
        this.$emit('friendship-removed', this.userId);
        console.log('Friendship removed with:', this.text);
      } catch (error) {
        console.error('Error removing friend:', error);
        // TODO: Show error message to user
      } finally {
        this.removingFriend = false;
      }
    },
    
    viewProfile() {
      this.$router.push(`/userdetail/${this.text}`);
    },
    
    startMessage() {
      if (this.canMessage) {
        console.log('Starting message with', this.text);
        // TODO: Implement messaging functionality
        // For now, just navigate to home and focus on chat
        this.$router.push('/home');
      }
    },
    
    getStatusColor() {
      switch (this.localRelation) {
        case 'FRIEND_WITH':
          return 'success';
        case 'REQUESTED_FRIENDSHIP_WITH':
          return 'warning';
        case 'RECEIVED_FRIENDSHIP_REQUEST_FROM':
          return 'info';
        default:
          return 'matf-red';
      }
    },
    
    getStatusText() {
      switch (this.localRelation) {
        case 'FRIEND_WITH':
          return 'Friend';
        case 'REQUESTED_FRIENDSHIP_WITH':
          return 'Pending';
        case 'RECEIVED_FRIENDSHIP_REQUEST_FROM':
          return 'Requests';
        default:
          return 'MATF';
      }
    }
  }
})
</script>

<style scoped>
.user-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: all 0.3s ease;
  cursor: pointer;
  animation: slideInUp 0.4s ease-out;
  border-radius: 8px;
}

.user-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 25px rgba(139, 0, 0, 0.15);
}

.stat-item {
  padding: 8px 4px;
  border-radius: 8px;
  background: rgba(139, 0, 0, 0.03);
  transition: background 0.2s ease;
}

.stat-item:hover {
  background: rgba(139, 0, 0, 0.08);
}

.user-stats {
  border: 1px solid rgba(139, 0, 0, 0.1);
  border-radius: 8px;
  padding: 8px;
  background: rgba(255, 255, 255, 0.8);
}

.v-btn {
  text-transform: none;
  font-weight: 600;
  border-radius: 8px;
}

.gap-2 > * + * {
  margin-left: 8px;
}

/* Card hover effects */
.user-card:hover .v-avatar {
  transform: scale(1.05);
  transition: transform 0.2s ease;
}

.user-card:hover .stat-item {
  transform: scale(1.02);
  transition: transform 0.2s ease;
}

/* Animations */
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

/* Mobile responsiveness */
@media (max-width: 600px) {
  .gap-2 {
    flex-direction: column;
    gap: 8px;
  }
  
  .gap-2 > * + * {
    margin-left: 0;
  }
  
  .user-card {
    margin-bottom: 1rem;
  }
}

/* Loading states */
.v-btn.v-btn--loading {
  pointer-events: none;
}

/* Prevent text selection on hover */
.user-card {
  user-select: none;
}
</style>
