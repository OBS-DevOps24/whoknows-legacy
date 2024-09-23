import { PageType, WeatherType, RegisterFormData, LoginFormData } from "../interfaces/types";
import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";

// DEFINE ENDPOINTS
const SEARCH_URL = API_URL + "/search";
const WEATHER_URL = API_URL + "/weather";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q) return fetch(SEARCH_URL).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`).then(handleHttpErrors);
}

async function getWeatherResults(
  city?: string,
  country?: string
): Promise<WeatherType> {
  if (city && country) {
    return fetch(
      `http://52.164.251.63${WEATHER_URL}?city=${city}&country=${country}`
    ).then(handleHttpErrors);
  }
  return fetch(`http://52.164.251.63${WEATHER_URL}`).then(handleHttpErrors);
}

async function registerUser(username: string, email: string, password: string, password2: string): Promise<RegisterFormData> {
  return fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, email, password, password2 }),
  }).then(handleHttpErrors);
}

async function loginUser(username: string, password: string): Promise<LoginFormData> {
  return fetch(`${API_URL}/login`, {
    method: "POST",
    credentials: 'include',
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password }),
  }).then(handleHttpErrors);
}

async function logoutUser(): Promise<void> {
  return fetch(`${API_URL}/logout`, {
    method: 'GET',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
  }).then(handleHttpErrors);
}

async function isUserLoggedIn(): Promise<boolean> {
  return fetch(`${API_URL}/is-logged-in`, {
    method: 'GET',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
  })
  .then(handleHttpErrors)
  .then(data => data.isLoggedIn);
}

export { getSearchResults, getWeatherResults, registerUser, loginUser, logoutUser, isUserLoggedIn };





