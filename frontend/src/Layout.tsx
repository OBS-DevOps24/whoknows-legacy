import Header from "./Header";
import Footer from "./Footer";
import { Link } from "react-router-dom";

type LayoutProps = {
  children: React.ReactNode;
};

export default function Layout({ children }: LayoutProps) {
  return (
    <main className="w-full min-h-screen bg-background text-text grid grid-rows-[auto,1fr,auto]">
      <Header />
      <div className="w-full">
        <div className="mt-10 max-w-[1200px] mx-auto">{children}</div>
      </div>
      <div>
        <Footer />
      </div>
    </main>
  );
}
