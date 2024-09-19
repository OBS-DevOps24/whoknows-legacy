import { useSearchParams } from "react-router-dom";
import { PageType } from "../../interfaces/types";
import { useEffect, useState } from "react";

// Type for search result item
export default function SearchResults() {
  const [searchParams] = useSearchParams();
  const [results, setResults] = useState<PageType[]>([]);

  useEffect(() => {
    // FETCH DATA FROM API
    if (!searchParams.get("q")) {
      setResults([]);
      return;
    }
    fetch(`http://localhost:5243/api/search?q=${searchParams.get("q")}`)
      .then(response => response.json())
      .then(data => {
        setResults(data);
      });
  }, [searchParams]);

  return (
    <div>
      {searchParams.get("q") && (
        <p className="text-[14px] mt-4">
          Showing results for{" "}
          <span className="font-bold">{searchParams.get("q")}</span>
        </p>
      )}
      {results.map(result => (
        <div
          key={result.title}
          className="p-4 mt-6 hover:bg-zinc-100 rounded-xl hover:cursor-pointer"
        >
          <h2 className="text-xl">
            <a
              href={result.url}
              className="font-medium text-blue-900/80 hover:text-blue-900"
            >
              {result.title}
            </a>
          </h2>
          <p className="truncate text-[14px] opacity-60">{result.content}</p>
          <a className="text-green-900 opacity-80" href={result.url}>
            {result.url}
          </a>
        </div>
      ))}
    </div>
  );
}
