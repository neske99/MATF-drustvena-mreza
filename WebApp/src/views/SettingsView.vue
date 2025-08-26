<template>
  <div class="settings-page">
    <!-- Settings Header -->
    <v-card 
      class="settings-header-card mb-6" 
      elevation="2"
      rounded="lg"
    >
      <v-card-text class="pa-6">
        <div class="d-flex align-center">
          <v-avatar size="60" color="matf-red" class="mr-4">
            <v-icon size="32" color="white">mdi-cog</v-icon>
          </v-avatar>
          <div class="flex-grow-1">
            <h1 class="text-h4 font-weight-bold matf-red--text mb-2">
              Account Settings
            </h1>
            <p class="text-body-1 text--secondary mb-0">
              Manage your account information and preferences
            </p>
          </div>
          
          <!-- Settings Icon -->
          <div class="d-none d-md-flex align-center">
            <v-icon size="48" color="matf-red">mdi-account-cog</v-icon>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Settings Content -->
    <v-row>
      <!-- Profile Settings -->
      <v-col cols="12" md="6">
        <v-card class="settings-card mb-6" elevation="2" rounded="lg">
          <v-card-title class="pa-4 pb-2">
            <v-icon class="mr-3" color="matf-red">mdi-account-edit</v-icon>
            <h3 class="text-h6 font-weight-bold">Profile Information</h3>
          </v-card-title>
          
          <v-card-text class="pa-4">
            <v-form ref="profileForm" @submit.prevent="updateProfile">
              <v-text-field
                v-model="profileData.username"
                label="Username"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                color="matf-red"
                class="mb-4"
                :rules="[rules.required, rules.minLength]"
                :loading="profileLoading"
                hint="This will change your login username"
                persistent-hint
              />

              <v-text-field
                v-model="profileData.firstName"
                label="First Name"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                color="matf-red"
                class="mb-4"
                :rules="[rules.required]"
                :loading="profileLoading"
              />

              <v-text-field
                v-model="profileData.lastName"
                label="Last Name"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                color="matf-red"
                class="mb-6"
                :rules="[rules.required]"
                :loading="profileLoading"
              />

              <div class="d-flex justify-end gap-2">
                <v-btn
                  variant="outlined"
                  color="grey"
                  class="text-none"
                  @click="resetProfileForm"
                  :disabled="profileLoading"
                >
                  <v-icon class="mr-2">mdi-refresh</v-icon>
                  Reset
                </v-btn>
                
                <v-btn
                  type="submit"
                  color="matf-red"
                  class="text-none"
                  :loading="profileLoading"
                  :disabled="!hasProfileChanges"
                >
                  <v-icon class="mr-2">mdi-content-save</v-icon>
                  Save Changes
                </v-btn>
              </div>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Password Settings -->
      <v-col cols="12" md="6">
        <v-card class="settings-card mb-6" elevation="2" rounded="lg">
          <v-card-title class="pa-4 pb-2">
            <v-icon class="mr-3" color="matf-red">mdi-lock</v-icon>
            <h3 class="text-h6 font-weight-bold">Change Password</h3>
          </v-card-title>
          
          <v-card-text class="pa-4">
            <v-form ref="passwordForm" @submit.prevent="changePassword">
              <v-text-field
                v-model="passwordData.currentPassword"
                label="Current Password"
                type="password"
                prepend-inner-icon="mdi-lock"
                variant="outlined"
                color="matf-red"
                class="mb-4"
                :rules="[rules.required]"
                :loading="passwordLoading"
              />

              <v-text-field
                v-model="passwordData.newPassword"
                label="New Password"
                type="password"
                prepend-inner-icon="mdi-lock-plus"
                variant="outlined"
                color="matf-red"
                class="mb-4"
                :rules="[rules.required, rules.passwordLength]"
                :loading="passwordLoading"
                hint="Minimum 8 characters required"
                persistent-hint
              />

              <v-text-field
                v-model="confirmNewPassword"
                label="Confirm New Password"
                type="password"
                prepend-inner-icon="mdi-lock-check"
                variant="outlined"
                color="matf-red"
                class="mb-6"
                :rules="[rules.required, rules.passwordMatch]"
                :loading="passwordLoading"
              />

              <div class="d-flex justify-end gap-2">
                <v-btn
                  variant="outlined"
                  color="grey"
                  class="text-none"
                  @click="resetPasswordForm"
                  :disabled="passwordLoading"
                >
                  <v-icon class="mr-2">mdi-refresh</v-icon>
                  Clear
                </v-btn>
                
                <v-btn
                  type="submit"
                  color="matf-red"
                  class="text-none"
                  :loading="passwordLoading"
                  :disabled="!hasPasswordData"
                >
                  <v-icon class="mr-2">mdi-lock-reset</v-icon>
                  Change Password
                </v-btn>
              </div>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Success Snackbar -->
    <v-snackbar
      v-model="showSuccess"
      color="success"
      timeout="5000"
      location="top"
    >
      {{ successMessage }}
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
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth';
import { userStore } from '../stores/user';
import type { UpdateProfileDTO, ChangePasswordDTO } from '@/dtos/user/updateUserDTO';

export default defineComponent({
  name: 'SettingsView',
  data() {
    return {
      profileData: {
        username: '',
        firstName: '',
        lastName: ''
      },
      passwordData: {
        currentPassword: '',
        newPassword: ''
      },
      confirmNewPassword: '',
      originalProfileData: {
        username: '',
        firstName: '',
        lastName: ''
      },
      currentUser: {
        username: '',
        firstName: '',
        lastName: ''
      },
      profileLoading: false,
      passwordLoading: false,
      showSuccess: false,
      showError: false,
      successMessage: '',
      errorMessage: '',
      rules: {
        required: (v: string) => !!v || 'This field is required',
        minLength: (v: string) => (v && v.length >= 3) || 'Username must be at least 3 characters',
        passwordLength: (v: string) => (v && v.length >= 8) || 'Password must be at least 8 characters',
        passwordMatch: (v: string) => v === this.passwordData.newPassword || 'Passwords do not match'
      }
    };
  },
  async created() {
    await this.loadCurrentUser();
  },
  computed: {
    hasProfileChanges() {
      return this.profileData.username !== this.originalProfileData.username ||
             this.profileData.firstName !== this.originalProfileData.firstName ||
             this.profileData.lastName !== this.originalProfileData.lastName;
    },
    
    hasPasswordData() {
      return this.passwordData.currentPassword.trim() !== '' &&
             this.passwordData.newPassword.trim() !== '' &&
             this.confirmNewPassword.trim() !== '';
    }
  },
  methods: {
    async loadCurrentUser() {
      try {
        const auth = authStore();
        const userstore = userStore();
        
        // Get current user data
        this.currentUser = await userstore.GetUser(auth.username);
        
        // Set form data
        this.profileData = {
          username: this.currentUser.username,
          firstName: this.currentUser.firstName,
          lastName: this.currentUser.lastName
        };
        
        // Store original data for comparison
        this.originalProfileData = { ...this.profileData };
        
        console.log('Current user loaded:', this.currentUser);
      } catch (error) {
        console.error('Error loading current user:', error);
        this.showErrorMessage('Failed to load user information');
      }
    },
    
    async updateProfile() {
      try {
        // Validate form
        const { valid } = await (this.$refs.profileForm as any).validate();
        if (!valid) return;
        
        this.profileLoading = true;
        const userstore = userStore();
        const auth = authStore();
        
        // Prepare update data (only send changed fields)
        const updateData: UpdateProfileDTO = {};
        
        if (this.profileData.username !== this.originalProfileData.username) {
          updateData.username = this.profileData.username;
        }
        if (this.profileData.firstName !== this.originalProfileData.firstName) {
          updateData.firstName = this.profileData.firstName;
        }
        if (this.profileData.lastName !== this.originalProfileData.lastName) {
          updateData.lastName = this.profileData.lastName;
        }
        
        // Update profile
        const updatedUser = await userstore.UpdateProfile(updateData);
        
        // Update auth store if username changed
        if (updateData.username) {
          auth.username = updateData.username;
        }
        
        // Update local data
        this.currentUser = updatedUser;
        this.originalProfileData = { ...this.profileData };
        
        this.showSuccessMessage('Profile updated successfully!');
        
      } catch (error: any) {
        console.error('Error updating profile:', error);
        this.showErrorMessage(error.response?.data?.message || 'Failed to update profile');
      } finally {
        this.profileLoading = false;
      }
    },
    
    async changePassword() {
      try {
        // Validate form
        const { valid } = await (this.$refs.passwordForm as any).validate();
        if (!valid) return;
        
        this.passwordLoading = true;
        const userstore = userStore();
        
        const passwordData: ChangePasswordDTO = {
          currentPassword: this.passwordData.currentPassword,
          newPassword: this.passwordData.newPassword
        };
        
        await userstore.ChangePassword(passwordData);
        
        // Clear password form
        this.resetPasswordForm();
        
        this.showSuccessMessage('Password changed successfully!');
        
      } catch (error: any) {
        console.error('Error changing password:', error);
        const errorMsg = error.response?.data?.errors 
          ? Object.values(error.response.data.errors).flat().join(', ')
          : error.response?.data?.message || 'Failed to change password';
        this.showErrorMessage(errorMsg);
      } finally {
        this.passwordLoading = false;
      }
    },
    
    resetProfileForm() {
      this.profileData = { ...this.originalProfileData };
    },
    
    resetPasswordForm() {
      this.passwordData = {
        currentPassword: '',
        newPassword: ''
      };
      this.confirmNewPassword = '';
    },
    
    showSuccessMessage(message: string) {
      this.successMessage = message;
      this.showSuccess = true;
    },
    
    showErrorMessage(message: string) {
      this.errorMessage = message;
      this.showError = true;
    }
  }
});
</script>

<style scoped>
.settings-page {
  animation: fadeInUp 0.6s ease-out;
}

.settings-header-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.settings-header-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.1);
}

.settings-card {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border: 1px solid rgba(139, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.settings-card:hover {
  box-shadow: 0 4px 20px rgba(139, 0, 0, 0.08);
}

.info-item {
  padding: 12px;
  background: rgba(139, 0, 0, 0.03);
  border-radius: 8px;
  border-left: 3px solid #8B0000;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
  border-radius: 8px;
}

.gap-2 > * + * {
  margin-left: 8px;
}

/* Form styling */
.v-text-field .v-field {
  border-radius: 8px;
}

.v-text-field .v-field:hover {
  box-shadow: 0 2px 8px rgba(139, 0, 0, 0.1);
}

/* Animations */
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

/* Mobile Responsiveness */
@media (max-width: 768px) {
  .settings-header-card .v-card-text {
    padding: 1.5rem;
  }
  
  .gap-2 {
    flex-direction: column;
    gap: 8px;
  }
  
  .gap-2 > * + * {
    margin-left: 0;
  }
}

@media (max-width: 600px) {
  .settings-header-card .d-flex {
    flex-direction: column;
    text-align: center;
  }
  
  .settings-header-card .mr-4 {
    margin-right: 0 !important;
    margin-bottom: 1rem;
  }
}
</style>