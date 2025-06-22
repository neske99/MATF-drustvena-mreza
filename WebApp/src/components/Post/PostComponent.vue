<template>
  <v-container fluid>
    <v-card outlined>

      <v-card-text>
        {{ text }}
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
  name: 'PostComponent',
  props: [/*'title',*/ 'text'],
  data() {
    return {


    }
  },
})
</script>
