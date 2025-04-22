import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { BoatListComponent } from './boat-list/boat-list.component';

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
    title: "Home",
  },
  {
    path: "login",
    component: LoginComponent,
    title: "Log in"
  },
  {
    path: "boatslist",
    component: BoatListComponent,
    title: "Boats"
  },
  {
    path: "**",
    redirectTo: "",
    title: "Fallback"
  }
];
