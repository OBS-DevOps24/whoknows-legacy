import {
  PageType,
  WeatherType,
  RegisterFormData,
  LoginFormData,
} from "../interfaces/types";
import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";
import axios from "axios";

// DEFINE ENDPOINTS
const SEARCH_URL = API_URL + "/search";
const WEATHER_URL = API_URL + "/weather";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q) return fetch(SEARCH_URL, { credentials: 'include' }).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`, { credentials: 'include' }).then(handleHttpErrors);
}

async function getWeatherResults(
  city?: string,
  country?: string
): Promise<WeatherType> {
  if (city && country) {
    return fetch(`${WEATHER_URL}?city=${city}&country=${country}`, { credentials: 'include' }).then(
      handleHttpErrors
    );
  }
  return fetch(`${WEATHER_URL}`, { credentials: 'include' }).then(handleHttpErrors);
}

async function registerUser(
  username: string,
  email: string,
  password: string,
  password2: string
): Promise<RegisterFormData> {
  return fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, email, password, password2 }),
    credentials: 'include',
  }).then(handleHttpErrors);
}

const loginUser = async (loginData: LoginFormData): Promise<void> => {
  await axios.post(`${API_URL}/login`, loginData, { withCredentials: true });
};

const logoutUser = async (): Promise<void> => {
  await axios.get(`${API_URL}/logout`, { withCredentials: true });
};

const checkAuthStatusApi = async (): Promise<boolean> => {
  try {
    const response = await axios.get(`${API_URL}/is-logged-in`, { withCredentials: true });
    return response.data.isLoggedIn;
  } catch {
    return false;
  }
};

export {
  getSearchResults,
  getWeatherResults,
  registerUser,
  loginUser,
  logoutUser,
  checkAuthStatusApi,
};
