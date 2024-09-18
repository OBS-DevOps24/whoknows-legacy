import { useSearchParams } from "react-router-dom";
import Search from "./Search";

export default function SearchPage() {
  const [searchParams] = useSearchParams();

  return (
    <div>
      <h1>Search</h1>
      <Search />
      <div className="my-2">
        {searchParams.get("q") && (
          <p className="text-[14px]">
            Showing results for{" "}
            <span className="font-bold">{searchParams.get("q")}</span>
          </p>
        )}
      </div>
    </div>
  );
}
