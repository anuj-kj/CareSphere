export type ExampleType = {
  id: number;
  name: string;
  description?: string;
};

export interface User {
  id: string;
  username: string;
  email: string;
}

export type ApiResponse<T> = {
  data: T;
  error?: string;
};