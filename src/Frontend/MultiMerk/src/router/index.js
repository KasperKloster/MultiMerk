import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignupView from '@/views/authentication/SignupView.vue'
import LoginView from '@/views/authentication/LoginView.vue'
import DashboardHomeViev from '@/views/dashboard/DashboardHomeViev.vue'
import CreateWeeklistView from '@/views/dashboard/weeklist/CreateWeeklistView.vue'
import AllWeeklistView from '@/views/dashboard/weeklist/AllWeeklistView.vue'
import { jwtDecode } from "jwt-decode";

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
      component: DashboardHomeViev,
      meta: { requiresAuth: true }
    },
    {
      path: '/weeklist/all',
      name: 'all-weeklist',
      component: AllWeeklistView,
      meta: { requiresAuth: true }
    },
    {
      path: '/weeklist/create',
      name: 'create-weeklist',
      component: CreateWeeklistView,
      meta: { requiresAuth: true, roles: ['Admin', 'Freelancer'] }
    },
  ]
});

// Authorize routes
router.beforeEach((to, from, next) => {
  const isLoggedIn = !!localStorage.getItem('multimerk_accessToken');  
  var userRole = '';
  if (isLoggedIn){    
    try {
      // Decode the token to get user role
      const decoded = jwtDecode(localStorage.getItem('multimerk_accessToken'));                
      // Check if the token has a role property and return it in lowercase
      var userRole = decoded.role;      
    } catch (error) {
      console.error("Error decoding token in router:", error);      
    }
  }
  
  // User is not logged in
  if (to.meta.requiresAuth && !isLoggedIn) {
    return next('/login')
  }
  // User has not the required role
  if (to.meta.roles && !to.meta.roles.includes(userRole)) {
    return next('/')
  }

  next()
})

export default router