import { computed, reactive, ref } from 'vue'
import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';
import { authStore } from './auth';
import type { ChatGroupDTO } from '@/dtos/chat/chatGroupDTO';

export const chatStore = defineStore('chat', () => {
  // state
  const currentChatMessages = reactive([{ message: "Dobar dan", isSender: false },
  { message: "Dobar dan", isSender: true },
  { message: "Da li znate da ste nepropisno parkirani?", isSender: false },
  { message: "Znamo znamo :)", isSender: true },
  { message: "I?!", isSender: false },
  { message: "Sta i?!!!", isSender: true }]);
  //getters
  const getCurrentMessages = computed(() => currentChatMessages)
  //actions
  const switchUserChat = async function (chatGroup: ChatGroupDTO) {
    let messages=(await axiosAuthenticated.get(`http://localhost:8095/api/v1/Chat/MessagesByChatGroup?userId=${authStore().userId}&chatGroupId=${chatGroup.chatId}`)).data;
    currentChatMessages.splice(0);
    currentChatMessages.splice(0, messages.length,
      ...messages);
      currentChatGroupId.value = chatGroup.chatId;
  }
  const refreshChat = function () {
    //TODO
  }
  let currentChatGroupId= ref(0);

  const getChatGroupsForUser=async function (){
    return (await axiosAuthenticated.get("http://localhost:8095/api/v1/Chat/ChatGroupForUser?userId=" + authStore().userId)).data;

  }

  return {currentChatGroupId, getCurrentMessages,getChatGroupsForUser, switchUserChat, refreshChat }
})
