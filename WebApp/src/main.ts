import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'


import App from './App.vue'
import router from './router'
import vuetify from './plugin/vuetify.ts'
import piniaPersist from 'pinia-plugin-persistedstate'
import signalR from './plugin/signalr.ts'

const pinia = createPinia();
pinia.use(piniaPersist);


const app = createApp(App)

app.use(pinia);
app.use(router);
app.use(vuetify);
app.use(signalR);

app.mount('#app');
