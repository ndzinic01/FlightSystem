import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {UserDTO, UserService} from '../../Services/user-service';
import {SnackbarService} from '../../Services/Notifications/snackbar-service';

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.html',
  styleUrl: './users.css',
})
export class Users implements OnInit {
  users: UserDTO[] = [];
  filteredUsers: UserDTO[] = [];
  searchTerm = '';
  loading = false;

  constructor(
    private userService: UserService,
    private snack: SnackbarService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;

    this.userService.getAll().subscribe({
      next: data => {
        this.users = data.filter(u => !u.isDeleted);
        this.filteredUsers = this.users;
        this.loading = false;

        this.cdr.detectChanges(); // ✅ OVO RIJEŠAVA PROBLEM PRVOG KLIKA
      },
      error: () => {
        this.loading = false;
        this.snack.error('Greška pri učitavanju korisnika.');
      }
    });
  }


  applyFilter() {
    const term = this.searchTerm.toLowerCase().trim();

    if (!term) {
      this.filteredUsers = this.users;
      return;
    }

    this.filteredUsers = this.users.filter(u =>
      u.fullName.toLowerCase().includes(term) ||
      u.email.toLowerCase().includes(term)
    );
  }

  updateStatus(user: UserDTO, value: boolean) {
    this.userService.update({
      id: user.id,
      isActive: value
    }).subscribe(() => {
      user.isActive = value;
      this.snack.success('Status ažuriran.');
    });
  }

  updateRole(user: UserDTO, role: string) {
    this.userService.update({
      id: user.id,
      role: role
    }).subscribe(() => {
      user.role = role;
      this.snack.success('Uloga ažurirana.');
    });
  }

  deleteUser(user: UserDTO) {
    if (!confirm('Da li ste sigurni da želite ukloniti korisnika?')) return;

    this.userService.delete(user.id).subscribe(() => {
      this.users = this.users.filter(u => u.id !== user.id);
      this.applyFilter();
      this.snack.success('Korisnik uklonjen.');
    });
  }
}
