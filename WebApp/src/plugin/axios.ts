import axios from 'axios'
import type { AxiosInstance } from 'axios'
import { authStore } from '@/stores/auth';

const BASE_URL = 'https://api.example.com'; // Replace with your actual API base URL
const auth = authStore();

const axiosAuthenticated: AxiosInstance = axios.create({
  baseURL: BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Optional: Add interceptors
axiosAuthenticated.interceptors.request.use(
  (config) => {
    // For example, add auth token
    const token = auth.isUserAuthenticated;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default axiosAuthenticated;
