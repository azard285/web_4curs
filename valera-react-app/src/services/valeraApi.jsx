// valera-react-app/src/services/valeraApi.jsx
import axios from 'axios';
import { authApi } from './authApi';

const API_BASE_URL = 'http://localhost:5073/api/valera';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Интерцептор для добавления токена
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

// Интерцептор для обработки ошибок аутентификации
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      authApi.logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const valeraApi = {
  getValeras: async (alcohol = 0) => {
    const user = authApi.getCurrentUser(); // Получаем данные текущего пользователя
    let endpoint = '/my'; // По умолчанию для пользователя

    if (user && user.role === 'Admin') {
      endpoint = ''; // Для админа используем корневой эндпоинт
    }

    const response = await api.get(`${endpoint}?alcohol=${alcohol}`);
    return response.data;
  },

  // Получить всех Валер (только для админа)
  getAllValeras: async (alcohol = 0) => {
    const response = await api.get(`?alcohol=${alcohol}`);
    return response.data;
  },

  // Получить моих Валер
  getMyValeras: async (alcohol = 0) => {
    const response = await api.get(`/my?alcohol=${alcohol}`);
    return response.data;
  },

  // Получить Валеру по ID
  getValeraById: async (id) => {
    const response = await api.get(`/${id}`);
    return response.data;
  },

  // Создать новую Валеру
  createValera: async (valeraData) => {
    const response = await api.post('', valeraData);
    return response.data;
  },

  // Выполнить действие с Валерей
  executeAction: async (id, action) => {
    const actionMap = {
      work: "GoToWork",
      contemplate_nature: "ContemplateNature", 
      drink_wine: "DrinkWineAndWatchSeries",
      go_to_bar: "GoToBar",
      drink_with_marginals: "DrinkWithMarginals",
      sing_in_metro: "SingInMetro", 
      sleep: "Sleep"
    };
    
    const backendAction = actionMap[action];
    if (!backendAction) {
      throw new Error(`Unknown action: ${action}`);
    }

    const response = await api.post(`/${id}/actions`, `"${backendAction}"`, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    return response.data;
  },

  // Удалить Валеру
  deleteValera: async (id) => {
    await api.delete(`/${id}`);
  },

  // Проверка прав админа
  isAdmin: () => {
    const user = authApi.getCurrentUser();
    return user?.role === 'Admin';
  }
};

export const ACTIONS = {
  WORK: 'work',
  CONTEMPLATE_NATURE: 'contemplate_nature',
  DRINK_WINE: 'drink_wine', 
  GO_TO_BAR: 'go_to_bar',
  DRINK_WITH_MARGINALS: 'drink_with_marginals',
  SING_IN_METRO: 'sing_in_metro',
  SLEEP: 'sleep'
};

export const ACTION_LABELS = {
  [ACTIONS.WORK]: 'Пойти на работу',
  [ACTIONS.CONTEMPLATE_NATURE]: 'Созерцать природу',
  [ACTIONS.DRINK_WINE]: 'Пить вино и смотреть сериал',
  [ACTIONS.GO_TO_BAR]: 'Сходить в бар', 
  [ACTIONS.DRINK_WITH_MARGINALS]: 'Выпить с маргинальными личностями',
  [ACTIONS.SING_IN_METRO]: 'Петь в метро',
  [ACTIONS.SLEEP]: 'Спать'
};