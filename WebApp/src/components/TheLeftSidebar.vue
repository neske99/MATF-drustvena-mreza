<template>

  <h1>
    Friends
  </h1>
  <v-navigation-drawer app permanent class="d-flex flex-column">

    <template v-slot:append>
      <v-list class="flex-grow-1">
        <v-list-item router to="/home">
          <v-list-item-title>Friend 1</v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title>Friend 2</v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title>Friend 3</v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title>Friend 4</v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title>Friend 5</v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title>Friend 6</v-list-item-title>
        </v-list-item>

      </v-list>
      <v-card-actions>
        <v-text-field placeholder="Type User">
        </v-text-field>
        <v-btn>Send</v-btn>
      </v-card-actions>

    </template>
  </v-navigation-drawer>
</template>




<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'TheLeftNavbar',
  data() {
    return {
    }
  },
})
</script>
