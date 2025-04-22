<template>
  <v-app-bar app>
    <v-btn to="/home">Home</v-btn>

    <!-- Spacer to push the content towards the center -->
    <v-spacer></v-spacer>

    <!-- Center the text field and increase its width -->
    <v-text-field placeholder="Search" dense class="search-field"></v-text-field>

    <v-spacer></v-spacer>

    <v-btn to="/auth/login">Signout</v-btn>
  </v-app-bar>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'TheNavbar',
  data() {
    return {
      drawer: false,
      links: [
        { icon: 'dashboard', text: 'Dashboard', route: '/' },
        { icon: 'folder', text: 'My Projects', route: '/projects' },
        { icon: 'person', text: 'Team', route: '/team' },
      ]
    }
  },
})
</script>
