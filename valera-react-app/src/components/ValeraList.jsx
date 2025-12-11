import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; // Добавляем этот хук
import { valeraApi } from '../services/valeraApi';
import CreateValeraModal from './CreateValeraModal';

const ValeraList = ({ user }) => { // Получаем user вместо onValeraSelect
  const [valeras, setValeras] = useState([]);
  const [searchId, setSearchId] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const navigate = useNavigate(); // Хук для навигации

  const loadValeras = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await valeraApi.getValeras(); // Используем исправленный метод
      setValeras(data);
    } catch (err) {
      setError('Ошибка загрузки Валер: ' + (err.response?.data || err.message));
      console.error('Error loading valeras:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadValeras();
  }, []);

  const handleCreateValera = async (valeraData) => {
    try {
      await valeraApi.createValera(valeraData);
      setIsModalOpen(false);
      await loadValeras();
    } catch (err) {
      setError('Ошибка создания Валеры: ' + (err.response?.data || err.message));
    }
  };

  // Функция перехода к деталям Валеры
  const handleValeraClick = (valeraId) => {
    navigate(`/valeras/${valeraId}`);
  };

  const filteredValeras = valeras.filter(valera =>
    valera.id.toString().includes(searchId)
  );

  return (
    <div className="container">
      <h1>🤪🤤🍻 Бухарик Валера</h1>
      
      {/* Показываем информацию о пользователе */}
      {user && (
        <div className="user-info">
          <p>👤 {user.username} ({user.role})</p>
        </div>
      )}
      
      <div className="controls">
        <input
          type="text"
          placeholder="🔍 Поиск по ID..."
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
          className="search-input"
        />
        <button 
          onClick={() => setIsModalOpen(true)}
          className="btn-primary"
        >
          ➕ Создать Валеру
        </button>
      </div>

      {error && <div className="error">{error}</div>}

      {loading ? (
        <div className="loading">Загрузка...</div>
      ) : filteredValeras.length === 0 ? (
        <div className="empty-state">
          📝 {valeras.length === 0 ? 'Нет Валер. Создайте первого!' : 'Валеры не найдены'}
        </div>
      ) : (
        <div className="valera-grid">
          {filteredValeras.map(valera => (
            <div 
              key={valera.id} 
              className="valera-card"
              onClick={() => handleValeraClick(valera.id)} // Изменено здесь
            >
              <h3>Валера #{valera.id}</h3>
              <div className="stats-preview">
                <div>❤️ Здоровье: {valera.health}</div>
                <div>🍺 Алкоголь: {valera.alcohol}</div>
                <div>😊 Жизнерадостность: {valera.joy}</div>
                <div>😴 Усталость: {valera.fatigue}</div>
                <div>💰 Деньги: {valera.money} ₽</div>
              </div>
              <div className="card-action">Нажмите для управления →</div>
            </div>
          ))}
        </div>
      )}

      <CreateValeraModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCreate={handleCreateValera}
      />
    </div>
  );
};

export default ValeraList;