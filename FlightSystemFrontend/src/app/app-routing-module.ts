import { NgModule } from '@angular/core';
import {NavigationEnd, Router, RouterModule, Routes} from '@angular/router';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: 'auth/login', component: Login },
  { path: 'auth/register', component: Register },

  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./admin/admin-module').then(m => m.AdminModule)
  },

  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: '**', redirectTo: 'auth/login' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload',   // 🔥 ključno !!
      scrollPositionRestoration: 'enabled' // nije obavezno ali user-friendly
    })
  ],

  exports: [RouterModule]
})
export class AppRoutingModule {
  constructor(router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        router.navigated = false;
      }
    });
  }
}


