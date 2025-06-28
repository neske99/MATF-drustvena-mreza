import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';

export const userStore = defineStore('user', () => {
  // state
  const currentUsers = reactive([{ title: 'Naslov', text: 'Tekst' }, { title: 'Naslov!', text: 'Tekst!' }, { title: 'Naslov!!', text: 'Tekst!!' }]);
  //getters
  const getSearchedUsers = computed(() => currentUsers);
  //actions
  const GetUsers = async function () {
    return (await axiosAuthenticated.get("http://localhost:8094/api/v1/User/GetAllUsers")).data;
  };

  const GetUser = async function (username: string) {
    return (await axiosAuthenticated.get(`http://localhost:8094/api/v1/User/GetUser/${username}`)).data;
  };

  return { getSearchedUsers, GetUsers,GetUser };
})
