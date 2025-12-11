import React, { useState } from 'react';
import ValeraList from './components/ValeraList';
import ValeraStats from './components/ValeraStats';
import './App.css';

function App() {// Глобальное состояние приложения
  const [currentView, setCurrentView] = useState('list');
  const [selectedValeraId, setSelectedValeraId] = useState(null);

  const handleValeraSelect = (valeraId) => {// Обработчик выбора Валеры
    setSelectedValeraId(valeraId);
    setCurrentView('stats');// Меняем вид на детали
  };

  const handleBackToList = () => {// Обработчик возврата к списку
    setCurrentView('list');
    setSelectedValeraId(null);
  };

  return (
    <div className="App">
      {currentView === 'list' && (
        <ValeraList onValeraSelect={handleValeraSelect} />
      )}
      {currentView === 'stats' && (
        <ValeraStats 
          valeraId={selectedValeraId} 
          onBack={handleBackToList}
        />
      )}
    </div>
  );
}

export default App;