export function isTokenValid(token: string): boolean {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp;
    return typeof exp === 'number' && exp * 1000 > Date.now();
  } catch {
    return false;
  }
}
export function getCurrentUserIdFromToken(): string {
  const token = localStorage.getItem('token');
  if (!token) return '';

  const payload = JSON.parse(atob(token.split('.')[1]));
  return payload['id'];
}
