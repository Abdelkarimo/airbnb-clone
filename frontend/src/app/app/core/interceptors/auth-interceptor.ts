import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse, HttpRequest } from '@angular/common/http';
import { AuthService } from '../services/auth-service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  // Clone request and add Authorization header if token exists
  let authRequest = req;
  if (token) {
    authRequest = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(authRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // Try refreshing token if expired
        return authService.refreshToken().pipe(
          switchMap(response => {
            const newToken = response.token;
            const newReq = req.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` }
            });
            return next(newReq);
          }),
          catchError(refreshError => {
            // If refresh fails, log out and throw
            authService.logout().subscribe();
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
