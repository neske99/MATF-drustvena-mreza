import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const authStore = defineStore('auth', () => {
  // state
  const isAuthenticated = ref(true)
  //getters
  const isUserAuthenticated = computed(() => isAuthenticated.value)
  //actions 
  

  return { isUserAuthenticated }
})
