const isDevelopment = import.meta.env.MODE === "dev";
console.log("isDevelopment", isDevelopment);

const URL = isDevelopment ? import.meta.env.VITE_DEV_API_BASE_URL : "http://localhost:8080/api";

console.log("URL", URL);

export const API_URL = URL;