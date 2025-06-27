<template>
  <h1>
    User Search Page
  </h1>
  <v-container>

    <v-card>
      Example friend
    </v-card>

    <v-card>
      Example friend
    </v-card>
    <v-card>
      Example friend
    </v-card>
    <v-card>
      Example friend
    </v-card>
    <v-card>
      Example friend
    </v-card>

  </v-container>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { authStore } from '../stores/auth.ts'

// Components


export default defineComponent({
  name: 'UserSearchComponent',
  components: {
  },
  data() {

  },
  mounted() {
    let self = this;
    let auth = authStore();
    console.log("from Usersearcha");
    console.log(auth.isAuthenticated);
    console.log(auth.accessToken);
    console.log(auth.refreshToken);
  }
});
</script>
