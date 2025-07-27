<template>
  <v-container fluid>
    <v-card outlined>


      <v-text-field
        v-model="newPost"
        label="Upload a post"
        outlined
        dense
        class="mt-2"
      ></v-text-field>

      <v-card-actions>
        <v-btn @click="uploadPost">Upload post</v-btn>
      </v-card-actions>

    </v-card>
  </v-container>
</template>

<script lang='ts'>
import { authStore } from '@/stores/auth';
import { postStore } from '@/stores/post';
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'PostComponent',
  props: [],
  data() {
    return {
      newPost:''
    }
  },
  methods:{
    uploadPost: async function(){
      let userId= authStore().userId;
      await postStore().UploadPost(this.newPost, userId);



    }

  }
})
</script>
