import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import axios from 'axios'

interface IAuthenticationToken {
  accessToken: string;
  refreshToken: string;
};
export const authStore = defineStore('auth', () => {
  // state

  const isAuthenticated = ref(true);
  const baseUrl = "api/v1/AuthenticationController/";
  let token: IAuthenticationToken;
  //getters
  const isUserAuthenticated = computed(() => isAuthenticated.value)
  //actions 
  const signup = async function (username: string, password: string) {
    //Todo
    token = await axios.post(baseUrl, { Username: username, Password: password });
  };
  const login = async function (username: string, password: string) {
    token = await axios.post(baseUrl + "Login", { Username: username, Password: password });
  };
  const logout = async function () {
    //Todo
    await axios.post(baseUrl + "Logout", token);
  };

  return { isUserAuthenticated, signup, login, logout };
},
  {
    persist: true
  });
