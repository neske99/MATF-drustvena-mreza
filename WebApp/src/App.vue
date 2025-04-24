<template>
  <v-app>
    <TheNavbar></TheNavbar>
    <TheLeftSidebar></TheLeftSidebar>

    <v-main>
      <router-view />
    </v-main>

    <TheRightSidebar></TheRightSidebar>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import TheNavbar from './components/TheNavbar.vue';
import TheLeftSidebar from './components/TheLeftSidebar.vue';
import TheRightSidebar from './components/TheRightSidebar.vue';
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
})
</script>

<style scoped></style>
