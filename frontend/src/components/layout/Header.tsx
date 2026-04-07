import { useState } from "react";
import { useNavigate } from "react-router-dom";

export function Header() {
  const [query, setQuery] = useState("");
  const navigate = useNavigate();

  function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!query.trim()) return;

    navigate(`/plates/${query.trim().toUpperCase()}`);
    setQuery("");
  }

  return (
    <div className="w-full bg-white shadow px-6 py-4 flex items-center">
      <form onSubmit={onSubmit} className="flex-1">
        <input
          type="text"
          placeholder="Search plates…"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          className="w-80 border rounded px-3 py-2 text-gray-700"
        />
      </form>
    </div>
  );
}