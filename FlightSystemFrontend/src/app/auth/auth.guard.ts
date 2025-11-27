import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Auth} from './auth';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private auth: Auth, private router: Router) {}

  canActivate(): boolean {
    const token = this.auth.getToken();

    if (!token) {
      this.router.navigate(['/auth/login']);
      return false;
    }

    return true;
  }
}

