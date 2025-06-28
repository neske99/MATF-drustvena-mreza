<template>
  <h1>
    User Search Page
  </h1>
  <v-container>
    <userCardComponent v-for="x in currentSearchedUsers" :text="x"></userCardComponent>


  </v-container>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'
import userCardComponent from '@/components/UserSearch/UserCard.vue'

// Components


export default defineComponent({
  name: 'UserSearchView',
  components: {
    userCardComponent
  },
  data() {
    return { currentSearchedUsers: [] }
  },
  async mounted() {
    let self = this;
    let userstore = userStore();
    self.currentSearchedUsers = await userstore.GetUsers()
  }
});
</script>
