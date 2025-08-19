import { computed, reactive, ref } from 'vue'
import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';
import type { UserPreviewDTO } from '@/dtos/user/userPreviewDTO';
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO';
import { authStore } from './auth';

export const userStore = defineStore('user', () => {
  // state
  const currentUsers = reactive([{ title: 'Naslov', text: 'Tekst' }, { title: 'Naslov!', text: 'Tekst!' }, { title: 'Naslov!!', text: 'Tekst!!' }]);
  const numFriendRequests = ref(0);

  //getters
  const getSearchedUsers = computed(() => currentUsers);
  //actions
  const GetUsers = async function () {
    let result: [UserPreviewDTO];

    result = (await axiosAuthenticated.get("http://localhost:8094/api/v1/User/GetAllUsers")).data;
    return result;
  };

  const GetSearchedUsers = async function (username: string) {
    let result: [UserPreviewDTO];

    result = (await axiosAuthenticated.get(`http://localhost:8094/api/v1/User/GetSearchedUsers?userId=${authStore().userId}&username=${username}`)).data;
    return result;
  };

  const GetFriendRequests = async function (){

    let result = (await axiosAuthenticated.get(`http://localhost:8094/api/v1/User/GetFriendRequests?userId=${authStore().userId}`)).data;
    numFriendRequests.value = result.length;
    return result;
  };

  const GetUser = async function (username: string) {
    let result: UserDetailDTO;
    result = (await axiosAuthenticated.get(`http://localhost:8094/api/v1/User/GetUser/${username}`)).data;
    return result;
  };

  const GetRelation = async function (userId: number, friendId: number) {
    return (await axiosAuthenticated.get(`http://localhost:8000/api/v1/Relations/relations/${userId}/${friendId}`)).data;
  }

  const SendFriendRequest = async function (userId: number, friendId: number) {
    await axiosAuthenticated.put(`http://localhost:8000/api/v1/Relations/sent/${userId}/${friendId}`);
  }

  const AcceptFriendRequest = async function (userId: number, friendId: number) {
    await axiosAuthenticated.put(`http://localhost:8000/api/v1/Relations/received/accept/${userId}/${friendId}`);
  }



  return { getSearchedUsers,numFriendRequests,GetFriendRequests, GetRelation,GetUsers, GetSearchedUsers, GetUser,SendFriendRequest,AcceptFriendRequest  };
})
