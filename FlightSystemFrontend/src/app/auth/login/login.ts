import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Auth } from '../auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl:'./login.css',
  standalone: false
})
export class Login implements OnInit {
  loginForm!: FormGroup; // Property za formu

  constructor(
    private fb: FormBuilder,
    private auth: Auth,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Inicijalizacija forme u ngOnInit
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.auth.login(this.loginForm.value).subscribe({
        next: (res) => {
          if (res.user.role !== 'Admin') {
            alert("Nemate privilegije za pristup administratorskoj aplikaciji!");
            this.auth.logout();
            return;
          }
          alert("Login successful!");
          this.router.navigate(['/admin/dashboard']);
      },
      error: () => alert("Invalid username or password")
    });
  }
}

