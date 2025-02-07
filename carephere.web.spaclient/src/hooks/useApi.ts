import { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance';

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE';

const useApi = <T>(endpoint: string, method: HttpMethod = 'GET', body?: any) => {
    const [data, setData] = useState<T | null>(null);
    const [error, setError] = useState<any>(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                let result;
                switch (method) {
                    case 'GET':
                        result = await axiosInstance.get(endpoint);
                        break;
                    case 'POST':
                        result = await axiosInstance.post(endpoint, body);
                        break;
                    case 'PUT':
                        result = await axiosInstance.put(endpoint, body);
                        break;
                    case 'DELETE':
                        result = await axiosInstance.delete(endpoint);
                        break;
                    default:
                        throw new Error('Unsupported HTTP method');
                }
                setData(result.data);
            } catch (error) {
                setError(error);
            }
        };

        fetchData();
    }, [endpoint, method, body]);

    return { data, error };
};

export default useApi;