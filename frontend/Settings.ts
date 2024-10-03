const isDevelopment = import.meta.env.VITE_MODE === "dev";

const URL = isDevelopment ? import.meta.env.VITE_DEV_API_BASE_URL : "/api";

if (isDevelopment) {
  console.log("URL", URL);
  console.log("isDevelopment", isDevelopment);
}

export const API_URL = URL;
