import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export function PageContainer({ children }: Props) {
  return (
    <div className="flex-1 p-6 bg-gray-100 min-h-screen">
      {children}
    </div>
  );
}