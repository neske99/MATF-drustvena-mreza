import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'

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
  const switchUserChat = function () {
    currentChatMessages.splice(0, currentChatMessages.length,
      ...[{ message: "Dobar dan", isSender: true },
      { message: "Dobro jutro", isSender: false },
      { message: "Da li moze?", isSender: true }, { message: "Moze", isSender: false },
      { message: "##$%!!@#$%^^&*!", isSender: true }])
  }
  const refreshChat = function () {
    //TODO
  }

  return { getCurrentMessages, switchUserChat, refreshChat }
})
