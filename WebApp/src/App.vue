<template>
  <v-app>
    <!---->
    <TheNavbar></TheNavbar>
    <TheLeftSidebar></TheLeftSidebar>
    <TheRightSidebar></TheRightSidebar><!---->

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
