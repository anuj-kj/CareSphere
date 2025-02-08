import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    return {
        base: env.VITE_BASE_PATH || "/app/",  // ✅ Ensure `/app/` is used
        plugins: [react()],
        server: {
            port: 53590,
        },
        define: {
            _GOOGLE_CLIENT_ID: JSON.stringify(env.VITE_GOOGLE_CLIENT_ID),
            __APP_ENV__: JSON.stringify(env.VITE_APP_ENV),
            VITE_APP_URI: JSON.stringify(env.VITE_APP_URI),
        },
        build: {
            outDir: "dist",
            emptyOutDir: true,
            minify: "esbuild",
            sourcemap: false,
        },
        envDir: path.resolve(__dirname),  // ✅ Ensure environment variables are loaded from `.env.production`
    };
});
