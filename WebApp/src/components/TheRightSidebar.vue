<template>

  <v-navigation-drawer app permanent location="right">
    <template v-slot:append>
      <MessageComponent v-for="message in messages" :message="message"></MessageComponent>

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


export default defineComponent({
  name: 'TheRightSidebar',
  components: {
    MessageComponent
  },
  data() {
    return {
      messages: ["Dobar dan", "Dobro jutro!", "Da li moze?", "Moze", "Hvala!"],
      messageToSend: ""
    }
  },
  methods: {
    onSend() {
      console.log(this.messageToSend);
      this.messageToSend = "";
    }
  }
})
</script>

<style scoped></style>
