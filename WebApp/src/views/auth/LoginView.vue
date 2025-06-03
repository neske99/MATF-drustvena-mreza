<template>
  <div>
    <h1>Login</h1>
  </div>

  <div>
    <v-form>
      <div>
        <v-text-field id="username" label="Username" v-model="username" />
      </div>
      <div>
        <v-text-field id="password" label="Password" type="password" v-model="password" />
      </div>
      <v-btn @click="onLoginClick">Login</v-btn>
    </v-form>
    <p>Don't have an account? <RouterLink to="/auth/signup">Sign up</RouterLink>.</p>
  </div>
</template>

<script lang="ts">

import { defineComponent } from 'vue';
import { authStore } from '../../stores/auth.ts'
// Components

export default defineComponent({
  name: 'Login',
  data() {
    return {
      username: '',
      password: ''
    };
  },
  methods: {
    async onLoginClick() {
      try {
        let store = authStore();
        await store.login(this.username, this.password);
      } catch (err) {

      }
    }
  }


});

</script>
