

type LayoutProps = {
  children: React.ReactNode;
};
export default function Layout({ children }: LayoutProps) {
  return (
    <main className="w-full min-h-screen bg-background text-text">
      <div className="mt-10 mx-2">
        {children}
      </div>
    </main>
  );
}
