import {inject, Injectable, Signal, signal} from '@angular/core';
import {firstValueFrom} from 'rxjs';
import {AuthService} from './api/services';

type LoginState = "Pending" | "NotLoggedIn" | { email: string; };

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _authService = inject(AuthService)

  private _userName = signal<LoginState>("NotLoggedIn");

  constructor() {
    this.checkLoggedIn();
  }

  public get userName(): Signal<LoginState> {
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
      this._userName.set({ email: loginInfo.email });
    } catch {
      this._userName.set("NotLoggedIn");
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
