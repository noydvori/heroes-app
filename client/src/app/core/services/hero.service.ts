import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import {
  Hero,
  HeroCreateRequest,
  HeroUpdateRequest,
} from '../models/hero.model';
import { API_BASE_URL } from '../../app.config';

@Injectable({ providedIn: 'root' })
export class HeroService {
  private readonly apiUrl = `${API_BASE_URL}/heroes`;

  constructor(private http: HttpClient) {}

  getMyHeroes(): Observable<Hero[]> {
    return this.http
      .get<Hero[]>(this.apiUrl)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }
  getAllHeroes(): Observable<Hero[]> {
    return this.http
      .get<Hero[]>(`${this.apiUrl}/all`)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  createHero(heroData: HeroCreateRequest): Observable<Hero> {
    return this.http
      .post<Hero>(this.apiUrl, heroData)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  trainHero(heroId: string): Observable<{ message: string }> {
    return this.http
      .post<{ message: string }>(`${this.apiUrl}/train/${heroId}`, null)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  updateHero(id: string, data: HeroUpdateRequest): Observable<Hero> {
    return this.http
      .put<Hero>(`${this.apiUrl}/${id}`, data)
      .pipe(catchError((err) => throwError(() => this.getErrorMessage(err))));
  }

  deleteHero(id: string): Observable<{ message: string }> {
    return this.http
      .delete<{ message: string }>(`${this.apiUrl}/${id}`)
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
