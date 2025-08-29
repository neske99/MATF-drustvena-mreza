<template>
    <v-dialog v-model="showDialog" max-width="400px">
        <v-card>
            <v-card-title class="text-h6 matf-red--text">Change Profile Picture</v-card-title>
            <v-card-text>
                <v-file-input accept="image/*"
                              label="Select Image"
                              prepend-icon="mdi-camera"
                              color="matf-red"
                              v-model="selectedFile"
                              @change="onFileSelected"
                              @click:clear="clearFileSelection"
                              class="mt-2"
                              clearable />
                <div v-if="selectedFile" class="filename-preview mb-2" style="text-align:center;font-size:13px;color:#8B0000;">
                    {{ selectedFile.name }}
                </div>
                <div v-if="previewUrl" class="image-preview mb-4">
                    <img :src="previewUrl" alt="Preview" class="profile-preview-img" />
                </div>
            </v-card-text>
            <v-card-actions>
                <v-spacer />
                <v-btn color="grey" variant="text" @click="closeDialog">Cancel</v-btn>
                <v-btn color="matf-red"
                       :loading="uploading"
                       @click="uploadProfilePicture"
                       :disabled="!selectedFile">
                    Upload
                </v-btn>
            </v-card-actions>
        </v-card>
        <v-snackbar v-model="showSuccess" color="success" timeout="3000">
            Profile picture updated!
        </v-snackbar>
        <v-snackbar v-model="showError" color="error" timeout="3000">
            {{ errorMessage }}
        </v-snackbar>
    </v-dialog>
</template>

<script lang="ts">
    import { defineComponent, ref, watch } from 'vue';
    import axiosAuthenticated from '@/plugin/axios';
    import { userStore } from '@/stores/user';

    export default defineComponent({
        name: 'ProfilePictureUpload',
        props: {
            show: {
                type: Boolean,
                required: true
            }
        },
        emits: ['close', 'profile-picture-updated'],
        setup(props, { emit }) {
            const selectedFile = ref<File | null>(null);
            const previewUrl = ref<string | null>(null);
            const uploading = ref(false);
            const showSuccess = ref(false);
            const showError = ref(false);
            const errorMessage = ref('');
            const showDialog = ref(props.show);

            watch(() => props.show, (val) => {
                showDialog.value = val;
            });

            watch(showDialog, (val) => {
                if (!val) emit('close');
            });

            const closeDialog = () => {
                showDialog.value = false;
                clearFileSelection();
            };

            const onFileSelected = (event: Event) => {
                const input = event.target as HTMLInputElement;
                if (!input.files || input.files.length === 0) {
                    clearFileSelection();
                    return;
                }

                // Clean up previous object URL if exists
                if (previewUrl.value) {
                    URL.revokeObjectURL(previewUrl.value);
                }

                selectedFile.value = input.files[0];
                previewUrl.value = URL.createObjectURL(selectedFile.value);
            };

            const clearFileSelection = () => {
                // Clean up the object URL
                if (previewUrl.value) {
                    URL.revokeObjectURL(previewUrl.value);
                    previewUrl.value = null;
                }
                selectedFile.value = null;
            };

           const uploadProfilePicture = async () => {
              if (!selectedFile.value) return;
              uploading.value = true;
              showError.value = false;
              errorMessage.value = '';
              try {
                const url = await userStore().UploadProfilePicture(selectedFile.value);
                emit('profile-picture-updated', url);
                showSuccess.value = true;
                closeDialog();
              } catch (err: any) {
                showError.value = true;
                errorMessage.value = err.response?.data?.message || 'Upload failed.';
              } finally {
                uploading.value = false;
              }
            };



            return {
                selectedFile,
                previewUrl,
                uploading,
                showSuccess,
                showError,
                errorMessage,
                showDialog,
                closeDialog,
                onFileSelected,
                clearFileSelection,
                uploadProfilePicture
            };
        }
    });
</script>

<style scoped>
    .image-preview {
        text-align: center;
    }

    .profile-preview-img {
        max-width: 100%;
        max-height: 180px;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.08);
        display: block;
        margin: 0 auto;
    }
</style>