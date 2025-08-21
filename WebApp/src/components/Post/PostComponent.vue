<template>
  <v-container fluid>
    <v-card outlined>

      <v-card-title >
        <router-link :to="'/userdetail/' + username">
          {{ username }}
        </router-link>
      </v-card-title>

      <v-card-text>
        {{ text }}
      </v-card-text>

      <!-- Likes Section -->
      <v-card-actions>
        <v-btn 
          :color="userHasLiked ? 'red' : 'grey'" 
          :icon="userHasLiked ? 'mdi-heart' : 'mdi-heart-outline'"
          @click="toggleLike"
          size="small"
        >
        </v-btn>
        <span class="ml-1">{{ likes?.length || 0 }}</span>
        
        <v-spacer></v-spacer>
        
        <!-- Show who liked the post -->
        <v-menu v-if="likes && likes.length > 0" offset-y>
          <template v-slot:activator="{ props }">
            <v-btn 
              text 
              size="small" 
              v-bind="props"
            >
              {{ likes.length === 1 ? '1 like' : `${likes.length} likes` }}
            </v-btn>
          </template>
          <v-list dense>
            <v-list-item v-for="like in likes" :key="like.id">
              <v-list-item-title v-if="like.user">
                <router-link :to="'/userdetail/'+ like.user.username">
                  {{ like.user.username }}
                </router-link>
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-card-actions>

      <v-divider></v-divider>

    <v-card-subtitle class="mt-2 mb-1">Comments</v-card-subtitle>
      <v-list dense>
        <v-list-item v-for="comment in comments" :key="comment.id">
            <v-list-item-title>{{ comment.text }}</v-list-item-title>
            <v-list-item-subtitle v-if="comment.user"><router-link :to="'/userdetail/'+ comment.user.username">{{ comment.user.username }} </router-link></v-list-item-subtitle>
        </v-list-item>
        <v-list-item v-if="comments.length === 0">
            <v-list-item-title class="text--disabled">No comments yet.</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-text-field
        v-model.trim="newComment"
        label="Add a comment"
        outlined
        dense
        class="mt-2"
      ></v-text-field>

      <v-card-actions>
        <v-btn @click="addComment">Comment</v-btn>
      </v-card-actions>

    </v-card>
  </v-container>
</template>

<script lang='ts'>
import { postStore } from '@/stores/post';
import { authStore } from '@/stores/auth';
import { defineComponent } from 'vue'
import type { likeDTO } from '@/dtos/post/likeDTO';

export default defineComponent({
  name: 'PostComponent',
  props: ['text', 'username', 'comments', 'likes', 'id'],
  data() {
    return {
      newComment: ''
    }
  },
  computed: {
    userHasLiked(): boolean {
      if (!this.likes || !Array.isArray(this.likes)) return false;
      const currentUserId = authStore().userId;
      return this.likes.some((like: likeDTO) => like.user?.id === currentUserId);
    }
  },
  methods: {
    addComment: async function() {
      if (this.newComment !== '') {
        await postStore().AddComment(this.newComment, this.id);
        this.newComment = '';
        // Emit event to parent to refresh posts or handle optimistic update
        this.$emit('comment-added');
      }
    },
    toggleLike: async function() {
      try {
        if (this.userHasLiked) {
          await postStore().RemoveLike(this.id);
        } else {
          await postStore().AddLike(this.id);
        }
        // Emit event to parent to refresh posts or handle optimistic update
        this.$emit('like-toggled');
      } catch (error) {
        console.error('Error toggling like:', error);
      }
    }
  }
})
</script>

<style scoped>
.v-btn {
  text-transform: none;
}
</style>
