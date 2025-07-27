<template>
  <h2>{{username}}</h2>

  <UploadPostComponent></UploadPostComponent>
  <PostComponent v-for="post in posts" :id="post.id" :text="post.text" :username="post.user.username" :comments="post.comments" :key="post.id"></PostComponent>
</template>

<script lang="ts">
import PostComponent from '@/components/Post/PostComponent.vue';
import UploadPostComponent from '@/components/Post/UploadPostComponent.vue';
import { defineComponent } from 'vue';
import { postStore } from '../stores/post.ts'
import  type { postDTO }  from '../dtos/post/postDTO.ts';
import { authStore } from '@/stores/auth.ts';

// Components


export default defineComponent({
  name: 'HomeView',
  components: {
    PostComponent,
    UploadPostComponent
  },
  data() {
    return {
      username:'',
      posts: [] as postDTO[]
    };
  },
  async created() {
    let self = this;

    self.username = authStore().username;
    self.posts = await postStore().GetPosts();

    console.log(self.posts);
  }
});
</script>
