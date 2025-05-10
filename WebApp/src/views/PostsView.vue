<template>
  <h1>Posts page</h1>

  <PostComponent v-for="post in posts" :title="post.title" :text="post.text" :key="post.title"></PostComponent>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import { defineComponent } from 'vue';
import { postStore } from '../stores/post.ts'

// Components


export default defineComponent({
  name: 'HomeView',
  components: {
    PostComponent
  },
  data() {
    return {
      posts: postStore().getCurrentPosts

    };
  }
});
</script>
