export const fetchWrapper = {
    get,
    post,
    // Add more methods as needed
};

async function get<T>(url: string): Promise<T> {
    const response = await fetch(url);
    return handleResponse<T>(response);
}

async function post<T>(url: string, body: any): Promise<T> {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    };
    const response = await fetch(url, requestOptions);
    return handleResponse<T>(response);
}

async function handleResponse<T>(response: Response): Promise<T> {
    const data = await response.json();
    if (!response.ok) {
        const error = (data && data.message) || response.statusText;
        return Promise.reject(error);
    }
    return data;
}