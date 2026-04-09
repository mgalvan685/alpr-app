//const API_BASE_URL = "http://localhost:5001";
//const API_BASE_URL = "http://localhost:7280";
const API_BASE_URL = "https://localhost:7280";

export async function apiGet<T>(url: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${url}`);
  if (!response.ok) {
    throw new Error(`GET ${url} failed: ${response.status}`);
  }
  return response.json() as Promise<T>;
}

export async function apiPostForm<T>(url: string, formData: FormData): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${url}`, {
    method: "POST",
    body: formData
  });

  if (!response.ok) {
    throw new Error(`POST ${url} failed: ${response.status}`);
  }

  return response.json() as Promise<T>;
}
