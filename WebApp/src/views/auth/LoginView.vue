<template>
  <div>
    <h1>Login</h1>
  </div>

  <div>
    <v-form>
      <div>
        <v-text-field id="username" label="Username" />
      </div>
      <div>
        <v-text-field id="password" label="Password" type="password" />
      </div>
      <v-btn>Login</v-btn>
    </v-form>
    <p>Don't have an account? <RouterLink to="/auth/signup">Sign up</RouterLink>.</p>
  </div>
</template>
