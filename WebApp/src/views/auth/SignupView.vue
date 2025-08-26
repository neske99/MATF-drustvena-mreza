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
                  </div>
                </div>
                
                <h1 class="display-1 font-weight-bold text-white mb-4">
                  Join the Student Community
                </h1>
                <p class="text-h6 text-white opacity-90 mb-6">
                  Create your account and start connecting with colleagues, sharing experiences, and discovering academic content at the Faculty of Mathematics.
                </p>
                <div class="stats-grid">
                  <div class="stat-item">
                    <v-icon size="32" color="white" class="mb-2">mdi-account-group</v-icon>
                    <h3 class="text-h5 font-weight-bold text-white">500+</h3>
                    <p class="text-body-2 text-white opacity-80">Students</p>
                  </div>
                  <div class="stat-item">
                    <v-icon size="32" color="white" class="mb-2">mdi-book-open-page-variant</v-icon>
                    <h3 class="text-h5 font-weight-bold text-white">1000+</h3>
                    <p class="text-body-2 text-white opacity-80">Posts</p>
                  </div>
                  <div class="stat-item">
                    <v-icon size="32" color="white" class="mb-2">mdi-handshake</v-icon>
                    <h3 class="text-h5 font-weight-bold text-white">2000+</h3>
                    <p class="text-body-2 text-white opacity-80">Connections</p>
                  </div>
                </div>
                
                
              </div>
            </div>
          </v-col>

          <!-- Right Side - Signup Form -->
          <v-col cols="12" md="5" class="d-flex align-center justify-center auth-form-section">
            <div class="auth-form-container">
              <div class="text-center mb-6">
                <v-avatar size="80" color="matf-red" class="mb-4">
                  <v-icon size="40" color="white">mdi-account-plus</v-icon>
                </v-avatar>
                <h2 class="text-h4 font-weight-bold matf-red--text mb-2">Sign Up</h2>
                <p class="text-body-1 text--secondary">Become part of the MATF community</p>
              </div>

              <v-card flat class="auth-card pa-8" elevation="0">
                <v-form @submit.prevent="onSignUpClick" ref="signupForm">
                  <v-row>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="firstName"
                        label="First Name"
                        prepend-inner-icon="mdi-account"
                        variant="outlined"
                        color="matf-red"
                        class="mb-4"
                        :rules="[rules.required, rules.nameLength]"
                        hide-details="auto"
                        hint="Your first name"
                      />
                    </v-col>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="lastName"
                        label="Last Name"
                        prepend-inner-icon="mdi-account"
                        variant="outlined"
                        color="matf-red"
                        class="mb-4"
                        :rules="[rules.required, rules.nameLength]"
                        hide-details="auto"
                        hint="Your last name"
                      />
                    </v-col>
                  </v-row>

                  <v-text-field
                    v-model="username"
                    label="Username"
                    prepend-inner-icon="mdi-at"
                    variant="outlined"
                    color="matf-red"
                    class="mb-4"
                    :rules="[rules.required, rules.minLength]"
                    hide-details="auto"
                    hint="Choose a unique username"
                  />

                  <v-text-field
                    v-model="password"
                    label="Password"
                    type="password"
                    prepend-inner-icon="mdi-lock"
                    variant="outlined"
                    color="matf-red"
                    class="mb-4"
                    :rules="[rules.required, rules.passwordLength]"
                    hide-details="auto"
                    hint="Minimum 8 characters"
                  />

                  <v-text-field
                    v-model="confirmPassword"
                    label="Confirm Password"
                    type="password"
                    prepend-inner-icon="mdi-lock-check"
                    variant="outlined"
                    color="matf-red"
                    class="mb-6"
                    :rules="[rules.required, rules.passwordMatch]"
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
                    <v-icon left class="mr-2">mdi-account-plus</v-icon>
                    Create Account
                  </v-btn>

                  <div class="text-center mb-4">
                    <v-divider class="mb-4">
                      <span class="text-caption text--secondary px-2">OR</span>
                    </v-divider>
                  </div>

                  <div class="text-center">
                    <p class="text-body-2 text--secondary mb-4">
                      Already have an account?
                    </p>
                    <v-btn
                      to="/auth/login"
                      variant="outlined"
                      color="matf-red"
                      size="large"
                      block
                      class="text-none"
                    >
                      <v-icon left class="mr-2">mdi-login</v-icon>
                      Sign In
                    </v-btn>
                  </div>
                </v-form>
              </v-card>
            </div>
          </v-col>
        </v-row>

        <!-- Success Snackbar -->
        <v-snackbar
          v-model="showSuccess"
          color="success"
          timeout="5000"
          location="top"
        >
          Account created successfully! Redirecting to login...
          <template #actions>
            <v-btn variant="text" @click="showSuccess = false">Close</v-btn>
          </template>
        </v-snackbar>

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
import { authStore } from '../../stores/auth.ts';

export default defineComponent({
  name: 'SignupView',
  data() {
    return {
      firstName: '',
      lastName: '',
      username: '',
      password: '',
      confirmPassword: '',
      loading: false,
      showError: false,
      showSuccess: false,
      errorMessage: '',
      rules: {
        required: (v: string) => !!v || 'This field is required',
        nameLength: (v: string) => (v && v.length >= 2) || 'Name must be at least 2 characters',
        minLength: (v: string) => (v && v.length >= 3) || 'Username must be at least 3 characters',
        passwordLength: (v: string) => (v && v.length >= 8) || 'Password must be at least 8 characters',
        passwordMatch: (v: string) => v === this.password || 'Passwords do not match'
      }
    };
  },
  methods: {
    async onSignUpClick() {
      // Validate form
      const { valid } = await (this.$refs.signupForm as any).validate();
      if (!valid) return;

      this.loading = true;
      this.showError = false;

      try {
        const store = authStore();
        await store.signup(this.username, this.password, this.firstName, this.lastName);
        this.showSuccess = true;
        
        // Clear form
        this.firstName = '';
        this.lastName = '';
        this.username = '';
        this.password = '';
        this.confirmPassword = '';
        
      } catch (err: any) {
        this.showError = true;
        this.errorMessage = err.response?.data?.message || 'Registration failed. Please try again.';
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
  background: linear-gradient(135deg, #8B0000 0%, #660000 100%);
}

.hero-section {
  background: linear-gradient(135deg, #8B0000 0%, #660000 100%);
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
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="academic-pattern" width="30" height="30" patternUnits="userSpaceOnUse"><path d="M15,5 L25,15 L15,25 L5,15 Z" fill="none" stroke="white" stroke-width="0.5" opacity="0.1"/></pattern></defs><rect width="100" height="100" fill="url(%23academic-pattern)"/></svg>');
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
  background: rgba(255, 255, 255, 0.1);
  border-radius: 16px;
  padding: 1.5rem;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  display: inline-block;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
  margin-top: 3rem;
}

.stat-item {
  text-align: center;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  padding: 1rem;
  backdrop-filter: blur(5px);
}

.feature-item {
  display: flex;
  align-items: center;
}

.auth-form-section {
  background: #ffffff;
  position: relative;
}

.auth-form-container {
  width: 100%;
  max-width: 420px;
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
    background: linear-gradient(135deg, #8B0000 0%, #660000 100%);
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
  
  .stats-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
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
  }
  to {
    opacity: 1;
  }
}

a {
  transition: color 0.2s ease;
}

a:hover {
  color: #660000 !important;
}
</style>
