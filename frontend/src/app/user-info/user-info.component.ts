import { Component, inject } from '@angular/core';
import { UserService } from '../user.service';
import {Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user-info',
  imports: [RouterLink, MatButtonModule],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent {
  private _userService = inject(UserService)
  private _router = inject(Router);

  public get userName() {
    const currentName = this._userService.userName();
    if (typeof currentName === "object") {
      return currentName.email;
    } else {
      return null;
    }
  }

  public logout() {
    this._userService.logout();
    this._router.navigateByUrl("/login");
  }

  public get showLoginLink(): boolean {
    return this._userService.userName() === "NotLoggedIn";
  }

  public get showLogoutLink(): boolean {
    return typeof this._userService.userName() === "object";
  }
}
