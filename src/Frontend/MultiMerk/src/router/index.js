import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignupView from '@/views/authentication/SignupView.vue'
import LoginView from '@/views/authentication/LoginView.vue'
import DashboardHomeViev from '@/views/dashboard/DashboardHomeViev.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
        path: '/signup',
        name: 'signup',
        component: SignupView
    },
    {
        path: '/login',
        name: 'login',
        component: LoginView
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: DashboardHomeViev
  },
  ]
})

export default router