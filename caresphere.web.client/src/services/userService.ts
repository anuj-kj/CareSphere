import { httpClient } from './httpClient';
import { User } from '../types';

export const userService = {
    getUser,
    createUser,
    // Add more user-specific methods as needed
};

async function getUser(userId: string): Promise<User> {
    return await httpClient.get<User>(`/users/${userId}`);
}

async function createUser(userData: any): Promise<User> {
    return await httpClient.post<User>('/users', userData);
}