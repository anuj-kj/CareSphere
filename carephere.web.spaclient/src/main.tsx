import React from 'react';
import App from './App';
import './index.css'; // For global styles
import './App.css'; // For app-specific styles
import 'bootstrap/dist/css/bootstrap.min.css'; // Bootstrap styles
import { createRoot } from 'react-dom/client';

const container = document.getElementById('root');
if (container) {
  const root = createRoot(container); // Create a root.

  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
} else {
  console.error('Root container not found');
}