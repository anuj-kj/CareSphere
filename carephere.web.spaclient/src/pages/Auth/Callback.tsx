import  { useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';

const Callback = () => {
    const navigate = useNavigate();
    const isProcessing = useRef(true);

    useEffect(() => {
        if (isProcessing.current) {
            const urlParams = new URLSearchParams(window.location.search);
            const token = urlParams.get('token');
            console.log('Token from URL:', token); // Log the token
            if (token) {
                localStorage.setItem('token', token);
                console.log('Token set in localStorage:', localStorage.getItem('token')); // Log the token from localStorage
                navigate('/organizations', { replace: true }); // Use replace to avoid going back to the callback page
                console.log('Navigating to /organizations'); // Log the navigation step
            } else {
                navigate('/login', { replace: true }); // Use replace to avoid going back to the callback page
                console.log('Navigating to /login'); // Log the navigation step
            }
            isProcessing.current = false; // Set processing to false to prevent re-execution
        }
    }, [navigate]);

    return <div>Loading...</div>;
};

export default Callback;