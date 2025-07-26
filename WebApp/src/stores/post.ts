import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';
import {authStore} from './auth.ts'

export const postStore = defineStore('post', () => {
  // state
  const currentPosts = reactive([{ title: 'Naslov', text: 'Tekst' }, { title: 'Naslov!', text: 'Tekst!' }, { title: 'Naslov!!', text: 'Tekst!!' }]);
  //getters
  const getCurrentPosts = computed(() => currentPosts)
  //actions
  const GetPosts = async function () {
    let userId=authStore().userId;
    return (await axiosAuthenticated.get(`http://localhost:8080/Post/GetPostsForUser?userId=${userId}`)).data;
  };


  return { getCurrentPosts, GetPosts };
})
