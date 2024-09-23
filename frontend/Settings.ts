const isDevelopment = import.meta.env.DEV;
console.log("isDevelopment", isDevelopment);

const URL = isDevelopment ? import.meta.env.VITE_DEV_API_BASE_URL : import.meta.env.VITE_PROD_API_BASE_URL;

console.log("URL", URL);

export const API_URL = URL;