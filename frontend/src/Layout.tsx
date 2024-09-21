import Header from "./Header";

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
      <div className="py-4 bg-zinc-200">FOOTER</div>
    </main>
  );
}
