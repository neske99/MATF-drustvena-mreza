<template>
  <v-navigation-drawer app permanent class="d-flex flex-column">

    <template v-slot:append>
      <v-list class="flex-grow-1">
        <v-list-item v-for="friend in filteredFriends" :key="friend.chatId" v-on:click="openChatWithFriend(friend)" :style="friend.chatId === currentChatGroupId ? 'background-color: #aef2e0;' : ''">
          <v-list-item-title :style="{fontWeight: friend.hasNewMessages ? 'bold' : 'normal' }"> {{ friend.username }} </v-list-item-title>
        </v-list-item>

      </v-list>
      <v-card-actions>
        <v-text-field placeholder="Type User" v-model.trim="searchedUser">
        </v-text-field>
      </v-card-actions>

    </template>
  </v-navigation-drawer>
</template>




<script lang='ts'>
import { defineComponent } from 'vue'
import { chatStore } from '../stores/chat.ts'
import type { HubConnection } from '@microsoft/signalr';
import type { ChatGroupDTO } from '@/dtos/chat/chatGroupDTO.ts';
import { getSignalRConnection } from '../plugin/signalr.ts'


export default defineComponent({
  name: 'TheLeftNavbar',
  data() {
    return {
      searchedUser: "",
      connection:null as HubConnection | null
    }
  },
  created: async function(){
    let self=this;

    await chatStore().getChatGroupsForUser();
    console.log(self.friends);

    this.connection =  await getSignalRConnection();
    if(this.connection) {
      this.connection.on("ReceiveMessage", (chatGroupId: number, userId: number,username:string) => {
        self.friends.push({username:username,chatId:chatGroupId,userId:userId, hasNewMessages:true});
        self.connection?.invoke("RegisterToGroup",chatGroupId);

      });
    }
  },
  computed: {
    filteredFriends() {
      return this.friends.filter(x => x.username.toLowerCase().includes(this.searchedUser.toLowerCase()));
    },
    friends(){
      return chatStore().currentChatGroups;
    },
    currentChatGroupId() {
      return chatStore().currentChatGroupId;
    }
  },
  methods: {
    openChatWithFriend(friend: ChatGroupDTO) {
      chatStore().switchUserChat(friend);
      friend.hasNewMessages = false; // Reset new messages flag
    }
  }
})
</script>
