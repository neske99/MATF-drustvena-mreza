<template>
    <div>
        <h1>Sign up</h1>
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
    </div>
    <p>Already have an account? <RouterLink to="/auth/login">Login</RouterLink>.</p>

</template>






