<template>
  <h1>Posts page</h1>
  <PostComponent></PostComponent>
  <v-spacer></v-spacer>
  <PostComponent></PostComponent>
  <v-spacer></v-spacer>
  <PostComponent></PostComponent>
  <v-spacer></v-spacer>
  <PostComponent></PostComponent>
  <v-spacer></v-spacer>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';

// Components


export default defineComponent({
  name: 'HomeView',

  components: {
    PostComponent
  },
});
</script>
