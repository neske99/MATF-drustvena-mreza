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
    let result = (await axiosAuthenticated.get(`http://localhost:8080/Post/GetPostsForUser?userId=${userId}`)).data;

    return result;
  };

  const AddComment = async function (text: string, postId: number) {
    let result = await axiosAuthenticated.post(`http://localhost:8080/Post/AddCommentToPost?postId=${postId}`, {
      text:text,
      userId: authStore().userId
    });
    return result;
  }

  const AddLike = async function (postId: number) {
    let result = await axiosAuthenticated.post(`http://localhost:8080/Post/AddLikeToPost?postId=${postId}`, {
      userId: authStore().userId
    });
    return result;
  }

  const RemoveLike = async function (postId: number) {
    let result = await axiosAuthenticated.delete(`http://localhost:8080/Post/RemoveLikeFromPost?postId=${postId}&userId=${authStore().userId}`);
    return result;
  }

  const UploadPost = async function (newPost: string, userId: number) {
    let result = await axiosAuthenticated.post(`http://localhost:8080/Post/CreatePostForUser?userId=${userId}`, {
      text:newPost,
      userId: authStore().userId,
      picture:'picture',
      comments:[]
    });
    return result;
  }


  return { getCurrentPosts,UploadPost,AddComment, AddLike, RemoveLike, GetPosts };
})
