import  { JSX } from 'react';
import { Navigate } from 'react-router-dom';
import { isTokenExpired } from '../../utils/tokenUtils';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
    const token = localStorage.getItem('token');

    if (!token || isTokenExpired(token)) {
        localStorage.removeItem('token');
        return <Navigate to="/login" replace />;
    }

    return children;
};

export default PrivateRoute;