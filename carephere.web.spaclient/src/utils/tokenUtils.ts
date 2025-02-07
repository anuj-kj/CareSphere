import { User } from '../models/User';
import {jwtDecode} from 'jwt-decode';

export const isTokenExpired = (token: string | null): boolean => {
    if (!token) return true;
    const { exp } = JSON.parse(atob(token.split('.')[1]));
    return Date.now() >= exp * 1000;
};

export const getTokenClaims = (): User | null => {
    const token = localStorage.getItem('token');
    if (!token || isTokenExpired(token)) {
        return null;
    }

    const claims: any = jwtDecode(token);
    return {
        name: claims[ClaimTypes.Name],
        email: claims[ClaimTypes.Email],
        userId: claims[ClaimTypes.NameIdentifier],
        role: claims[ClaimTypes.Role],
        username: claims.username,
    };
};

// Define the claim types and JWT registered claim names
export const ClaimTypes = {
    Name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
    Email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    NameIdentifier: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    Role: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
};

export const JwtRegisteredClaimNames = {
    Sub: 'sub',
    Jti: 'jti',
};