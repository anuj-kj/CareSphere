import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

// Load environment variables based on the mode
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd());

    return {
        plugins: [react()],
        server: {
            port: 53590, // Local development port
        },
        define: {
            // Expose environment variables for use in code
            _GOOGLE_CLIENT_ID: JSON.stringify(env.VITE_GOOGLE_CLIENT_ID),
            __APP_ENV__: JSON.stringify(env.VITE_APP_ENV),
            VITE_APP_URI: JSON.stringify(env.VITE_APP_URI),
        },
    };
});

