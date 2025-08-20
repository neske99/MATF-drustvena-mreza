<template>
  <v-btn v-if="relation==='NONE' && !friendshipButtonClicked" @click="sendRequest">Send friend request</v-btn>
  <v-btn v-if="relation==='RECEIVED_FRIENDSHIP_REQUEST_FROM' && !friendshipButtonClicked" @click="acceptRequest">Accept friend request</v-btn>
  <h1>User Detail Page</h1>
  <h1>Username: {{ userDetail.username }}</h1>
  <h1>Firest name: {{ userDetail.firstName }}</h1>
  <h1>Last name: {{ userDetail.lastName }}</h1>

  <PostComponent v-for="post in userPosts" :id="post.id" :text="post.text" :username="post.user.username" :comments="post.comments" :key="post.id"></PostComponent>

</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import userCardComponent from '@/components/UserSearch/UserCard.vue'
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO.ts';
import type { postDTO } from '@/dtos/post/postDTO.ts';
import { chatStore } from '@/stores/chat.ts';
import { postStore } from '@/stores/post.ts';

// Components


export default defineComponent({
  name: 'UserDetailView',
  props: ['username'],
  components: {
    userCardComponent,
    PostComponent
  },
  data() {
    return {
      userDetail: {} as UserDetailDTO,
      relation:'',
      friendshipButtonClicked:false,
      userPosts:[] as postDTO[]
    };
  },
  async created(){
    let self = this;
    let userstore = userStore();

    self.userDetail = await userstore.GetUser(self.username);
    self.relation= await userstore.GetRelation(authStore().userId, self.userDetail.id);
    self.userPosts = await postStore().GetPostsForUser(self.userDetail.id);


  },
  methods: {
    async sendRequest() {
      let userId=authStore().userId;
      let userstore = userStore();
      await userstore.SendFriendRequest(userId, this.userDetail.id);
      this.friendshipButtonClicked=true;
    },
    async acceptRequest() {
      let userId=authStore().userId;
      let userstore = userStore();
      await userstore.AcceptFriendRequest(userId, this.userDetail.id);
      this.friendshipButtonClicked=true;
    }
  },
});
</script>
