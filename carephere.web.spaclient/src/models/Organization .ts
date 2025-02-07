export interface Organization {
    organizationId: number;
    name: string;
    address: string | null;
    phoneNumber: string;
    email: string;
    createdAt: Date;
    imageUrl: string | null;
}