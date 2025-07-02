<template>
  <h1>User Detail Page</h1>
  <h1>Username: {{ userDetail.username }}</h1>
  <h1>Firest name: {{ userDetail.firstName }}</h1>
  <h1>Last name: {{ userDetail.lastName }}</h1>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import userCardComponent from '@/components/UserSearch/UserCard.vue'
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO.ts';

// Components


export default defineComponent({
  name: 'UserDetailView',
  props: ['username'],
  components: {
    userCardComponent
  },
  data() {
    return {
      userDetail: {} as UserDetailDTO
    };
  },
  async mounted() {
    let self = this;
    let userstore = userStore();

    self.userDetail = await userstore.GetUser(self.username);
  }
});
</script>
