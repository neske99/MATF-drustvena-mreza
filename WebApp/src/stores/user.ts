import { computed, reactive, ref } from 'vue'
import { defineStore } from 'pinia'
import axiosAuthenticated from '@/plugin/axios';
import type { UserPreviewDTO } from '@/dtos/user/userPreviewDTO';
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO';
import type { UpdateProfileDTO, ChangePasswordDTO } from '@/dtos/user/updateUserDTO';
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
    try {
      console.log(`=== Frontend GetRelation Call ===`);
      console.log(`Calling: http://localhost:8000/api/v1/Relations/relations/${userId}/${friendId}`);
      
      const response = await axiosAuthenticated.get(`http://localhost:8000/api/v1/Relations/relations/${userId}/${friendId}`);
      
      console.log(`API Response Status: ${response.status}`);
      console.log(`API Response Data:`, response.data);
      
      // Just return the raw response - no normalization needed
      return response.data || "NONE";
    } catch (error) {
      console.error('Error in GetRelation:', error);
      return "NONE"; // Fallback to prevent crashes
    }
  }

  const SendFriendRequest = async function (userId: number, friendId: number) {
    await axiosAuthenticated.put(`http://localhost:8000/api/v1/Relations/sent/${userId}/${friendId}`);
  }

  const AcceptFriendRequest = async function (userId: number, friendId: number) {
    await axiosAuthenticated.put(`http://localhost:8000/api/v1/Relations/received/accept/${userId}/${friendId}`);
  }

  const DeclineFriendRequest = async function (userId: number, friendId: number) {
    await axiosAuthenticated.delete(`http://localhost:8000/api/v1/Relations/received/decline/${userId}/${friendId}`);
  }

  const RemoveFriend = async function (userId: number, friendId: number) {
    await axiosAuthenticated.delete(`http://localhost:8000/api/v1/Relations/friends/${userId}/${friendId}`);
  }

  const UpdateProfile = async function (updateData: UpdateProfileDTO) {
    let result: UserDetailDTO;
    result = (await axiosAuthenticated.put(`http://localhost:8094/api/v1/User/UpdateProfile`, updateData)).data;
    return result;
  }

  const ChangePassword = async function (passwordData: ChangePasswordDTO) {
    await axiosAuthenticated.put(`http://localhost:8094/api/v1/User/ChangePassword`, passwordData);
  }

  const GetUserFriends = async function (userId: number) {
    const result = (await axiosAuthenticated.get(`http://localhost:8000/api/v1/Relations/friends/${userId}`)).data;
    return result;
  }

  const UploadProfilePicture = async function (file: File) {
    const formData = new FormData();
    formData.append('file', file);

    // Use the correct backend port (8090 or 8094, as needed)
    const response = await axiosAuthenticated.post(
      "http://localhost:8094/api/v1/User/UploadProfilePicture",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }
    );
    // Return the URL from the backend response
    return response.data.url;
  }

  return { getSearchedUsers,numFriendRequests,GetFriendRequests, GetUserFriends, GetRelation,GetUsers, GetSearchedUsers, GetUser,SendFriendRequest,AcceptFriendRequest, DeclineFriendRequest, RemoveFriend, UpdateProfile, ChangePassword, UploadProfilePicture  };
})
