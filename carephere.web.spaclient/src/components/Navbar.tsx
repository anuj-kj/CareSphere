import { Link } from 'react-router-dom';
import UserInfo from './auth/UserInfo';

export default function Navbar() {
    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container">
                <Link to="/" className="navbar-brand">
                    NurtureNet
                </Link>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNav"
                    aria-controls="navbarNav"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ms-auto">
                        <li className="nav-item">
                            <Link to="/" className="nav-link">
                                Home
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/about" className="nav-link">
                                About
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/contact" className="nav-link">
                                Contact
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/contact-redux" className="nav-link">
                                Contact Redux
                            </Link>
                        </li>
                    </ul>
                    <UserInfo />
                </div>
            </div>
        </nav>
    );
}
