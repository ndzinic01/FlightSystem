import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Auth } from '../auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.css',
  standalone: false
})
export class Login implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private auth: Auth,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.loginForm.disable();

    this.auth.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.loginForm.enable();

        if (res.user.role !== 'Admin') {
          alert("Nemate privilegije za pristup administratorskoj aplikaciji!");
          this.auth.logout();
          return;
        }

        this.router.navigate(['/admin/dashboard']);
      },
      error: () => {
        this.isLoading = false;
        this.loginForm.enable();
        alert("Pogrešno korisničko ime ili lozinka");
      }
    });
  }
}
