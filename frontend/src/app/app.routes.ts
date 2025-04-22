import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { BoatListComponent } from './boat-list/boat-list.component';
import { BoatDetailComponent } from './boat-edit/boat-detail.component';
import { authRouteGuard } from './auth-route.guard';

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
    path: "boat-list",
    component: BoatListComponent,
    title: "Boats",
    canActivate: [authRouteGuard]
  },
  {
    path: "boat-detail/:boatId/:mode",
    component: BoatDetailComponent,
    canActivate: [authRouteGuard]
  },
  {
    path: "**",
    redirectTo: "",
    title: "Fallback"
  }
];
