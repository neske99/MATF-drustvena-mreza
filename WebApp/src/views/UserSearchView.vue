<template>
  <h1>
    User Search Page
  </h1>
  <v-container>
    <userCardComponent v-for="x in currentSearchedUsers" :text="x.username" :firstName="x.firstName"
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
  name: 'UserSearchView',
  props: ['userSearchName'],
  components: {
    userCardComponent
  },
  data() {
    return {
      currentSearchedUsers:
        [] as UserPreviewDTO[],
    }
  },
  created() {
    let self = this;


    console.log("usersearchName:" + self.userSearchName);
  },
  async mounted() {
    let self = this;
    let userstore = userStore();

    self.currentSearchedUsers =
      await userstore.GetSearchedUsers(self.userSearchName);
  },
  watch: {
    '$route.params.userSearchName': async function (newValue) {
      let self = this;
      let userstore = userStore();

      self.currentSearchedUsers =
        await userstore.GetSearchedUsers(newValue);
        console.log(self.currentSearchedUsers);
    }
  }
});
</script>
