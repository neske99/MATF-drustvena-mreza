<template>
  <v-app>
    <!---->
    <TheNavbar v-if='isAuthenticated'> </TheNavbar>
    <TheLeftSidebar v-if='isAuthenticated'></TheLeftSidebar>
    <TheRightSidebar v-if='isAuthenticated'></TheRightSidebar><!---->

    <v-main style="padding-left:0px;padding-right: 0px;">
      <v-container fluid>
        <router-view />
      </v-container>
    </v-main>

  </v-app>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import TheNavbar from './components/TheNavbar.vue';
import TheLeftSidebar from './components/TheLeftSidebar.vue';
import TheRightSidebar from './components/TheRightSidebar.vue';
import { authStore } from './stores/auth';
export default defineComponent({
  name: 'App',
  components: {
    TheNavbar,
    TheRightSidebar,
    TheLeftSidebar
  },

  data() {
    return {
      //
    }

  },
  computed: {
    isAuthenticated() {
      let auth = authStore();
      console.log(auth.isAuthenticated);
      return auth.isAuthenticated;
    }
  }
})
</script>

<style scoped></style>
