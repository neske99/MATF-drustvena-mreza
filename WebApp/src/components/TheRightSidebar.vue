<template>

  <v-navigation-drawer app permanent location="right">
    Chat
    <v-container>
      <v-card-text>
        Lorem ipsum
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-text-field placeholder="Type Message">
        </v-text-field>
        <v-btn>Send</v-btn>
      </v-card-actions>
    </v-container>
  </v-navigation-drawer>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'TheRightSidebar',
  data() {
    return {

    }
  },
})
</script>

<style scoped></style>
