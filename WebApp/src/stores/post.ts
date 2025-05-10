import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'

export const postStore = defineStore('post', () => {
  // state
  const currentPosts = reactive([{ title: 'Naslov', text: 'Tekst' }, { title: 'Naslov!', text: 'Tekst!' }, { title: 'Naslov!!', text: 'Tekst!!' }]);
  //getters
  const getCurrentPosts = computed(() => currentPosts)
  //actions


  return { getCurrentPosts };
})
