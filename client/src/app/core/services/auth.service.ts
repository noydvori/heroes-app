import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError, tap } from 'rxjs';
import {
  LoginRequest,
  AuthResponse,
  RegisterRequest,
} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = 'http://localhost:5026/api/auth';

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap((res) => {
          if (res.token) {
            this.storeToken(res.token);
          } else {
            console.warn('Login response missing token');
          }
        }),
        catchError((err) => throwError(() => this.getErrorMessage(err)))
      );
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, data)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  private storeToken(token: string): void {
    localStorage.setItem('token', token);
  }

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object' && 'error' in error) {
      const err = error as { error?: { message?: string } };
      return err.error?.message || 'Something went wrong. Please try again.';
    }
    return 'Unexpected error';
  }
}
