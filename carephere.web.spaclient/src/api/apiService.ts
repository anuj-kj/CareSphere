import axiosInstance from './axiosInstance';

export const getData = async (url: string) => {
    return await axiosInstance.get(url);
};

export const postData = async (url: string, data: any) => {
    return await axiosInstance.post(url, data);
};

export default axiosInstance;