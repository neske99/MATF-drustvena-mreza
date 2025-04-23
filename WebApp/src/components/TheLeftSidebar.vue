<template>

  <v-navigation-drawer app permanent>
    Friends
    <v-container>
      <v-card outlined>
        <v-card-actions>
          <v-btn>Friend1</v-btn>
        </v-card-actions>
      </v-card>
      <v-spacer></v-spacer>
      <v-card outlined>
        <v-card-actions>
          <v-btn>Friend2</v-btn>
        </v-card-actions>
      </v-card>
      <v-spacer></v-spacer>
      <v-card outlined>
        <v-card-actions>
          <v-btn>Friend3</v-btn>
        </v-card-actions>
      </v-card>

      <v-spacer></v-spacer>
      <v-card outlined>
        <v-card-actions>
          <v-btn>Friend4</v-btn>
        </v-card-actions>
      </v-card>

      <v-spacer></v-spacer>
      <v-card outlined>
        <v-card-actions>
          <v-btn>Friend5</v-btn>
        </v-card-actions>
      </v-card>


    </v-container>

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
