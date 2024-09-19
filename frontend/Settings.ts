const isProduction = import.meta.env.MODE === "prod";

let URL = isProduction
  ? import.meta.env.VITE_PROD_API_BASE_URL
  : import.meta.env.VITE_DEV_API_BASE_URL;

const MODE = isProduction ? "prod" : "dev";
console.info(MODE + " API URL: " + URL);
// console.info("ENV", import.meta.env);

if (!URL) {
  URL = "/api";
}

export const API_URL = URL;
