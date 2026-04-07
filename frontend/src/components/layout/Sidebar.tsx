export function Sidebar() {
  return (
    <div className="w-64 bg-gray-900 text-gray-100 h-screen p-4">
      <h1 className="text-xl font-bold mb-6">Galvan ALPR</h1>

      <nav className="space-y-3">
        <a href="/" className="block hover:text-white">Dashboard</a>
        <a href="/videos" className="block hover:text-white">Videos</a>
        <a href="/plates" className="block hover:text-white">Plates</a>
        <a href="/upload" className="block hover:text-white">Upload Video</a>
      </nav>
    </div>
  );
}