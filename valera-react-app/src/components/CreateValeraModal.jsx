import React, { useState } from 'react';

const CreateValeraModal = ({ isOpen, onClose, onCreate }) => {
  const [formData, setFormData] = useState({
    health: 100,
    alcohol: 0,
    joy: 0,
    fatigue: 0,
    money: 0
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    onCreate(formData);
    setFormData({
      health: 100,
      alcohol: 0,
      joy: 0,
      fatigue: 0,
      money: 0
    });
  };

  const handleChange = (field, value) => {
    setFormData(prev => ({
      ...prev,
      [field]: parseInt(value) || 0
    }));
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>Создать новую Валеру</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>❤️ Здоровье:</label>
            <input
              type="number"
              min="0"
              max="100"
              value={formData.health}
              onChange={(e) => handleChange('health', e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>🍺 Алкоголь:</label>
            <input
              type="number"
              min="0"
              max="100"
              value={formData.alcohol}
              onChange={(e) => handleChange('alcohol', e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>😊 Жизнерадостность:</label>
            <input
              type="number"
              min="-10"
              max="10"
              value={formData.joy}
              onChange={(e) => handleChange('joy', e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>😴 Усталость:</label>
            <input
              type="number"
              min="0"
              max="100"
              value={formData.fatigue}
              onChange={(e) => handleChange('fatigue', e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>💰 Деньги:</label>
            <input
              type="number"
              min="0"
              step="0.01"
              value={formData.money}
              onChange={(e) => handleChange('money', e.target.value)}
            />
          </div>

          <div className="modal-actions">
            <button type="button" onClick={onClose} className="btn-secondary">
              Отмена
            </button>
            <button type="submit" className="btn-primary">
              Создать Валеру
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateValeraModal;