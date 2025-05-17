import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'


import App from './App.vue'
import router from './router'
import vuetify from './plugin/vuetify.ts'
import piniaPersist from 'pinia-plugin-persistedstate'

const pinia = createPinia();
pinia.use(piniaPersist);


const app = createApp(App)

app.use(pinia);
app.use(router);
app.use(vuetify);

app.mount('#app');
