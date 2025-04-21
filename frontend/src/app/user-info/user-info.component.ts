import { Component, inject } from '@angular/core';
import { UserService } from '../user.service';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-user-info',
  imports: [RouterLink],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent {
  private _userService = inject(UserService)

  public get userName() {
    const currentName = this._userService.userName();
    if (typeof currentName === "object") {
      return currentName.email;
    } else {
      return null;
    }
  }

  public get showLoginLink(): boolean {
    return this._userService.userName() === "NotLoggedIn";
  }
}
