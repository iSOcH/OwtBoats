import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { UserService } from './user.service';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';

export const authRouteGuard: CanActivateFn = (route, state) => {
  const authService = inject(UserService);
  return toObservable(authService.isLoggedIn).pipe(filter(s => s !== null));
};
