import { useSearchParams } from "react-router-dom";
import Search from "./Search";
import SearchResults from "./SearchResults";
import { PageType } from "../../interfaces/types";

export default function SearchPage() {
  return (
    <div>
      <Search />
      <SearchResults />
    </div>
  );
}
