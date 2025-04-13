<template>
    <div>
        <h1>Login</h1>
    </div>

    <div>
        <form >
            <div>
                <label for="username">Username: </label>
                <input id="username" type="text">
            </div>
            <div>
                <label for="password">Password: </label>
                <input id="password" type="password">
            </div>
        </form>
        <p>Don't have an account? <RouterLink to="/auth/signup">Sign up</RouterLink>.</p>
    </div>
</template>