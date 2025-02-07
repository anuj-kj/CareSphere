import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { credentialLogin } from '../../services/authService';
import GoogleLoginButton from '../../components/auth/GoogleLoginButton';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [usernameError, setUsernameError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        let valid = true;

        if (!username) {
            setUsernameError('Username is required');
            valid = false;
        } else {
            setUsernameError('');
        }

        if (!password) {
            setPasswordError('Password is required');
            valid = false;
        } else {
            setPasswordError('');
        }

        if (!valid) {
            return;
        }

        try {
            const jwtModel = await credentialLogin(username, password);
            localStorage.setItem('token', jwtModel.token);
            navigate('/organizations');
        } catch (err) {
            console.log(err);
            setError('Invalid username or password');
        }
    };

   

    return (
        <div className="container py-5">
            <div className="row justify-content-center">
                <div className="col-md-6 mb-3">
                    <h1>Login</h1>
                    <GoogleLoginButton />
                    <form onSubmit={handleLogin}>
                        <div className="mb-3">
                            <label htmlFor="username" className="form-label">Username</label>
                            <input
                                type="text"
                                className="form-control"
                                id="username"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                            />
                            {usernameError && <div className="text-danger">{usernameError}</div>}
                        </div>
                        <div className="mb-3">
                            <label htmlFor="password" className="form-label">Password</label>
                            <input
                                type="password"
                                className="form-control"
                                id="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                            {passwordError && <div className="text-danger">{passwordError}</div>}
                        </div>
                        {error && <div className="alert alert-danger">{error}</div>}
                        <button type="submit" className="btn btn-primary">Login</button>
                    </form>
                    <div className="mt-3">
                        <span>Don't have an account? </span>
                        <a href="/register">Register</a>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Login;