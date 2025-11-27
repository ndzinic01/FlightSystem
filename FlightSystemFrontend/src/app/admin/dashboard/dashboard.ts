import {Component, OnInit} from '@angular/core';
import {Auth} from '../../auth/auth';
import { Router } from '@angular/router';
@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {

  username: string = '';
  role: string = '';

  constructor(private auth: Auth, private router: Router) {}

  ngOnInit(): void {
    const user = this.auth.getUser(); // pretpostavljamo da AuthService čuva usera u localStorage
    if (user) {
      this.username = user.username;
      this.role = user.role;
    }
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
