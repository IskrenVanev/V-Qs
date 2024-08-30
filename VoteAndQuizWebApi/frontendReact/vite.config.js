import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 7056, // Set Vite to run on this port
        proxy: {
            '/api': {
                target: 'http://localhost:7055', // Update this to the new port of your backend
                changeOrigin: true,
                secure: false,
                rewrite: (path) => path.replace(/^\/api/, ''), // Adjust if needed
            },
        },
    },
});