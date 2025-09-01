<template>
  <v-app-bar 
    app
    color="matf-red" 
    dark 
    height="64"
    class="university-header"
    elevation="2"
    fixed
  >
    <v-container fluid class="px-6">
      <div class="d-flex align-center w-100">
        <!-- University Brand -->
        <div class="d-flex align-center flex-shrink-0" style="min-width: 200px;">
          <v-avatar size="40" color="white" class="mr-3">
            <v-icon color="matf-red" size="24">mdi-school</v-icon>
          </v-avatar>
          <div>
            <h2 class="text-h6 font-weight-bold mb-0">MATF Social</h2>
            <p class="text-caption mb-0 opacity-80">Faculty of Mathematics</p>
          </div>
        </div>

        <!-- Navigation Links -->
        <div class="d-flex align-center mx-4 flex-shrink-0">
          <v-btn 
            to="/home"
            variant="text" 
            color="white" 
            class="text-none mr-2 nav-btn"
            prepend-icon="mdi-home"
            :class="{ 'active-nav': $route.path === '/home' }"
          >
            Home
          </v-btn>
          
          <v-btn 
            :to="`/userDetail/${currentUsername}`"
            variant="text" 
            color="white" 
            class="text-none mr-2 nav-btn"
            prepend-icon="mdi-account"
            :class="{ 'active-nav': $route.path.includes('/userDetail/') }"
          >
            Profile
          </v-btn>
        </div>

        <!-- Centered Search Bar -->
        <div class="flex-grow-1 d-flex justify-center px-4">
          <v-text-field
            v-model.trim="searchText"
            placeholder="Search for students, professors, subjects..."
            prepend-inner-icon="mdi-magnify"
            variant="outlined"
            density="compact"
            bg-color="rgba(255, 255, 255, 0.1)"
            color="white"
            class="search-field"
            style="max-width: 450px; width: 100%;"
            hide-details
            @keydown.enter="search"
            clearable
          />
        </div>

        <!-- Right Actions -->
        <div class="d-flex align-center flex-shrink-0" style="min-width: 200px; justify-content: flex-end;">
          <!-- Friend Requests -->
          <v-btn 
            to="/friendRequests"
            variant="text" 
            color="white" 
            class="text-none mr-2 nav-btn"
            :class="{ 'active-nav': $route.path === '/friendRequests' }"
          >
            <v-badge 
              v-if="numFriendRequests > 0"
              :content="numFriendRequests" 
              color="error"
              overlap
            >
              <v-icon>mdi-account-multiple-plus</v-icon>
            </v-badge>
            <v-icon v-else>mdi-account-multiple-plus</v-icon>
            <span class="ml-1 d-none d-lg-inline">Requests</span>
          </v-btn>

          <!-- User Menu -->
          <v-menu offset-y>
            <template v-slot:activator="{ props }">
              <v-btn 
                variant="text" 
                color="white" 
                class="text-none user-menu-btn"
                v-bind="props"
              >
                <v-avatar size="32" class="mr-2">
                  <img 
                    v-if="getCurrentUserProfilePicture()" 
                    :src="getCurrentUserProfilePicture()" 
                    alt="Your Profile Picture"
                    @error="() => {}"
                    class="navbar-avatar-image"
                  />
                  <v-icon v-else>mdi-account</v-icon>
                </v-avatar>
                <span class="d-none d-md-inline">{{ currentUsername }}</span>
                <v-icon class="ml-1">mdi-chevron-down</v-icon>
              </v-btn>
            </template>
            
            <v-list class="user-menu">
              <v-list-item :to="`/userDetail/${currentUsername}`">
                <v-list-item-title>
                  <v-icon class="mr-3">mdi-account</v-icon>
                  My Profile
                </v-list-item-title>
              </v-list-item>
              
              <v-list-item to="/settings">
                <v-list-item-title>
                  <v-icon class="mr-3">mdi-cog</v-icon>
                  Settings
                </v-list-item-title>
              </v-list-item>
              
              <v-divider />
              
              <v-list-item @click="onSignoutClick" class="logout-item">
                <v-list-item-title>
                  <v-icon class="mr-3">mdi-logout</v-icon>
                  Logout
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </div>
      </div>
    </v-container>
  </v-app-bar>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import { chatStore } from '@/stores/chat.ts'

export default defineComponent({
  name: 'TheNavbar',
  data() {
    return {
      searchText: "" as string
    }
  },
  computed: {
    isAuthenticated() {
      return authStore().isAuthenticated;
    },
    
    currentUsername() {
      return authStore().username;
    },

    numFriendRequests() {
      return userStore().numFriendRequests;
    }
  },
  methods: {
    async onSignoutClick() {
      authStore().logout();
    },
    
    search() {
      if (this.searchText.trim() !== "") {
        this.$router.push(`/usersearch/${this.searchText.trim()}`);
        this.searchText = "";
      }
    },

    getCurrentUserProfilePicture() {
      const profilePictureUrl = authStore().profilePictureUrl;
      if (!profilePictureUrl) return null;
      
      if (profilePictureUrl.startsWith('/uploads/profile-pictures/')) {
        return import.meta.env.DEV ? `http://localhost:8094${profilePictureUrl}` : profilePictureUrl;
      }
      
      if (profilePictureUrl.startsWith('/uploads/')) {
        return import.meta.env.DEV ? `http://localhost:8094${profilePictureUrl}` : profilePictureUrl;
      }
      
      return profilePictureUrl;
    }
  }
})
</script>

<style scoped>
.university-header {
  background: linear-gradient(135deg, #8B0000 0%, #660000 100%) !important;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

/* Consistent border radius for all elements */
.search-field :deep(.v-field) {
  border-radius: 8px !important;
  backdrop-filter: blur(10px);
  transition: all 0.2s ease;
}

.search-field :deep(.v-field):hover {
  background: rgba(255, 255, 255, 0.15);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.search-field :deep(.v-field__input) {
  color: white;
}

.search-field :deep(.v-field__input::placeholder) {
  color: rgba(255, 255, 255, 0.7);
}

/* Navigation buttons with consistent styling */
.nav-btn {
  border-radius: 8px !important;
}

.active-nav {
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(10px);
  border-radius: 8px !important;
}

.user-menu {
  min-width: 200px;
  border-radius: 8px;
  border: 1px solid rgba(139, 0, 0, 0.1);
}

.user-menu-btn {
  border-radius: 8px !important;
  padding: 4px 12px;
}

.logout-item {
  color: #d32f2f;
}

.logout-item:hover {
  background: rgba(211, 47, 47, 0.1);
}

.v-btn {
  text-transform: none;
  font-weight: 600;
  border-radius: 8px !important;
}

.v-badge :deep(.v-badge__badge) {
  font-size: 10px;
  height: 18px;
  min-width: 18px;
}

/* Mobile Responsiveness */
@media (max-width: 1200px) {
  .d-none.d-lg-inline {
    display: none !important;
  }
}

@media (max-width: 768px) {
  .university-header .v-container {
    padding: 0 12px;
  }
  
  .search-field {
    max-width: 250px !important;
  }
  
  .mx-4 {
    margin: 0 8px !important;
  }
  
  .flex-shrink-0[style*="min-width: 200px"] {
    min-width: 150px !important;
  }
}

@media (max-width: 600px) {
  .search-field {
    max-width: 180px !important;
  }
  
  .text-h6 {
    font-size: 0.9rem !important;
  }
  
  .text-caption {
    font-size: 0.65rem !important;
  }
  
  .nav-btn .v-btn__prepend {
    margin-inline-end: 4px !important;
  }
}

/* Profile Picture Styles - Fix image fitting */
.navbar-avatar-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

.v-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

/* Animations */
.v-btn {
  transition: all 0.2s ease;
}

.v-btn:hover {
  transform: translateY(-1px);
}

.active-nav {
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

/* Search field animations */
.search-field {
  transition: all 0.3s ease;
}

.search-field:focus-within {
  transform: scale(1.02);
}
</style>
