<template>

  <v-navigation-drawer app permanent location="right">

    Chat
    <template v-slot:append>
      <MessageComponent></MessageComponent>
      <MessageComponent></MessageComponent>
      <MessageComponent></MessageComponent>
      <MessageComponent></MessageComponent>

      <v-divider></v-divider>
      <v-card-actions>
        <v-text-field placeholder="Type Message">
        </v-text-field>
        <v-btn>Send</v-btn>
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

    }
  },
})
</script>

<style scoped></style>
