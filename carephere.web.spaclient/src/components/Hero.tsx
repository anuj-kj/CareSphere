import { Link } from 'react-router-dom';

export default function Hero() {
    return (
        <div className="bg-light text-center py-5">
            <div className="container">
                <h1 className="display-4">Welcome to MyReactApp</h1>
                <p className="lead">
                    This is a simple, responsive template built with React, Vite, and Bootstrap.
                </p>
                <Link to="/organizations" className="btn btn-primary btn-lg">
                    Learn More
                </Link>
            </div>
        </div>
    );
}