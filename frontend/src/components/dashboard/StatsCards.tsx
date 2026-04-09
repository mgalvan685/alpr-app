export function StatCard({ label, value }: { label: string; value: number }) {
  return (
    <div className="bg-card border border-border rounded-lg p-6 text-center">
      <div className="text-3xl font-bold text-foreground">{value}</div>
      <div className="text-muted-foreground mt-2">{label}</div>
    </div>
  );
}
