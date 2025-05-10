<template>
  <v-navigation-drawer app permanent class="d-flex flex-column">

    <template v-slot:append>
      <v-list class="flex-grow-1">
        <v-list-item v-for="friend in filteredFriends" :key="friend" v-on:click="openChatWithFriend(friend)">
          <v-list-item-title>{{ friend }}</v-list-item-title>
        </v-list-item>

      </v-list>
      <v-card-actions>
        <v-text-field placeholder="Type User" v-model.trim="searchedUser">
        </v-text-field>
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
      friends: ["Mika", "Zika", "Pera"],
      searchedUser: ""
    }
  },
  computed: {
    filteredFriends() {
      return this.friends.filter(x => x.toLowerCase().includes(this.searchedUser.toLowerCase()));
    }
  },
  methods: {
    openChatWithFriend(friend: string) {
      console.log(friend);
    }
  }
})
</script>
