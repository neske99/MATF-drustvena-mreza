<template>
  <v-app>
    <v-main>
      <v-container fluid class="pa-0 fill-height auth-container">
        <v-row no-gutters class="fill-height">
          <!-- Left Side - Hero Section -->
          <v-col cols="12" md="7" class="d-none d-md-flex hero-section">
            <div class="hero-content">
              <div class="hero-text">
                <!-- MATF Logo -->
                <div class="university-logo mb-6">
                  <div class="logo-container">
                    <v-icon size="60" color="white" class="mb-2">mdi-school</v-icon>
                    <h3 class="text-h5 font-weight-bold text-white mb-1">MATF</h3>
                    <p class="text-caption text-white opacity-80">Faculty of Mathematics</p>
                    <p class="text-caption text-white opacity-60">University of Belgrade</p>
                  </div>
                </div>

                <h1 class="display-1 font-weight-bold text-white mb-4">
                  Welcome back to MATF Social
                </h1>
                <p class="text-h6 text-white opacity-90 mb-6">
                  Connect with colleagues, share knowledge and stay updated with events at the Faculty of Mathematics.
                </p>
                <div class="feature-list">
                  <div class="feature-item mb-3">
                    <v-icon color="white" class="mr-3">mdi-heart</v-icon>
                    <span class="text-white">Like and comment on posts</span>
                  </div>
                  <div class="feature-item mb-3">
                    <v-icon color="white" class="mr-3">mdi-chat</v-icon>
                    <span class="text-white">Exchange messages in real-time</span>
                  </div>
                  <div class="feature-item mb-3">
                    <v-icon color="white" class="mr-3">mdi-book-open-variant</v-icon>
                    <span class="text-white">Share materials and experiences</span>
                  </div>
                  <div class="feature-item">
                    <v-icon color="white" class="mr-3">mdi-account-group</v-icon>
                    <span class="text-white">Build your academic network</span>
                  </div>
                </div>
              </div>
            </div>
          </v-col>

          <!-- Right Side - Login Form -->
          <v-col cols="12" md="5" class="d-flex align-center justify-center auth-form-section">
            <div class="auth-form-container">
              <div class="text-center mb-8">
                <v-avatar size="80" color="matf-red" class="mb-4">
                  <v-icon size="40" color="white">mdi-account</v-icon>
                </v-avatar>
                <h2 class="text-h4 font-weight-bold matf-red--text mb-2">Welcome back!</h2>
                <p class="text-body-1 text--secondary">Sign in to access your account</p>
              </div>

              <v-card flat class="auth-card pa-8" elevation="0">
                <v-form @submit.prevent="onLoginClick" ref="loginForm">
                  <v-text-field
                    v-model="username"
                    label="Username"
                    prepend-inner-icon="mdi-account"
                    variant="outlined"
                    color="matf-red"
                    class="mb-4"
                    :rules="[rules.required]"
                    hide-details="auto"
                  />

                  <v-text-field
                    v-model="password"
                    label="Password"
                    type="password"
                    prepend-inner-icon="mdi-lock"
                    variant="outlined"
                    color="matf-red"
                    class="mb-6"
                    :rules="[rules.required]"
                    hide-details="auto"
                  />

                  <v-btn
                    type="submit"
                    color="matf-red"
                    size="large"
                    block
                    class="mb-4 text-none"
                    :loading="loading"
                    elevation="2"
                  >
                    Sign In
                  </v-btn>

                  <div class="text-center mb-4">
                    <v-btn
                      variant="text"
                      color="matf-red"
                      size="small"
                      class="text-none"
                    >
                      Forgot password?
                    </v-btn>
                  </div>

                  <div class="text-center mb-4">
                    <v-divider class="mb-4">
                      <span class="text-caption text--secondary px-2">OR</span>
                    </v-divider>
                  </div>

                  <div class="text-center">
                    <p class="text-body-2 text--secondary mb-4">
                      New to MATF Social?
                    </p>
                    <v-btn
                      to="/auth/signup"
                      variant="outlined"
                      color="matf-red"
                      size="large"
                      block
                      class="text-none"
                    >
                      Create new account
                    </v-btn>
                  </div>
                </v-form>
              </v-card>
              
            </div>
          </v-col>
        </v-row>

        <!-- Error Snackbar -->
        <v-snackbar
          v-model="showError"
          color="error"
          timeout="5000"
          location="top"
        >
          {{ errorMessage }}
          <template #actions>
            <v-btn variant="text" @click="showError = false">Close</v-btn>
          </template>
        </v-snackbar>
      </v-container>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { authStore } from '../../stores/auth.ts'
import { chatStore } from '@/stores/chat.ts';
import { userStore } from '@/stores/user.ts';


export default defineComponent({
  name: 'LoginView',
  data() {
    return {
      username: '',
      password: '',
      loading: false,
      showError: false,
      errorMessage: '',
      rules: {
        required: (v: string) => !!v || 'This field is required'
      }
    };
  },
  methods: {
    async onLoginClick() {
      // Validate form
      const { valid } = await (this.$refs.loginForm as any).validate();
      if (!valid) return;

      this.loading = true;
      this.showError = false;

      try {
        const store = authStore();
        await store.login(this.username, this.password);
      } catch (err: any) {
        this.showError = true;
        this.errorMessage = err.response?.data?.message || 'Invalid username or password. Please try again.';
      } finally {
        this.loading = false;
      }
    }
  }
});
</script>

<style scoped>
.auth-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #660000 0%, #8B0000 100%);
}

.hero-section {
  background: linear-gradient(135deg, #660000 0%, #8B0000 100%);
  position: relative;
  overflow: hidden;
}

.hero-section::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="university-pattern" width="40" height="40" patternUnits="userSpaceOnUse"><circle cx="20" cy="20" r="1" fill="white" opacity="0.1"/><circle cx="10" cy="30" r="0.5" fill="white" opacity="0.05"/><circle cx="30" cy="10" r="0.5" fill="white" opacity="0.05"/></pattern></defs><rect width="100" height="100" fill="url(%23university-pattern)"/></svg>');
}

.hero-content {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 2rem;
}

.hero-text {
  max-width: 500px;
}

.university-logo {
  text-align: center;
}

.logo-container {
  background: rgba(255, 255, 255, 0.15);
  border-radius: 16px;
  padding: 1.5rem;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  display: inline-block;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}

.feature-item {
  display: flex;
  align-items: center;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 8px;
  padding: 0.75rem;
  backdrop-filter: blur(5px);
}

.auth-form-section {
  background: #ffffff;
  position: relative;
}

.auth-form-container {
  width: 100%;
  max-width: 400px;
  padding: 2rem 1rem;
}

.auth-card {
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(10px);
  border-radius: 16px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 8px 32px rgba(139, 0, 0, 0.1);
}

.v-btn {
  border-radius: 8px;
  font-weight: 600;
  text-transform: none;
  letter-spacing: normal;
}

.v-text-field {
  margin-bottom: 0;
}

.v-text-field .v-field {
  border-radius: 8px;
}

/* Mobile Styles */
@media (max-width: 960px) {
  .auth-container {
    background: linear-gradient(135deg, #660000 0%, #8B0000 100%);
  }
  
  .auth-form-section {
    background: transparent;
  }
  
  .auth-form-container {
    padding: 2rem 1.5rem;
  }
  
  .auth-card {
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
  }
}

/* Animation */
.auth-form-container {
  animation: fadeInUp 0.6s ease-out;
}

.university-logo {
  animation: fadeIn 0.8s ease-out;
}

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

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.9);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

.text-caption {
  font-size: 0.75rem;
  line-height: 1.2;
}
</style>
