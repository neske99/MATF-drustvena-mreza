<template>
  <v-container>
    <v-card outlined>
      <v-card-text>
        {{ message }}
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'MessageComponent',
  props: [
    "message"
  ],
  data() {
    return {
      //
    }
  },
})
</script>
