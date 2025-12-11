// valera-react-app/src/services/authApi.jsx
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5073/api/auth';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

export const authApi = {
  // Регистрация
  register: async (email, password, username) => {
    console.log('Registering:', { email, username });
    try {
      const response = await api.post('/register', { 
        email, 
        password, 
        username 
      });
      
      console.log('Register response:', response.data);
      
      // Сохраняем токен в localStorage
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('user', JSON.stringify({
          email: response.data.email,
          username: response.data.username,
          role: response.data.role
        }));
      }
      
      return response.data;
    } catch (error) {
      console.error('Register error:', error.response?.data || error.message);
      throw error;
    }
  },

  // Логин
  login: async (email, password) => {
    console.log('Logging in:', { email });
    try {
      const response = await api.post('/login', { 
        email, 
        password 
      });
      
      console.log('Login response:', response.data);
      
      // Сохраняем токен в localStorage
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('user', JSON.stringify({
          email: response.data.email,
          username: response.data.username,
          role: response.data.role
        }));
      }
      
      return response.data;
    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);
      throw error;
    }
  },

  // Выход
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  // Проверка аутентификации
  isAuthenticated: () => {
    const token = localStorage.getItem('token');
    console.log('Checking auth, token exists:', !!token);
    return !!token;
  },

  // Получение текущего пользователя
  getCurrentUser: () => {
    const userStr = localStorage.getItem('user');
    const user = userStr ? JSON.parse(userStr) : null;
    console.log('Getting current user:', user);
    return user;
  },

  // Получение токена
  getToken: () => {
    return localStorage.getItem('token');
  }
};

// Интерцептор для добавления токена к запросам
api.interceptors.request.use(
  (config) => {
    const token = authApi.getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);