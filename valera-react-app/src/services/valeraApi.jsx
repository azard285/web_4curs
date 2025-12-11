import axios from 'axios';//библиотека для всех HTTP запросов

const API_BASE_URL = 'http://localhost:5073/api/valera';

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const valeraApi = {
  // Получить всех Валер
  getAllValeras: async (alcohol = 0) => {
    const response = await api.get(`?alcohol=${alcohol}`);
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
    // Маппинг действий на правильные названия для backend
    const actionMap = {
      [ACTIONS.WORK]: "GoToWork",
      [ACTIONS.CONTEMPLATE_NATURE]: "ContemplateNature", 
      [ACTIONS.DRINK_WINE]: "DrinkWineAndWatchSeries",
      [ACTIONS.GO_TO_BAR]: "GoToBar",
      [ACTIONS.DRINK_WITH_MARGINALS]: "DrinkWithMarginals",
      [ACTIONS.SING_IN_METRO]: "SingInMetro", 
      [ACTIONS.SLEEP]: "Sleep"
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
  }
};

// Список доступных действий (остается как было)
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