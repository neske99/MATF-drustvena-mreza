<template>
  <v-container fluid>
    <v-card outlined>

      <v-card-title>
        {{ username }}
      </v-card-title>

      <v-card-text>
        {{ text }}
      </v-card-text>

      <v-divider></v-divider>

    <v-card-subtitle class="mt-2 mb-1">Comments</v-card-subtitle>
      <v-list dense>
        <v-list-item v-for="comment in comments" :key="comment.id">
            <v-list-item-title>{{ comment.text }}</v-list-item-title>
            <v-list-item-subtitle v-if="comment.user">{{ comment.user.username }}</v-list-item-subtitle>
        </v-list-item>
        <v-list-item v-if="comments.length === 0">
            <v-list-item-title class="text--disabled">No comments yet.</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-text-field
        v-model="newComment"
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
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'PostComponent',
  props: [/*'title',*/ 'text', 'username', 'comments','id'],
  data() {
    return {
      newComment:''
    }
  },
  methods:{
    addComment: async function(){
      await postStore().AddComment(this.newComment, this.id);



    }

  }
})
</script>
