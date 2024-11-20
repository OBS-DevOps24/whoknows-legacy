import Header from "./Header";
import Footer from "./Footer";

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
        <SecurityBreachBanner />
        <Footer />
      </div>
    </main>
  );
}

function SecurityBreachBanner() {
  return (
    <div className="relative px-4 py-3 font-bold text-center text-red-700 bg-yellow-100 border border-yellow-400 rounded">
      ⚠️ <strong>Security Alert:</strong> We have detected a potential security
      breach. Please update your password immediately to secure your account.
    </div>
  );
}
