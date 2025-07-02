import { createRouter, createWebHistory, type RouteRecord, type RouteRecordRaw } from 'vue-router'
import SignupView from '@/views/auth/SignupView.vue'
import LoginView from '@/views/auth/LoginView.vue'
import NotFoundView from '@/views/NotFoundView.vue'
import { authStore } from '@/stores/auth'
import UserDetailView from '@/views/UserDetailView.vue'
import UserSearchView from '@/views/UserSearchView.vue'
import PostsView from '@/views/PostsView.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'root',
    component: { template: '<div></div>' },
    beforeEnter: (to, from, next) => {
      const auth = authStore();
      console.log('router:' + auth.isAuthenticated);
      if (auth.isAuthenticated) {
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
    path: '/userDetail/:username',
    name: 'userDetail',
    component: UserDetailView,
    props: true
  },
  {
    path: '/userSearch/:userSearchName',
    name: 'userSearch',
    component: UserSearchView,
    props: true
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
});

router.beforeEach((to, from, next) => {
  const auth = authStore();
  const publicPages = ['/auth/login', '/auth/signup'];
  const authRequired = !publicPages.includes(to.path);

  console.log(auth.isAuthenticated);
  console.log(auth.accessToken);
  console.log(auth.refreshToken);


  if (authRequired && !auth.isAuthenticated) {
    return next('/auth/login');
  }

  next();
});

export default router
