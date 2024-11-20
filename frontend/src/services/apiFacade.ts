import {
  PageType,
  WeatherType,
  RegisterFormData,
  LoginFormData,
  ChangePasswordFormData,
} from "../interfaces/types";
import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";
import axios from "axios";

// DEFINE ENDPOINTS
const SEARCH_URL = API_URL + "/search";
const WEATHER_URL = API_URL + "/weather";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q)
    return fetch(SEARCH_URL, { credentials: "include" }).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`, { credentials: "include" }).then(
    handleHttpErrors
  );
}

async function getWeatherResults(
  city?: string,
  country?: string
): Promise<WeatherType> {
  if (city && country) {
    return fetch(`${WEATHER_URL}?city=${city}&country=${country}`, {
      credentials: "include",
    }).then(handleHttpErrors);
  }
  return fetch(`${WEATHER_URL}`, { credentials: "include" }).then(
    handleHttpErrors
  );
}

const registerUser = async (registerData: RegisterFormData): Promise<void> => {
  try {
    await axios.post(`${API_URL}/register`, registerData, {
      withCredentials: true,
    });
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error("An unexpected error occurred");
  }
};

const loginUser = async (loginData: LoginFormData): Promise<void> => {
  try {
    await axios.post(`${API_URL}/login`, loginData, { withCredentials: true });
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error("An unexpected error occurred");
  }
};

const logoutUser = async (): Promise<void> => {
  await axios.get(`${API_URL}/logout`, { withCredentials: true });
};

const checkAuthStatusApi = async (): Promise<boolean> => {
  try {
    const response = await axios.get(`${API_URL}/is-logged-in`, {
      withCredentials: true,
    });
    return response.data.isLoggedIn;
  } catch {
    return false;
  }
};

const checkPasswordStatusApi = async (): Promise<boolean> => {
  try {
    const response = await axios.get(`${API_URL}/is-logged-in`, {
      withCredentials: true,
    });
    return response.data.expiredPassword;
  } catch {
    return false;
  }
};

const changePasswordApi = async (
  changePasswordData: ChangePasswordFormData
): Promise<void> => {
  try {
    await axios.put(`${API_URL}/auth`, changePasswordData, {
      withCredentials: true,
    });
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error("An unexpected error occurred");
  }
};

export {
  getSearchResults,
  getWeatherResults,
  registerUser,
  loginUser,
  logoutUser,
  checkAuthStatusApi,
  checkPasswordStatusApi,
  changePasswordApi,
};
