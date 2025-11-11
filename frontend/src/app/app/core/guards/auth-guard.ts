import { CanActivate, CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { inject, Injectable } from '@angular/core';

export const authGuard: CanActivateFn = (): boolean | UrlTree => {

  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isAuthenticated()) {
    return true;
  }
  else {
    return router.createUrlTree(['/auth/login']);
  }
};
