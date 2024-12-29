import { fetchWrapper } from '../utils/fetchWrapper';
import config from '../configs/config';

const { API_BASE_URL } = config;

export const httpClient = {
    get,
    post,
    // Add more methods as needed
};

async function get<T>(endpoint: string): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    return await fetchWrapper.get<T>(url);
}

async function post<T>(endpoint: string, data: any): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    return await fetchWrapper.post<T>(url, data);
}