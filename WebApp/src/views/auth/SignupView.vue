<template>
  <div>
    <h1>Sign up</h1>
  </div>

  <div>
    <v-form>
      <div>
        <label for="username">Username: </label>
        <v-text-field id="username" label="Username" />
      </div>
      <div>
        <label for="password">Password: </label>
        <v-text-field id="password" type="password" label="Password" />
      </div>
      <v-btn>Signup</v-btn>
    </v-form>
  </div>
  <p>Already have an account? <RouterLink to="/auth/login">Login</RouterLink>.</p>

</template>
