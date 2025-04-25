import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignupView from '@/views/authentication/SignupView.vue'
import LoginView from '@/views/authentication/LoginView.vue'
import DashboardHomeViev from '@/views/dashboard/DashboardHomeViev.vue'
import CreateWeeklistView from '@/views/dashboard/weeklist/CreateWeeklistView.vue'
import AllWeeklistView from '@/views/dashboard/weeklist/AllWeeklistView.vue'

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
    {
      path: '/weeklist/create',
      name: 'create-weeklist',
      component: CreateWeeklistView
    },
    {
      path: '/weeklist/all',
      name: 'all-weeklist',
      component: AllWeeklistView
    },

  ]
})

export default router