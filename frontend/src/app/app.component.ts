import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  imports: [
      RouterOutlet
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  private userService = inject(UserService)

  title = 'OwtBoats';

  async ngOnInit(): Promise<any> {
    const isLoggedIn = await this.userService.isLoggedIn();
    console.log("Are we logged in?", isLoggedIn);
  }
}
