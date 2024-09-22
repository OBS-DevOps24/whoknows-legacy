import { useEffect, useState } from "react";
import { WeatherType } from "../../interfaces/types";
import { getWeatherResults } from "../../services/apiFacade";

export default function WeatherComponent() {
  const [weather, setWeather] = useState<WeatherType | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  // // const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    getWeatherResults().then(data => {
      setLoading(false);
      setWeather(data);
    });
  }, []);

  return (
    <>
      {loading ? (
        <Skeleton />
      ) : (
        <div className="max-w-md mx-auto mt-10 overflow-hidden bg-white shadow-2xl rounded-2xl">
          <div className="p-6 bg-blue-500">
            <h2 className="text-3xl font-bold text-center text-white">
              Weather in {weather?.city}, {weather?.country}
            </h2>
          </div>
          <div className="p-6">
            <p className="mb-4 text-2xl text-gray-800">
              <span className="font-semibold">Temperature:</span>{" "}
              {weather?.temperatureValue} {weather?.temperatureUnit}
            </p>
            <p className="text-2xl text-gray-800">
              <span className="font-semibold">Wind Speed:</span>{" "}
              {weather?.windSpeedValue} {weather?.windSpeedUnit}
            </p>
          </div>
          <div className="p-4 text-center bg-gray-100">
            <p className="text-[12px] text-gray-600">
              Updated at: {new Date().toLocaleTimeString()}
            </p>
          </div>
        </div>
      )}
      ;
    </>
  );
}

const Skeleton = () => {
  return (
    <div className="max-w-md mx-auto mt-10 overflow-hidden bg-white shadow-2xl rounded-2xl">
      <div className="p-6 bg-blue-500">
        <h2 className="w-3/4 h-8 mx-auto bg-gray-300 rounded animate-pulse"></h2>
      </div>
      <div className="p-6">
        <p className="h-6 mb-4 bg-gray-300 rounded animate-pulse"></p>
        <p className="h-6 bg-gray-300 rounded animate-pulse"></p>
      </div>
      <div className="p-4 text-center bg-gray-100">
        <p className="w-1/4 h-4 mx-auto bg-gray-300 rounded animate-pulse"></p>
      </div>
    </div>
  );
};
