import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    return {
        base: env.VITE_BASE_PATH || "/app/",  // ✅ Ensure React uses `/app/`
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
            outDir: "dist",  // ✅ Ensure build output is placed in `dist/`
            emptyOutDir: true,  // ✅ Clears the output directory before building
            minify: "esbuild",  // ✅ Enables minification
            sourcemap: false,  // ✅ Disables source maps for production
            rollupOptions: {
                input: path.resolve(__dirname, "index.html"),  // ✅ Fix: Ensure Vite finds `index.html`
            },
        },
    };
});
