import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.html',
  standalone: false
})

export class Register implements OnInit {
  registerForm!: FormGroup; // Property za formu

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Inicijalizacija forme u ngOnInit
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    const { password, confirmPassword, ...rest } = this.registerForm.value;
    if (password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    const body = { ...rest, password };

    this.auth.register(body).subscribe({
      next: () => {
        alert("Registration successful!");
        this.router.navigate(['/auth/login']);
      },
      error: err => alert(err.error.message || "Registration failed")
    });
  }
}
