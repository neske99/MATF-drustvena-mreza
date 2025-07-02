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
  </v-container>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'UserSearchCardComponent',
  props: [/*'title',*/ 'text', 'firstName', 'lastName', 'userId'],
  data() {
    return {

    }
  },
})
</script>
