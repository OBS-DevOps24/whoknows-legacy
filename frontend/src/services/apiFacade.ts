import { PageType } from "../interfaces/types";
// import { API_URL } from "../../Settings";
import { handleHttpErrors } from "./fetchUtils";

// DEFINE ENDPOINTS
const SEARCH_URL = "/search";

async function getSearchResults(q: string): Promise<PageType[]> {
  if (!q) return fetch(SEARCH_URL).then(handleHttpErrors);
  return fetch(`${SEARCH_URL}?q=${q}`).then(handleHttpErrors);
}

export { getSearchResults };
