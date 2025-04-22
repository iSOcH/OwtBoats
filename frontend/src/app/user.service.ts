import {computed, inject, Injectable, Signal, signal} from '@angular/core';
import {firstValueFrom} from 'rxjs';
import {AuthService} from './api/services';

type LoginState = "Pending" | "NotLoggedIn" | { email: string; };

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _authService = inject(AuthService)

  private _userName = signal<LoginState>("Pending");

  public readonly isLoggedIn = computed(() => {
    const currentUsername = this._userName();
    if (currentUsername === "Pending") {
      return null;
    } else {
      return currentUsername !== "NotLoggedIn";
    }
  })

  constructor() {
    this.checkLoggedIn();
  }

  public get userName(): Signal<LoginState> {
    return this._userName;
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

  async logout(): Promise<void> {
    await firstValueFrom(this._authService.authLogoutPost());
    await this.checkLoggedIn();
  }
}
