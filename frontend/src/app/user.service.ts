import {inject, Injectable, Signal, signal} from '@angular/core';
import {firstValueFrom} from 'rxjs';
import {AuthService} from './api/services';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _authService = inject(AuthService)

  private _userName = signal("Checking login state...");

  constructor() {
    this.checkLoggedIn();
  }

  public get userName(): Signal<string> {
    return this._userName;
  }

  async isLoggedIn(): Promise<boolean> {
    try {
      await firstValueFrom(this._authService.authManageInfoGet());
      return true;
    }
    catch {
      return false;
    }
  }

  async checkLoggedIn(): Promise<undefined> {
    try {
      const loginInfo = await firstValueFrom(this._authService.authManageInfoGet());
      this._userName.set(loginInfo.email);
    } catch {
      this._userName.set("Not logged in");
    }
  }

  async logIn(username: string, password: string): Promise<boolean> {
    const loginRequest = {
      useSessionCookies: true,
      body: {
        email: username,
        password,
      }
    };

    const response = await firstValueFrom(this._authService.authLoginPost$Response(loginRequest));
    await this.checkLoggedIn();

    return response.status === 200;
  }
}
