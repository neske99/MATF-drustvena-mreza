import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const authStore = defineStore('auth', () => {
  // state
  const isAuthenticated = ref(true)
  //getters
  const isUserAuthenticated = computed(() => isAuthenticated.value)
  //actions 
  const signup = function () {
    //Todo
  }
  const login = function () {
    //Todo
  }
  const logout = function () {
    //Todo
  }

  return { isUserAuthenticated, signup, login, logout }
})
