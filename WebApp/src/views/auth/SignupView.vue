<template>
  <div>
    <h1>Sign up</h1>
  </div>

  <div>
    <v-form>
      <div>
        <label for="username">Username: </label>
        <v-text-field id="username" label="Username" v-model="username" />
      </div>
      <div>
        <label for="password">Password: </label>
        <v-text-field id="password" type="password" label="Password" v-model="password" />
      </div>
      <v-btn @click="onSignUpClick">Signup</v-btn>
    </v-form>
  </div>
  <p>Already have an account? <RouterLink to="/auth/login">Login</RouterLink>.</p>

</template>

<script lang="ts">

import { defineComponent } from 'vue';
import { authStore } from '../../stores/auth.ts'
// Components

export default defineComponent({
  name: 'Signup',
  data() {
    return {
      username: '',
      password: ''
    };
  },
  methods: {
    async onSignUpClick() {
      try {
        let store = authStore();
        await store.signup(this.username, this.password);
      } catch (err) {

      }
    }
  }


});

</script>
