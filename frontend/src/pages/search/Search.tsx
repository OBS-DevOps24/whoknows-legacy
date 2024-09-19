import { MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { useState } from "react";
import { useSearchParams } from "react-router-dom";

export default function Search() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchTerm, setSearchTerm] = useState(searchParams.get("q") || "");

  // Function to update search params when the button is clicked
  const handleSearch = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSearchParams({ q: searchTerm });
  };

  return (
    <div>
      <form
        onSubmit={handleSearch}
        className="relative flex flex-1 flex-shrink-0"
      >
        <label htmlFor="search" className="sr-only">
          Search
        </label>
        <input
          className="peer block w-full rounded-md border border-gray-200 py-[9px] pl-10 text-sm outline-2 placeholder:text-gray-500"
          placeholder={"Search..."}
          value={searchTerm}
          onChange={e => setSearchTerm(e.target.value)}
        />
        <MagnifyingGlassIcon className="absolute left-3 top-1/2 h-[18px] w-[18px] -translate-y-1/2 text-gray-500 peer-focus:text-gray-900" />
        <button className="px-4 py-2 ml-2 text-white bg-blue-500 rounded-md hover:bg-blue-600">
          Search
        </button>
      </form>
    </div>
  );
}
