import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/auth.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup<{
    email: FormControl<string>;
    password: FormControl<string>;
  }>;

  serverError: string | null = null;
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required, Validators.email],
      }),
      password: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required],
      }),
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;
    const formValue = this.loginForm.value as LoginRequest;

    this.authService.login(formValue).subscribe({
      next: (res) => {
        if (!res.success) {
          this.serverError = res.message || 'Login failed.';
          return;
        }

        this.successMessage = res.message ?? 'Login successful!';
        this.serverError = '';
        localStorage.setItem('token', res.token);
        setTimeout(() => this.router.navigate(['/heroes']), 2000);
      },
      error: (err) => {
        this.serverError = 'Server error. Please try again.';
      },
    });
  }

  get email() {
    return this.loginForm.controls.email;
  }

  get password() {
    return this.loginForm.controls.password;
  }
}
