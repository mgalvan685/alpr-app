export function Sidebar() {
  return (
    <div className="w-64 bg-card text-card-foreground h-screen p-4">
      <h1 className="text-xl font-bold mb-6">Galvan ALPR</h1>

      <nav className="space-y-3">
        <a href="/" className="block hover:text-accent">Dashboard</a>
        <a href="/videos" className="block hover:text-accent">Videos</a>
        <a href="/plates" className="block hover:text-accent">Plates</a>
        <a href="/upload" className="block hover:text-accent">Upload Video</a>
      </nav>
    </div>
  );
}