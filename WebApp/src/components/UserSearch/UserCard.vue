<template>
  <v-container fluid>
    <v-card :to="`/userdetail/${text}`" outlined>

      <v-card-text to="'/userdetail/'+text">
        {{ text }}
      </v-card-text>
      <v-card-text>
        {{ firstName }} {{ lastName }}
      </v-card-text>

      <v-card-actions>
        <v-btn>Show more</v-btn>
      </v-card-actions>

    </v-card>
    <v-card>
    <v-card-actions v-if="!requestSent && relation.indexOf('NONE')!=-1">
        <v-btn @click="sendRequest">Send friend request</v-btn>
      </v-card-actions>
    <v-card-actions v-if="!requestSent  && relation.indexOf('RECEIVED_FRIENDSHIP_REQUEST_FROM')!=-1">
        <v-btn @click="acceptRequest">Accept friend request</v-btn>
      </v-card-actions>

    </v-card>
  </v-container>
</template>

<script lang='ts'>
import { authStore } from '@/stores/auth';
import { userStore } from '@/stores/user';
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'UserSearchCardComponent',
  props: [/*'title',*/ 'text', 'firstName', 'lastName', 'userId','relation'],
  data() {
    return {
      requestSent :false
    }
  },
  methods:{
    sendRequest:function(){
      userStore().SendFriendRequest(authStore().userId,this.userId);
      this.requestSent=true;
    },
    acceptRequest:function(){
      userStore().AcceptFriendRequest(authStore().userId,this.userId);
      this.requestSent=true;
    }
  }
})
</script>
