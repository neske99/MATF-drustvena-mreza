import { ref, computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import axios from 'axios'
import router from '@/router'
import { startSignalRConnection, stopSignalRConnection } from '@/plugin/signalr';

export const authStore = defineStore('auth', () => {
  // state

  const baseUrl = "http://localhost:8094/api/v1/Authentication/";
  let username = ref("");
  let userId=ref(0);

  let accessToken = ref("");
  let refreshToken = ref("");
  let isAuthenticated = ref(false);
  //getters
  const isUserAuthenticated = computed(() => isAuthenticated);
  //actions
  const signup = async function (username: string, password: string) {
    //Todo
    try {
      let response = await axios.post(baseUrl + "RegisterBuyer", { Username: username, Password: password, FirstName: "Una", LastName: "Na", email: Math.random().toString(36).substring(2, 10) + "@gmail.com" });

      console.log(response);
      router.push("/auth/login")
    } catch (err) {
      alert(err);
    }
  };
  const login = async function (usrname: string, password: string) {

    try {
      let response = await axios.post(baseUrl + "Login", { Username: usrname, Password: password });
      username.value = usrname;
      accessToken.value = response.data.accessToken;
      refreshToken.value = response.data.refreshToken;
      userId.value= response.data.userId;
      isAuthenticated.value = true;

       await startSignalRConnection();

      console.log(isAuthenticated);
      router.push('/home')
    } catch (err) {
      if (axios.isAxiosError(err)
        && err.status == 401) {
        alert(err.message);
      } else {
        //console.log(err);
        throw err;
      }

    }
    //console.log(response);
  };
  const logout = async function () {
    //Todo
    console.log(username.value);
    console.log(refreshToken.value);
    try {
      let response = await axios.post(baseUrl + "Logout", { username: username.value, refreshToken: refreshToken.value },
        {
          headers: {
            Authorization: "Bearer " + accessToken.value
          }
        });
      isAuthenticated.value = false;
      router.push('/auth/login')

      await stopSignalRConnection();
    } catch (err) {
      //error 401
      if (axios.isAxiosError(err))
        alert(err.message);

    }

    username.value = "";
    accessToken.value = "";
    refreshToken.value = "";
    userId.value= 0;
    isAuthenticated.value = false;
  };
  const refresh = async function () {
    try {
      let response = await axios.post(baseUrl + "Refresh", { username: username.value, refreshToken: refreshToken.value });
      accessToken.value = response.data.accessToken;
      refreshToken.value = response.data.refreshToken;
      console.log(response);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        alert(err.message + "\nRefresh flow failed");
        username.value = "";
        accessToken.value = "";
        refreshToken.value = "";
        userId.value= 0;
        isAuthenticated.value = false;
      }
    }
  }

  return { username, isAuthenticated, accessToken, refreshToken,userId, refresh, signup, login, logout };
},
  {
    persist: true
  });


