import { HttpInterceptorFn } from '@angular/common/http';

function isPublicRequest(url: string): boolean {
  return ['/login', '/register'].some((path) => url.includes(path));
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');

  if (token && !isPublicRequest(req.url)) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(req);
};
