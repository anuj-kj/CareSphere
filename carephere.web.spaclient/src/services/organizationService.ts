import { getData } from '../api/apiService';
import { Organization } from '../models/Organization ';

export const getOrganizations = async (): Promise<Organization[]> => {
    const response = await getData('/organization/all');
    return response.data;
};

