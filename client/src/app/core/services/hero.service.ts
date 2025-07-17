import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Hero, HeroCreateRequest } from '../models/hero.model';

@Injectable({ providedIn: 'root' })
export class HeroService {
  private baseUrl = 'http://localhost:5026/api/hero';

  constructor(private http: HttpClient) {}

  getMyHeroes(): Observable<Hero[]> {
    return this.http
      .get<Hero[]>(this.baseUrl)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  createHero(heroData: HeroCreateRequest): Observable<Hero> {
    return this.http
      .post<Hero>(this.baseUrl, heroData)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  trainHero(heroId: string): Observable<{ message: string }> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/train/${heroId}`, null)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object' && 'error' in error) {
      const err = error as { error?: { message?: string } };
      return err.error?.message || 'Something went wrong.';
    }
    return 'Unexpected error';
  }
}
