import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';
import { authStore } from './auth';
import type { ChatGroupDTO } from '@/dtos/chat/chatGroupDTO';
import type { ChatMessageDTO } from '@/dtos/chat/chatMessageDTO';
import type { AxiosError } from 'axios';

export const chatStore = defineStore('chat', {
  state: () => ({
    currentChatMessages: [] as ChatMessageDTO[],
    currentChatGroups: [] as ChatGroupDTO[],
    currentChatGroupId: 0
  }),

  getters: {
    getCurrentMessages: (state) => state.currentChatMessages
  },

  actions: {
    async switchUserChat(chatGroup: ChatGroupDTO) {
      console.log('=== CHAT STORE: switchUserChat called ===');
      console.log('Chat group:', chatGroup);
      console.log('Current user ID:', authStore().userId);
      
      try {
        this.currentChatGroupId = chatGroup.chatId;
        console.log('Set currentChatGroupId to:', this.currentChatGroupId);
        
        const apiUrl = `http://localhost:8095/api/v1/Chat/MessagesByChatGroup?userId=${authStore().userId}&chatGroupId=${chatGroup.chatId}`;
        console.log('Making API call to:', apiUrl);
        
        const response = await axiosAuthenticated.get(apiUrl);
        console.log('API response status:', response.status);
        console.log('API response data:', response.data);
        
        this.currentChatMessages = response.data;
        
        console.log(`Switched to chat ${this.currentChatGroupId}. Loaded ${this.currentChatMessages.length} messages.`);
        console.log('Current messages:', this.currentChatMessages);
        console.log('=== CHAT STORE: switchUserChat completed ===');
      } catch (error) {
        console.error('=== CHAT STORE: switchUserChat error ===');
        console.error('Error switching chat:', error);
        if (error && typeof error === 'object' && 'response' in error) {
          const axiosError = error as AxiosError;
          console.error('Error response:', axiosError.response);
          console.error('Error status:', axiosError.response?.status);
          console.error('Error data:', axiosError.response?.data);
        }
        this.currentChatMessages = []; // Clear messages on error
      }
    },

    async getChatGroupsForUser() {
      console.log('=== CHAT STORE: getChatGroupsForUser called ===');
      console.log('Current user ID:', authStore().userId);
      
      try {
        const apiUrl = `http://localhost:8095/api/v1/Chat/ChatGroupForUser?userId=${authStore().userId}`;
        console.log('Making API call to:', apiUrl);
        
        const response = await axiosAuthenticated.get(apiUrl);
        console.log('API response status:', response.status);
        console.log('API response data:', response.data);
        
        this.currentChatGroups = response.data;
        console.log(`Loaded ${this.currentChatGroups.length} chat groups.`);
        console.log('Chat groups:', this.currentChatGroups);
        console.log('=== CHAT STORE: getChatGroupsForUser completed ===');
        return this.currentChatGroups;
      } catch (error) {
        console.error('=== CHAT STORE: getChatGroupsForUser error ===');
        console.error('Error loading chat groups:', error);
        if (error && typeof error === 'object' && 'response' in error) {
          const axiosError = error as AxiosError;
          console.error('Error response:', axiosError.response);
          console.error('Error status:', axiosError.response?.status);
          console.error('Error data:', axiosError.response?.data);
        }
        this.currentChatGroups = [];
        return [];
      }
    }
  }
})
