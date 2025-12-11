import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { valeraApi, ACTIONS, ACTION_LABELS } from '../services/valeraApi';

const ValeraStats = ({ user }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  
  const [valera, setValera] = useState(null);
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState(false);
  const [error, setError] = useState('');

  const loadValera = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await valeraApi.getValeraById(id);
      setValera(data);
    } catch (err) {
      setError('Ошибка загрузки Валеры: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) {
      loadValera();
    }
  }, [id]);

  const handleBack = () => {
    navigate('/valeras');
  };

  const handleAction = async (action) => {
    console.log('🚀 Executing action:', action, 'for valera ID:', id);
    
    try {
      setActionLoading(true);
      setError('');
      const updatedValera = await valeraApi.executeAction(id, action);
      console.log('✅ Action successful:', updatedValera);
      setValera(updatedValera);
    } catch (err) {
      console.error('❌ Action failed:', err);
      setError('Ошибка выполнения действия: ' + (err.response?.data || err.message));
    } finally {
      setActionLoading(false);
    }
  };

  // Валидация кнопок
  const canWork = valera && valera.fatigue < 10 && valera.alcohol < 50;
  const canDrinkWine = valera && valera.money >= 20;
  const canGoToBar = valera && valera.money >= 100;
  const canDrinkWithMarginals = valera && valera.money >= 150;

  const ProgressBar = ({ value, max, color, label }) => (
    <div className="progress-item">
      <div className="progress-label">{label}: {value}/{max}</div>
      <div className="progress-bar">
        <div 
          className="progress-fill"
          style={{ 
            width: `${(value / max) * 100}%`,
            backgroundColor: color
          }}
        ></div>
      </div>
    </div>
  );

  if (loading) return <div className="container loading">Загрузка Валеры...</div>;
  if (!valera) return <div className="container error">Валера не найдена</div>;

  return (
    <div className="container">
      <button onClick={handleBack} className="btn-back">← Назад к списку</button>
      
      <h1>Управление Валера #{valera.id}</h1>
      
      {error && <div className="error">{error}</div>}

      <div className="stats-section">
        <h2>📊 Состояние Валеры</h2>
        <div className="stats-grid">
          <ProgressBar value={valera.health} max={100} color="#ef4444" label="❤️ Здоровье" />
          <ProgressBar value={valera.alcohol} max={100} color="#f59e0b" label="🍺 Алкоголь" />
          <ProgressBar value={valera.joy + 10} max={20} color="#10b981" label="😊 Жизнерадостность" />
          <ProgressBar value={valera.fatigue} max={100} color="#6366f1" label="😴 Усталость" />
          <div className="money-display">
            <span className="money-label">💰 Деньги:</span>
            <span className="money-value">{valera.money} ₽</span>
          </div>
        </div>
      </div>

      <div className="actions-section">
        <h2>🎮 Действия</h2>
        <div className="actions-grid">
          <button 
            onClick={() => handleAction(ACTIONS.WORK)}
            disabled={!canWork || actionLoading}
            className={!canWork ? 'btn-disabled' : 'btn-action'}
            title={!canWork ? 'Нельзя работать: усталость ≥ 10 или алкоголь ≥ 50' : ''}
          >
            💼 {ACTION_LABELS[ACTIONS.WORK]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.CONTEMPLATE_NATURE)}
            disabled={actionLoading}
            className="btn-action"
          >
            🌳 {ACTION_LABELS[ACTIONS.CONTEMPLATE_NATURE]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.DRINK_WINE)}
            disabled={!canDrinkWine || actionLoading}
            className={!canDrinkWine ? 'btn-disabled' : 'btn-action'}
            title={!canDrinkWine ? 'Нужно 20 ₽' : ''}
          >
            🍷 {ACTION_LABELS[ACTIONS.DRINK_WINE]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.GO_TO_BAR)}
            disabled={!canGoToBar || actionLoading}
            className={!canGoToBar ? 'btn-disabled' : 'btn-action'}
            title={!canGoToBar ? 'Нужно 100 ₽' : ''}
          >
            🍻 {ACTION_LABELS[ACTIONS.GO_TO_BAR]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.DRINK_WITH_MARGINALS)}
            disabled={!canDrinkWithMarginals || actionLoading}
            className={!canDrinkWithMarginals ? 'btn-disabled' : 'btn-action'}
            title={!canDrinkWithMarginals ? 'Нужно 150 ₽' : ''}
          >
            🥴 {ACTION_LABELS[ACTIONS.DRINK_WITH_MARGINALS]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.SING_IN_METRO)}
            disabled={actionLoading}
            className="btn-action"
          >
            🎤 {ACTION_LABELS[ACTIONS.SING_IN_METRO]}
          </button>

          <button 
            onClick={() => handleAction(ACTIONS.SLEEP)}
            disabled={actionLoading}
            className="btn-action"
          >
            😴 {ACTION_LABELS[ACTIONS.SLEEP]}
          </button>
        </div>
        
        {actionLoading && <div className="loading">Выполнение действия...</div>}
      </div>
    </div>
  );
};

export default ValeraStats;