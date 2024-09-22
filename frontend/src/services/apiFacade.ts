import { PageType, WeatherType } from "../interfaces/types";
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

export { getSearchResults, getWeatherResults };
