<template>
  <v-app-bar app>
    <v-btn to="/home">Home</v-btn>

    <!-- Spacer to push the content towards the center -->
    <v-spacer></v-spacer>

    <!-- Center the text field and increase its width -->
    <v-text-field placeholder="Search" dense class="search-field" v-model="searchText"></v-text-field>
    <v-btn @click="search" to="/usersearc/${searchText}">Search</v-btn>

    <v-spacer></v-spacer>

    <v-btn v-if="isAuthenticated" @click="onSignoutClick">Signout</v-btn>
  </v-app-bar>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import { authStore } from '../stores/auth.ts'
import { userStore } from '../stores/user.ts'

export default defineComponent({
  name: 'TheNavbar',
  data() {
    return {
      searchText: "" as string
    }
  },
  computed: {
    isAuthenticated() {
      return authStore().isAuthenticated;
    },
  },
  methods: {
    async onSignoutClick() {
      let auth = authStore();
      try {
        await auth.logout();
      } catch (err) {

      }
    },
    async search() {
      let self = this;
      this.$router.push(`/usersearch/${self.searchText}`);
    }

  }
})
</script>
