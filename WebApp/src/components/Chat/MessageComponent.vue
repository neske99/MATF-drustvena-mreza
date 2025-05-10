<template>
  <v-container style="width:80%"
    :style="{ marginRight: isSender ? '0px' : 'auto', marginLeft: isSender ? 'auto' : '0px' }">
    <v-card outlined :style="{ backgroundColor: isSender ? 'lightblue' : 'white' }">
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
    "message",
    "isSender"
  ],
  data() {
    return {
      //
    }
  },
})
</script>
