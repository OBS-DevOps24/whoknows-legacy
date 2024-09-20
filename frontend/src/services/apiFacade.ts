import { PageType, RegisterFormData } from "../interfaces/types";
import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";

// DEFINE ENDPOINTS
const SEARCH_URL = API_URL + "/search";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q) return fetch(SEARCH_URL).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`).then(handleHttpErrors);
}

async function registerUser(username: string, email: string, password: string, password2: string): Promise<RegisterFormData> {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ username, email, password, password2 }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || 'Registration failed');
  }

  return response.json();
}

export { getSearchResults, registerUser };

