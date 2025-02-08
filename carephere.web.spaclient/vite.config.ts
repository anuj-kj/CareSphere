import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

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
        root: ".", // Ensure Vite starts from the project root
        build: {
            outDir: "dist",  // Ensure build output is placed in `dist/`
            emptyOutDir: true,  // Clears the output directory before building
        },
    };
});


