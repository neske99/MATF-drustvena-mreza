<template>
  <h1>User Detail Page</h1>
  {{ userDetail }}
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import userCardComponent from '@/components/UserSearch/UserCard.vue'

// Components


export default defineComponent({
  name: 'UserDetailView',
  props: ['username'],
  components: {
    userCardComponent
  },
  data() {
    return {
      userDetail: {}
    };
  },
  async mounted() {
    let self = this;
    let userstore = userStore();

    self.userDetail = await userstore.GetUser(self.username);
  }
});
</script>
