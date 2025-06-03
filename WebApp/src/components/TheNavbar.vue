<template>
  <v-app-bar app>
    <v-btn to="/home">Home</v-btn>

    <!-- Spacer to push the content towards the center -->
    <v-spacer></v-spacer>

    <!-- Center the text field and increase its width -->
    <v-text-field placeholder="Search" dense class="search-field"></v-text-field>

    <v-spacer></v-spacer>

    <v-btn v-if="isAuthenticated" @click="onSignoutClick">Signout</v-btn>
  </v-app-bar>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import { authStore } from '../stores/auth.ts'

export default defineComponent({
  name: 'TheNavbar',
  data() {
    return {

    }
  },
  computed: {
    isAuthenticated() {
      return authStore().isUserAuthenticated;
    }
  },
  methods: {
    async onSignoutClick() {
      let auth = authStore();
      try {
        await auth.logout();
      } catch (err) {

      }
    },

  }
})
</script>
