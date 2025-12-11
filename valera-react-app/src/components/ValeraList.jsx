import React, { useState, useEffect } from 'react';
import { valeraApi } from '../services/valeraApi';
import CreateValeraModal from './CreateValeraModal';

const ValeraList = ({ onValeraSelect }) => {
  const [valeras, setValeras] = useState([]);// Список Валер
  const [searchId, setSearchId] = useState('');// Поисковый запрос
  const [isModalOpen, setIsModalOpen] = useState(false);// Открыта ли модалка
  const [loading, setLoading] = useState(false);// Загрузка данных
  const [error, setError] = useState('');// Ошибки

  const loadValeras = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await valeraApi.getAllValeras();
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
      await loadValeras(); // Перезагружаем список
    } catch (err) {
      setError('Ошибка создания Валеры: ' + (err.response?.data || err.message));
    }
  };

  const filteredValeras = valeras.filter(valera =>
    valera.id.toString().includes(searchId)
  );

  return (
    <div className="container">
      <h1>🤪🤤🍻 Бухарик Валера</h1>
      
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
              onClick={() => onValeraSelect(valera.id)}
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