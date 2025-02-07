import React from 'react';
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import { verifyGoogleToken } from '../../services/authService';
import { useNavigate } from 'react-router-dom';

const GoogleLoginButton: React.FC = () => {
  const googleClientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;
  const navigate = useNavigate();

  if (!googleClientId) {
    console.error('Google Client ID is not defined');
    return null;
  }

  const handleGoogleLogin = async (response: any) => {
    console.log('Google login successful:', response);
    try {
      const token = response.credential;
      const jwtModel = await verifyGoogleToken(token);
      localStorage.setItem('token', jwtModel.token);
     navigate('/organizations');
    } catch (error) {
      console.error('Error verifying Google token:', error);
    }
  };

  const handleGoogleLoginFailure = () => {
    console.log('Google login failed');
  };

  return (
    <GoogleOAuthProvider clientId={googleClientId}>
      <GoogleLogin
        onSuccess={handleGoogleLogin}
        onError={handleGoogleLoginFailure}
      />
    </GoogleOAuthProvider>
  );
};

export default GoogleLoginButton;