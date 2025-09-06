<template>
  <v-navigation-drawer
    permanent
    location="right"
    class="right-sidebar"
    width="320"
  >
    <div class="chat-container">
      <!-- Chat Header -->
      <div class="chat-header pa-4">
        <div v-if="currentChatGroupId === 0" class="no-chat-selected">
          <div class="text-center">
            <v-icon size="48" color="grey-lighten-1" class="mb-3">mdi-chat-outline</v-icon>
            <h3 class="text-h6 font-weight-bold text--secondary mb-2">Select a conversation</h3>
            <p class="text-body-2 text--secondary">Choose from your existing conversations or start a new one</p>
          </div>
        </div>

        <div v-else class="active-chat-header">
          <div class="d-flex align-center">
            <v-avatar size="40" color="matf-red" class="mr-3">
              <img
                v-if="getCurrentChatUserProfilePicture()"
                :src="getCurrentChatUserProfilePicture()"
                alt="Profile Picture"
                @error="() => {}"
                class="chat-avatar-image"
              />
              <v-icon v-else color="white">mdi-account</v-icon>
            </v-avatar>
            <div class="flex-grow-1">
              <h3 class="text-h6 font-weight-bold text--primary mb-0">
                {{ currentChatUser }}
              </h3>
              <p class="text-caption text--secondary mb-0">
                {{ getCurrentChatUserDisplayName }}
              </p>
            </div>
            <v-btn
              icon
              variant="text"
              size="small"
              color="grey"
            >
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </div>
        </div>
      </div>

      <v-divider />

      <!-- Messages Container -->
      <div class="messages-container" ref="messagesContainer">
        <div v-if="currentChatGroupId === 0" class="no-messages-placeholder">
          <div class="text-center pa-6">
            <v-icon size="64" color="grey-lighten-2" class="mb-4">mdi-message-outline</v-icon>
            <p class="text-body-2 text--secondary">No messages to display</p>
          </div>
        </div>

        <div v-else-if="loading" class="loading-messages">
          <div class="text-center pa-6">
            <v-progress-circular
              indeterminate
              color="matf-red"
              size="32"
              class="mb-3"
            />
            <p class="text-body-2 text--secondary">Loading messages...</p>
          </div>
        </div>

        <div v-else-if="messages.length === 0" class="empty-chat">
          <div class="text-center pa-6">
            <v-icon size="48" color="matf-red" class="mb-3">mdi-chat-plus</v-icon>
            <h4 class="text-subtitle-1 font-weight-medium text--secondary mb-2">
              Start the conversation
            </h4>
            <p class="text-caption text--secondary">
              Send your first message to {{ currentChatUser }}
            </p>
          </div>
        </div>

        <div v-else class="messages-list">
          <MessageComponent
            v-for="message in messages"
            :message="message.message"
            :timestamp="new Date(message.timestamp)"
            :key="message.id"
            :isSender="message.isSender"
            class="message-item"
          />
        </div>
      </div>

      <!-- Message Input -->
      <div class="message-input-container" v-if="currentChatGroupId !== 0">
        <v-divider />

        <div class="message-input pa-4">
          <div class="d-flex align-end">
            <v-text-field
              v-model="messageToSend"
              placeholder="Type a message..."
              variant="outlined"
              color="matf-red"
              density="compact"
              hide-details
              class="message-field flex-grow-1"
              @keydown.enter.prevent="onSend"
              :disabled="sendingMessage"
            />

            <v-btn
              color="matf-red"
              class="ml-2 send-button"
              :disabled="!messageToSend.trim() || sendingMessage"
              @click="onSend"
              size="large"
              icon
              :loading="sendingMessage"
            >
              <v-icon>mdi-send</v-icon>
            </v-btn>
          </div>

          <!-- Quick Actions -->
          <div class="quick-actions mt-2" v-if="!sendingMessage">
            <v-btn
              variant="text"
              size="small"
              color="grey"
              class="text-none mr-2"
              disabled
            >
              <v-icon class="mr-1" size="16">mdi-attachment</v-icon>
              File
            </v-btn>
            <v-btn
              variant="text"
              size="small"
              color="grey"
              class="text-none"
              disabled
            >
              <v-icon class="mr-1" size="16">mdi-gif</v-icon>
              GIF
            </v-btn>
          </div>
        </div>
      </div>
    </div>
  </v-navigation-drawer>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import MessageComponent from './Chat/MessageComponent.vue';
import { chatStore } from '../stores/chat'
import { userCacheService } from '../services/userCacheService'
import { startSignalRConnection, getSignalRConnection, stopSignalRConnection } from '../plugin/signalr'
import type { HubConnection } from '@microsoft/signalr';
import { authStore } from '../stores/auth';

export default defineComponent({
  name: 'TheRightSidebar',
  components: {
    MessageComponent
  },
  data() {
    return {
      messageToSend: "",
      connection: null as HubConnection | null,
      loading: false,
      sendingMessage: false,
      currentChatUserInfo: null as any
    }
  },
  computed: {
    messages() {
      const messages = chatStore().getCurrentMessages;
      console.log('RIGHT SIDEBAR: messages computed property called, returning:', messages);
      return messages;
    },

    isSendMessageDisabled() {
      return chatStore().currentChatGroupId === 0;
    },

    currentChatGroupId() {
      const chatId = chatStore().currentChatGroupId;
      console.log('RIGHT SIDEBAR: currentChatGroupId computed property called, returning:', chatId);
      return chatId;
    },

    currentChatUser() {
      const store = chatStore();
      const currentGroup = store.currentChatGroups.find(
        group => group.chatId === store.currentChatGroupId
      );
      const username = currentGroup?.username || 'Unknown User';
      console.log('RIGHT SIDEBAR: currentChatUser computed property called, returning:', username);
      return username;
    },

    getCurrentChatUserDisplayName() {
      if (this.currentChatUserInfo) {
        return `${this.currentChatUserInfo.firstName} ${this.currentChatUserInfo.lastName}`;
      }
      return 'MATF Student';
    }
  },
  async created() {
    this.connection = await getSignalRConnection();
    if (this.connection) {
      this.connection.on("ReceiveMessageReal", (userId: number, message: string, chatGroupId: number, messageId: number, timestamp?: string) => {
        console.log('Received message via SignalR:', { userId, message, chatGroupId, messageId });

        const store = chatStore();
        if (store.currentChatGroupId === chatGroupId) {
          const newMessage = {
            message: message,
            isSender: userId === authStore().userId,
            id: messageId,
            timestamp: timestamp ? new Date(timestamp) : new Date()
          };

          // Add message to the store's message array
          store.currentChatMessages.push(newMessage);

          console.log('Added message to current chat, total messages:', store.currentChatMessages.length);

          this.scrollToBottom();
        }

        const chGroup = store.currentChatGroups.find(x => x.chatId === chatGroupId);
        if (chGroup) {
          chGroup.hasNewMessages = userId !== authStore().userId;
          if (chGroup.hasNewMessages) {
            const index = store.currentChatGroups.indexOf(chGroup);
            store.currentChatGroups.splice(index, 1);
            store.currentChatGroups.unshift(chGroup);
          }
        }
      });
    }
  },
  beforeUnmount() {
    // Clean up SignalR connection
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  },
  methods: {
    getCurrentChatUserProfilePicture() {
      if (!this.currentChatUserInfo?.profilePictureUrl) return null;

      const url = this.currentChatUserInfo.profilePictureUrl;
      if (url.startsWith('/uploads/profile-pictures/')) {
        return import.meta.env.DEV ? `http://localhost:8094${url}` : url;
      }

      if (url.startsWith('/uploads/')) {
        return import.meta.env.DEV ? `http://localhost:8094${url}` : url;
      }

      return url;
    },
    async onSend() {
      if (!this.messageToSend.trim() || this.sendingMessage) return;

      try {
        this.sendingMessage = true;
        const messageText = this.messageToSend.trim();

        // Try to get connection, but send message regardless
        if (!this.connection) {
          this.connection = await getSignalRConnection();
        }

        if (this.connection) {
          const store = chatStore();
          await this.connection.invoke("SendMessageToGroup", store.currentChatGroupId, messageText);
        }

        // Clear message regardless of success/failure
        this.messageToSend = "";
      } catch (error) {
        console.error('Error sending message:', error);
        // Still clear the message even if sending failed
        this.messageToSend = "";
      } finally {
        this.sendingMessage = false;
      }
    },

    scrollToBottom() {
      this.$nextTick(() => {
        const container = this.$refs.messagesContainer as HTMLElement;
        if (container) {
          container.scrollTop = container.scrollHeight;
        }
      });
    },

    async loadMessages() {
      if (this.currentChatGroupId === 0) return;

      this.loading = true;
      try {
        // Messages are loaded via chatStore.switchUserChat
      } catch (error) {
        // Ignore loading errors
      } finally {
        this.loading = false;
      }
    }
  },
  watch: {
    currentChatGroupId: {
      immediate: true,
      async handler(newChatId, oldChatId) {
        console.log('=== RIGHT SIDEBAR: CurrentChatGroupId watcher ===');
        console.log('CurrentChatGroupId changed:', newChatId, 'from:', oldChatId);
        console.log('Messages length after chat switch:', this.messages.length);
        console.log('Current messages:', this.messages);

        if (newChatId !== oldChatId) {
          console.log('Chat ID actually changed, processing...');

          if (newChatId !== 0) {
            console.log('New chat ID is not 0, loading user info...');

            // Load user info for current chat user
            const store = chatStore();
            const currentGroup = store.currentChatGroups.find(
              group => group.chatId === newChatId
            );

            console.log('Current group from store:', currentGroup);
            console.log('All groups in store:', store.currentChatGroups);

            if (currentGroup?.username) {
              try {
                console.log('Fetching user info for username:', currentGroup.username);
                this.currentChatUserInfo = await userCacheService.getUserByUsername(currentGroup.username);
                console.log('Loaded user info:', this.currentChatUserInfo);
              } catch (error) {
                console.error(`Could not fetch user info for ${currentGroup.username}:`, error);
              }
            } else {
              console.log('No current group found or no username');
            }

            await this.loadMessages();
            this.$nextTick(() => {
              this.scrollToBottom();
            });
          } else {
            console.log('New chat ID is 0, clearing user info');
            this.currentChatUserInfo = null;
          }
        } else {
          console.log('Chat ID did not change, skipping...');
        }

        console.log('=== RIGHT SIDEBAR: CurrentChatGroupId watcher completed ===');
      }
    }
  }
})
</script>

<style scoped>
.right-sidebar {
  background: linear-gradient(135deg, #FFFFFF 0%, #FAFAFA 100%);
  border-left: 1px solid rgba(139, 0, 0, 0.1);
}

.chat-container {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.chat-header {
  background: rgba(139, 0, 0, 0.03);
  border-bottom: 1px solid rgba(139, 0, 0, 0.1);
  min-height: 80px;
  display: flex;
  align-items: center;
}

.no-chat-selected,
.active-chat-header {
  width: 100%;
}

.messages-container {
  flex: 1;
  overflow-y: auto;
  background: #FAFAFA;
}

.messages-list {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 100%;
}

.message-item {
  margin-bottom: 12px;
  animation: messageSlideIn 0.3s ease-out;
}

.no-messages-placeholder,
.empty-chat,
.loading-messages {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
}

.message-input-container {
  background: #FFFFFF;
  border-top: 1px solid rgba(139, 0, 0, 0.1);
}

.message-field :deep(.v-field) {
  border-radius: 8px;
  background: #F5F5F5;
}

.message-field :deep(.v-field--focused) {
  background: #FFFFFF;
}

.send-button {
  border-radius: 8px;
  min-width: 48px;
  height: 48px;
}

.quick-actions {
  display: flex;
  gap: 8px;
}

.v-btn {
  text-transform: none;
  font-weight: 600;
  border-radius: 8px;
}

/* Custom scrollbar */
.messages-container::-webkit-scrollbar {
  width: 4px;
}

.messages-container::-webkit-scrollbar-track {
  background: transparent;
}

.messages-container::-webkit-scrollbar-thumb {
  background: rgba(139, 0, 0, 0.3);
  border-radius: 2px;
}

.messages-container::-webkit-scrollbar-thumb:hover {
  background: rgba(139, 0, 0, 0.5);
}

/* Animations */
@keyframes messageSlideIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.chat-container {
  animation: slideInRight 0.3s ease-out;
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

/* Loading state */
.loading-messages .v-progress-circular {
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% { opacity: 1; }
  50% { opacity: 0.5; }
  100% { opacity: 1; }
}

/* Mobile responsiveness */
@media (max-width: 1200px) {
  .right-sidebar {
    display: none !important;
  }
}

/* Profile Picture Styles - Fix image fitting */
.chat-avatar-image {
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
</style>
