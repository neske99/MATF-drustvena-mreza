<template>
  <h1>
    Friend requests page
  </h1>
  <v-container>
    <userCardComponent v-for="x in friendRequests" :text="x.username" :firstName="x.firstName"
      :lastName="x.lastName" :userId="x.id" :relation="x.relation">
    </userCardComponent>


  </v-container>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import userCardComponent from '@/components/UserSearch/UserCard.vue'
import type { UserPreviewDTO } from '@/dtos/user/userPreviewDTO.ts';

// Components


export default defineComponent({
  name: 'FriendRequestsView',
  //  props: ['userSearchName'],
  components: {
    userCardComponent
  },
  data() {
    return {
      friendRequests:[] as UserPreviewDTO[]
    }
  },
  async created() {
    let self = this;
    self.friendRequests = await userStore().GetFriendRequests();


  },
  async mounted() {
    let self = this;

  }

});
</script>
