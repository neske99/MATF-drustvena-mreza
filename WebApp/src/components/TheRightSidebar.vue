<template>

  <v-navigation-drawer app permanent location="right">
    <template v-slot:append>
      <MessageComponent v-for="message in messages" :message="message.message" :key="message.message"
        :isSender="message.isSender">
      </MessageComponent>

      <v-divider></v-divider>
      <v-card-actions>
        <v-text-field placeholder="Type Message" v-model="messageToSend">
        </v-text-field>
        <v-btn v-on:click="onSend">Send</v-btn>
      </v-card-actions>
    </template>
  </v-navigation-drawer>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'
import MessageComponent from './Chat/MessageComponent.vue';
import { chatStore } from '../stores/chat.ts'
import { startSignalRConnection,getSignalRConnection,stopSignalRConnection} from '../plugin/signalr.ts'
import type { HubConnection } from '@microsoft/signalr';
import { authStore } from '@/stores/auth.ts';

export default defineComponent({
  name: 'TheRightSidebar',
  components: {
    MessageComponent
  },
  data() {
    return {
      messages: chatStore().getCurrentMessages,
      messageToSend: "",
      connection:null as HubConnection | null
    }
  },
  async created() {
    this.connection =  await startSignalRConnection();
    this.connection.on("ReceiveMessage", (username: string, message: string) => {
      this.messages.push({ message:message, isSender: username === authStore().username });
    });
  },
  methods: {
    onSend() {
      console.log(this.messageToSend);
      this.connection?.invoke("SendMessage",authStore().username,this.messageToSend)
      this.messageToSend = "";
    }
  }
})
</script>

<style scoped></style>
