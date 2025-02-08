/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_GOOGLE_CLIENT_ID: string;
    readonly VITE_APP_URI: string;
    readonly VITE_APP_ENV: "development" | "production" | "staging";
  }
  
  interface ImportMeta {
    readonly env: ImportMetaEnv;
  }
  