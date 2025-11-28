import {Component, OnInit} from '@angular/core';
import {Auth} from '../../auth/auth';
import {Router} from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css',
})
export class AdminLayout implements OnInit {

  username: string = '';

  constructor(private auth: Auth, private router: Router) {}

  ngOnInit(): void {
    const user = this.auth.getUser();
    this.username = user?.username || 'Admin';
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
