import { httpClient } from './httpClient';
import { Product } from '../types';

export const productService = {
    getProduct,
    createProduct,
    // Add more product-specific methods as needed
};

async function getProduct(productId: string): Promise<Product> {
    return await httpClient.get<Product>(`/products/${productId}`);
}

async function createProduct(productData: any): Promise<Product> {
    return await httpClient.post<Product>('/products', productData);
}