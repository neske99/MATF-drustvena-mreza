<template>
  <h2>{{username}}</h2>

  <UploadPostComponent @post-uploaded="refreshPosts"></UploadPostComponent>
  <PostComponent 
    v-for="post in posts" 
    :id="post.id" 
    :text="post.text" 
    :username="post.user.username" 
    :comments="post.comments" 
    :likes="post.likes"
    :key="post.id"
    @comment-added="refreshPosts"
    @like-toggled="refreshPosts"
  ></PostComponent>
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
    await this.loadPosts();
  },
  methods: {
    async loadPosts() {
      this.username = authStore().username;
      this.posts = await postStore().GetPosts();
      console.log(this.posts);
    },
    async refreshPosts() {
      // Refresh posts after comment/like/new post actions
      await this.loadPosts();
    }
  }
});
</script>
