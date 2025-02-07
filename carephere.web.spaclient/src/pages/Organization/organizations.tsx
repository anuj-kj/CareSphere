import  { useEffect, useState } from 'react';
import '@fortawesome/fontawesome-free/css/all.min.css';
import { getOrganizations } from '../../services/organizationService';
import { Organization } from '../../models/Organization ';

const OrganizationList = () => {
    const [organizations, setOrganizations] = useState<Organization[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<any>(null);

    useEffect(() => {
        const fetchOrganizations = () => {
            setLoading(true);
            getOrganizations()
            .then(data => {
                setOrganizations(data);
            })
            .catch(error => {
                setError(error);
            })
            .finally(() => {
                setLoading(false);
            });
        };

        fetchOrganizations();
    }, []);

    const handleButtonClick = (organizationId: number) => {
        console.log(`Button clicked for organizationId: ${organizationId}`);
        // Add your logic here
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error.message}</div>;

    const defaultImage = 'images/defaultOrg.jpeg'; // Replace with the correct path to your default image

    return (
        <div className="container py-5">
            <h1>Organizations</h1>
            <ul className="list-group">
                {organizations.map(org => (
                    <li key={org.organizationId} className="list-group-item d-flex justify-content-between align-items-center">
                        <div className="d-flex align-items-center">
                            <img
                                src={org.imageUrl || defaultImage}
                                alt={org.name}
                                className="img-thumbnail"
                                style={{ width: '100px', height: '100px', marginRight: '20px' }}
                            />
                            <div>
                            <h5>{org.name}</h5>
                                <p><i className="fas fa-map-marker-alt"></i> {org.address}</p>
                                <p><i className="fas fa-phone"></i> {org.phoneNumber}</p>
                                <p><i className="fas fa-envelope"></i> {org.email}</p>
                            </div>
                        </div>
                        <button
                            className="btn btn-primary"
                            onClick={() => handleButtonClick(org.organizationId)}
                        >
                            Call Function
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OrganizationList;