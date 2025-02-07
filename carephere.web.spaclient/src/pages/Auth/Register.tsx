import  { useState } from 'react';
import { useForm } from 'react-hook-form';
import { RegisterModel } from '../../models/auth/RegisterModel';
import { register as registerUser } from '../../services/authService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from '@fortawesome/free-solid-svg-icons';
import { faGoogle } from '@fortawesome/free-brands-svg-icons';

const Register = () => {
    const { register, handleSubmit, formState: { errors }, reset } = useForm<RegisterModel>();
    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const onSubmit = async (data: RegisterModel) => {
        setIsLoading(true);
        try {
            await registerUser(data);
            setSuccessMessage('Registration successful! You can now ');
            setErrorMessage('');
            reset(); // Clear the form
        } catch (error) {
            setErrorMessage('Registration failed. Please try again.');
            setSuccessMessage('');
        } finally {
            setIsLoading(false);
        }
    };

    const handleGoogleSignup = async () => {
        try {
            window.location.href = 'http://localhost:5158/api/auth/google-login';
        } catch (err) {
            console.error('Google signup failed', err);
        }
    };

    return (
        <div className="container py-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <h1>Register</h1>
                    {successMessage && (
                        <div className="alert alert-success">
                            {successMessage}
                            <a href="/login" className="alert-link">login</a>.
                        </div>
                    )}
                    {errorMessage && <div className="alert alert-danger">{errorMessage}</div>}
                    <button className="btn btn-danger mb-3" onClick={handleGoogleSignup}>
                        <FontAwesomeIcon icon={faGoogle} className="me-2" />
                        Signup with Google
                    </button>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className="mb-3">
                            <label htmlFor="name" className="form-label">Name</label>
                            <input
                                type="text"
                                className="form-control"
                                id="name"
                                {...register('name', { required: 'Name is required' })}
                            />
                            {errors.name && <div className="text-danger">{errors.name.message}</div>}
                        </div>
                        <div className="mb-3">
                            <label htmlFor="email" className="form-label">Email</label>
                            <input
                                type="email"
                                className="form-control"
                                id="email"
                                {...register('email', { required: 'Email is required' })}
                            />
                            {errors.email && <div className="text-danger">{errors.email.message}</div>}
                        </div>
                        <div className="mb-3">
                            <label htmlFor="password" className="form-label">Password</label>
                            <input
                                type="password"
                                className="form-control"
                                id="password"
                                {...register('password', { required: 'Password is required' })}
                            />
                            {errors.password && <div className="text-danger">{errors.password.message}</div>}
                        </div>
                        <button type="submit" className="btn btn-primary" disabled={isLoading}>
                            {isLoading ? (
                                <>
                                    <FontAwesomeIcon icon={faSpinner} spin /> Please wait
                                </>
                            ) : (
                                'Register'
                            )}
                        </button>
                    </form>
                    <div className="mt-3">
                        <span>Already have an account? </span>
                        <a href="/login">Login</a>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Register;