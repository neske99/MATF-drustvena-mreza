<template>
  <v-app>
    <!-- Modern University Header -->
    <TheNavbar v-if="isAuthenticated" />

    <!-- Main Layout with Sidebars -->
    <div v-if="isAuthenticated" class="app-layout">
      <!-- Left Sidebar - Navigation & Friends -->
      <TheLeftSidebar class="left-sidebar" />
      
      <!-- Main Content Area -->
      <v-main class="main-content">
        <div class="content-wrapper">
          <router-view />
        </div>
      </v-main>
      
      <!-- Right Sidebar - Chat -->
      <TheRightSidebar class="right-sidebar" />
    </div>

    <!-- Auth Pages (Login/Signup) -->
    <v-main v-else class="auth-main">
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import TheNavbar from './components/TheNavbar.vue';
import TheLeftSidebar from './components/TheLeftSidebar.vue';
import TheRightSidebar from './components/TheRightSidebar.vue';
import { authStore } from './stores/auth';

export default defineComponent({
  name: 'App',
  components: {
    TheNavbar,
    TheRightSidebar,
    TheLeftSidebar
  },
  computed: {
    isAuthenticated() {
      const auth = authStore();
      const authenticated = auth.isAuthenticated;
      console.log('App.vue isAuthenticated computed - value:', authenticated);
      console.log('App.vue auth store state:', {
        username: auth.username,
        userId: auth.userId,
        hasAccessToken: !!auth.accessToken
      });
      return authenticated;
    }
  }
})
</script>

<style scoped>
.app-layout {
  display: flex;
  min-height: 100vh;
  background: linear-gradient(135deg, #FAFAFA 0%, #F5F5F5 100%);
  margin-top: 64px; /* Account for fixed navbar height */
}

.left-sidebar {
  width: 280px;
  flex-shrink: 0;
  border-right: 1px solid rgba(139, 0, 0, 0.1);
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(10px);
  height: calc(100vh - 64px);
  overflow-y: auto;
  position: fixed;
  top: 64px;
  left: 0;
}

.main-content {
  flex: 1;
  padding: 0;
  overflow-y: auto;
  margin-left: 280px;
  margin-right: 320px;
  max-height: calc(100vh - 64px);
}

.content-wrapper {
  max-width: 1200px; /* Increased from 800px to 1200px */
  margin: 0 auto;
  padding: 24px 32px; /* Increased horizontal padding */
}

.right-sidebar {
  width: 320px;
  flex-shrink: 0;
  border-left: 1px solid rgba(139, 0, 0, 0.1);
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(10px);
  height: calc(100vh - 64px);
  overflow-y: auto;
  position: fixed;
  top: 64px;
  right: 0;
}

.auth-main {
  padding: 0;
}

/* Mobile Responsiveness */
@media (max-width: 1200px) {
  .right-sidebar {
    display: none;
  }
  
  .main-content {
    margin-right: 0;
  }
  
  .content-wrapper {
    max-width: 900px; /* Adjust for medium screens */
    padding: 24px;
  }
}

@media (max-width: 768px) {
  .left-sidebar {
    display: none;
  }
  
  .main-content {
    margin-left: 0;
    margin-right: 0;
  }
  
  .content-wrapper {
    max-width: none; /* Full width on mobile */
    padding: 16px;
  }
}

/* Smooth transitions */
.left-sidebar,
.right-sidebar,
.main-content {
  transition: all 0.3s ease;
}
</style>
