import { createRouter, createWebHistory, type RouteRecord, type RouteRecordRaw } from 'vue-router'
import SignupView from '@/views/auth/SignupView.vue'
import LoginView from '@/views/auth/LoginView.vue'
import NotFoundView from '@/views/NotFoundView.vue'
import { authStore } from '@/stores/auth'
import PostsView from '@/views/PostsView.vue'
import UserDetailView from '@/views/UserDetailView.vue'
import UserSearchView from '@/views/UserSearchView.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'root',
    component: { template: '<div></div>' },
    beforeEnter: (to, from, next) => {
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
    name: 'posts',
    component: PostsView,
  },
  {
    path: '/userDetail',
    name: 'userDetail',
    component: UserDetailView,
  },
  {
    path: '/userSearch',
    name: 'userSearch',
    component: UserSearchView,
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
