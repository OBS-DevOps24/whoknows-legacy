import { PageType, RegisterFormData, LoginFormData } from "../interfaces/types";
import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";

// DEFINE ENDPOINTS
const SEARCH_URL = API_URL + "/search";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q) return fetch(SEARCH_URL).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`).then(handleHttpErrors);
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
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password }),
  }).then(handleHttpErrors);
}


export { getSearchResults, registerUser, loginUser };


