import { ref, computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import axios from 'axios'
import router from '@/router'

interface IAuthenticationInformation {
  username: string;
  accessToken: string;
  refreshToken: string;
  isAuthenticated: boolean;
};
export const authStore = defineStore('auth', () => {
  // state

  const baseUrl = "http://localhost:8094/api/v1/Authentication/";
  let authenticationInformation: IAuthenticationInformation = reactive({
    username: "", accessToken: "", refreshToken: "", isAuthenticated: false
  });
  //getters
  const isUserAuthenticated = computed(() => authenticationInformation.isAuthenticated);
  //actions 
  const signup = async function (username: string, password: string) {
    //Todo
    try {
      let response = await axios.post(baseUrl + "RegisterBuyer", { Username: username, Password: password, FirstName: "Una", LastName: "Na", email: Math.random().toString(36).substring(2, 10) + "@gmail.com" });

      console.log(response);
      router.push("/auth/login")
    } catch (err) {
      //console.log(err.response);
      console.log(err);
    }
  };
  const login = async function (username: string, password: string) {

    try {
      let response = await axios.post(baseUrl + "Login", { Username: username, Password: password });
      authenticationInformation.username = username;
      authenticationInformation.accessToken = response.data.accessToken;
      authenticationInformation.refreshToken = response.data.refreshToken;
      authenticationInformation.isAuthenticated = true;

      console.log(authenticationInformation);
      router.push('/home')
    } catch (err) {
      //error 401
      console.log(err);
    }
    //console.log(response);
  };
  const logout = async function () {
    //Todo
    try {
      let response = await axios.post(baseUrl + "Logout", { username: authenticationInformation.username, refreshToken: authenticationInformation.refreshToken },
        {
          headers: {
            Authorization: "Bearer " + authenticationInformation.accessToken
          }
        });
      authenticationInformation.isAuthenticated = false;
      console.log(response);
      router.push('/auth/login')
    } catch (err) {
      //error 401
      console.log(err);
    }
  };
  const refresh = async function () {
    try {
      let response = await axios.post(baseUrl + "Refresh", { username: authenticationInformation.username, refreshToken: authenticationInformation.refreshToken });
      console.log(response);
    } catch (err) {
      //error 401
      console.log(err);
    }

  }

  return { isUserAuthenticated, signup, login, logout };
},
  {
    persist: true
  });
