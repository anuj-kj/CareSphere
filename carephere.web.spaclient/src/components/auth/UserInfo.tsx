import { useNavigate } from 'react-router-dom';
import { signOut } from '../../services/authService';
import { getTokenClaims } from '../../utils/tokenUtils';

const UserInfo = () => {
    const navigate = useNavigate();
    const user = getTokenClaims();

    const handleSignOut = () => {
        signOut();
        navigate('/login');
    };

    if (!user) {
        return null;
    }

    return (
        <div className="d-flex align-items-center">
            <span className="me-3">Hello, {user.name}</span>
            <button className="btn btn-outline-secondary" onClick={handleSignOut}>Sign Out</button>
        </div>
    );
};

export default UserInfo;