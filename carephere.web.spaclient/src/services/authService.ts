
import { JwtModel } from '../models/auth/JwtModel';

import { getData, postData } from '../api/apiService';
import { RegisterModel } from '../models/auth/RegisterModel';

export const credentialLogin = async (username: string, password: string): Promise<JwtModel> => {
    const response = await postData('/auth/login', { username, password });
     return response.data;
    
};
export const register = async (registerModel: RegisterModel): Promise<JwtModel> => {
    const response =  await postData('/auth/register', registerModel);
    return response.data;
};
export const googleLogin = async () => {
    const response = await getData('/auth/google-login');
    console.log(response.data);
    return response.data;
};
export const verifyGoogleToken = async (credential: string) => {
    const response = await  await postData('/auth/verify-google-token', { credential }); 
    console.log(response.data);
    return response.data;
};



export const signOut = () => {
    localStorage.removeItem('token');
};