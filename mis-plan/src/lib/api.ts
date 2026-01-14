// ============================================
// FILE: src/lib/api.ts
// ============================================
import axios from 'axios';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:44315/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add JWT token
apiClient.interceptors.request.use(
  (config) => {
     if (typeof window !== 'undefined') {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor for handling errors
apiClient.interceptors.response.use(
  (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        if (typeof window !== 'undefined') {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export const authAPI = {
  login: async (credentials: { username: string; password: string }) => {
    const response = await apiClient.post('/auth/login', credentials);
    return response.data;
  },
  
  logout: async () => {
    const response = await apiClient.post('/auth/logout');
    return response.data;
  },
};

export const userAPI = {
  getByUsername: async (username: string) => {
    const response = await apiClient.get(`/user/get_by_username?userName=${username}`);
    return response.data;
  },
  
  // Alternative: using the list endpoint with filter
  getUsersList: async (id?: number, userName?: string, fullName?: string) => {
    const params = new URLSearchParams();
    if (id) params.append('id', id.toString());
    if (userName) params.append('userName', userName);
    if (fullName) params.append('fullName', fullName);
    
    const response = await apiClient.get(`/user/list?${params.toString()}`);
    return response.data;
  },
};

export const officeAPI = {
  getLoginOfficeDetails: async () => {
    const response = await apiClient.get('/auth/login-office-details');
    return response.data;
  },
};

export const generalSetupAPI = {
  getAll: async () => {
    const response = await apiClient.get('/generalsetup');
    return response.data;
  },
};

export const masterSetupAPI = {
  getAll: async () => {
    const response = await apiClient.get('/mastersetup');
    return response.data;
  },
};

export default apiClient;