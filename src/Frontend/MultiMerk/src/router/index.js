import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignupView from '@/views/authentication/SignupView.vue'
import LoginView from '@/views/authentication/LoginView.vue'
import DashboardHomeViev from '@/views/dashboard/DashboardHomeView.vue'
import CreateWeeklistView from '@/views/dashboard/weeklist/tasks/CreateWeeklistView.vue'
import AllWeeklistView from '@/views/dashboard/weeklist/AllWeeklistView.vue'
import { jwtDecode } from "jwt-decode";
import CreateAIContentView from '@/views/dashboard/weeklist/tasks/content/CreateAIContentView.vue'
import UploadAIContentView from '@/views/dashboard/weeklist/tasks/content/UploadAIContentView.vue'
import AssignLocationAndQtyView from '@/views/dashboard/weeklist/tasks/warehouse/AssignLocationAndQtyView.vue'
import AssignEanView from '@/views/dashboard/weeklist/tasks/admin/AssignEanView.vue'
import CreateFinalListView from '@/views/dashboard/weeklist/tasks/admin/CreateFinalListView.vue'
import CreateTranslationsView from '@/views/dashboard/weeklist/tasks/admin/CreateTranslationsView.vue'

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
      path: '/weeklist/tasks/create',
      name: 'create-weeklist',
      component: CreateWeeklistView,
      meta: { requiresAuth: true, roles: ['Admin', 'Freelancer'] }
    },
    {
      path: '/weeklist/tasks/content/create-ai-content',
      name: 'create-ai-content',
      component: CreateAIContentView,
      meta: { requiresAuth: true, roles: ['Admin', 'Writer'] }
    },
    {
      path: '/weeklist/tasks/content/upload-ai-content',
      name: 'upload-ai-content',
      component: UploadAIContentView,
      meta: { requiresAuth: true, roles: ['Admin', 'Writer'] }
    },    
    {
      path: '/weeklist/tasks/warehouse/assign-location-qty',
      name: 'assign-location-qty',
      component: AssignLocationAndQtyView,
      meta: { requiresAuth: true, roles: ['Admin', 'WarehouseWorker', 'WarehouseManager'] }
    }, 
    {
      path: '/weeklist/tasks/admin/assign-ean/:id',
      name: 'assign-ean',
      component: AssignEanView,
      meta: { requiresAuth: true, roles: ['Admin'] }
    },    
    {
      path: '/weeklist/tasks/admin/create-final-list',
      name: 'create-final-list',
      component: CreateFinalListView,
      meta: { requiresAuth: true, roles: ['Admin'] }
    },
    {
      path: '/weeklist/tasks/admin/create-translations',
      name: 'create-translations',
      component: CreateTranslationsView,
      meta: { requiresAuth: true, roles: ['Admin'] }
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