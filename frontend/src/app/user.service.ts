import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './api/services';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authService = inject(AuthService)

  constructor() { }

  async isLoggedIn(): Promise<boolean> {
    try {
      await firstValueFrom(this.authService.authManageInfoGet());
      return true;
    }
    catch {
      return false;
    }
  }
}
