import './assets/index.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'


createApp(App)
.use(router)
.provide('apiUrl', 'http://localhost:5020')
.mount('#app')
