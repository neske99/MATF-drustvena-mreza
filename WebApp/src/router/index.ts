import { createRouter, createWebHistory, type RouteRecord, type RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import  SignupView  from '@/views/auth/SignupView.vue'
import LoginView from '@/views/auth/LoginView.vue'
import NotFoundView from '@/views/NotFoundView.vue'
import { authStore } from '@/stores/auth'

const routes: RouteRecordRaw[] = [
    {
      path: '/',
      name:'root',
      component:{template:'<div></div>'},
      beforeEnter: (to, from , next)=>{
        const auth = authStore();
        if (auth.isUserAuthenticated) {
          next('/home')
        } else {
          next('/auth/login')
        }
      }
    },    
    {
      path: '/home',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/auth/signup',
      name: 'signup',
      component: SignupView,
    },
    {
      path: '/auth/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: NotFoundView
    }
  ]


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

export default router
