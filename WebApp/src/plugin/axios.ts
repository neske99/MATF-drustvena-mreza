import axios, { AxiosError } from 'axios'
import type { AxiosInstance, AxiosRequestConfig } from 'axios'
import { authStore } from '@/stores/auth';

let isRefreshing = false;
let failedRequests = [];
const axiosAuthenticated: AxiosInstance = axios.create({
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Optional: Add interceptors
axiosAuthenticated.interceptors.request.use(
  (config) => {
    // For example, add auth token
    const auth = authStore();
    const token = auth.accessToken;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

axiosAuthenticated.interceptors.response.use(
  (response) => response,
  async (Error: AxiosError) => {
    const config = Error.config as AxiosRequestConfig & { _retry: boolean };
    if (Error.response?.status == 401 && !config._retry) {
      //TODO add mutex
      if (!isRefreshing) {

        isRefreshing = true;
        config._retry = true;
        failedRequests.push(config);
        const auth = authStore();
        await auth.refresh();
        isRefreshing = false;
        failedRequests.forEach((config) => {
          axiosAuthenticated(config);
        })

      } else {
        config._retry = true;
        failedRequests.push(config);
      }
    } else {
      throw Error;
    }
  }
);



export default axiosAuthenticated;
