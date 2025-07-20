import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeroCardComponent } from '../hero-card/hero-card.component';
import { HeroFormComponent } from '../hero-form/hero-form.component';
import { HeroService } from '../../../../core/services/hero.service';
import { Hero, HeroCreateRequest } from '../../../../core/models/hero.model';
import { OtherHeroCardComponent } from '../other-hero-card/other-hero-card-component';
import { getCurrentUserIdFromToken } from '../../../../core/utils/token-utils';
import { HeroHubService } from '../../../../core/services/hero-hub.service';

@Component({
  selector: 'app-hero-list',
  standalone: true,
  imports: [
    CommonModule,
    HeroCardComponent,
    HeroFormComponent,
    OtherHeroCardComponent,
  ],
  templateUrl: './heroes-list.component.html',
  styleUrls: ['./heroes-list.component.css'],
})
export class HeroListComponent implements OnInit {
  heroes: Hero[] = [];
  error: string | null = null;
  loaded = false;
  showForm = false;
  currentUserId: string = '';

  constructor(
    private heroService: HeroService,
    private heroHubService: HeroHubService
  ) {}

  ngOnInit(): void {
    this.currentUserId = getCurrentUserIdFromToken();
    this.loadHeroes();

    this.heroHubService.startConnection();

    this.heroHubService.heroChanged$.subscribe((updatedHero) => {
      this.updateHeroList(updatedHero);
    });
  }

  loadHeroes(): void {
    this.heroService.getAllHeroes().subscribe({
      next: (data) => {
        this.heroes = data;
        this.loaded = true;
      },
      error: () => {
        this.error = 'Failed to load heroes.';
      },
    });
  }

  createHero(hero: HeroCreateRequest): void {
    this.heroService.createHero(hero).subscribe({
      next: (newHero: Hero) => {
        this.showForm = false;
        this.updateHeroList(newHero);
        alert(`Hero ${newHero.name} created successfully`);
      },
      error: () => {
        alert('Failed to create hero.');
      },
    });
  }

  trainHero(hero: Hero): void {
    this.heroService.trainHero(hero.id).subscribe({
      next: (res: any) => {
        const message = res?.message ?? `${hero.name} trained successfully!`;
        alert(message);

        if (res.updatedHero) {
          this.updateHeroList(res.updatedHero);
        }

        this.heroes.sort((a, b) => b.currentPower - a.currentPower);
      },
      error: (err) => {
        const message = err ?? 'Training failed.';
        alert(message);
      },
    });
  }

  trackById(index: number, hero: Hero): string {
    return hero.id;
  }
  private updateHeroList(updatedHero: Hero): void {
    const index = this.heroes.findIndex((h) => h.id === updatedHero.id);

    if (index > -1) {
      this.heroes[index] = updatedHero;
    } else {
      this.heroes.push(updatedHero);
    }

    this.sortHeroes();
  }

  private sortHeroes(): void {
    this.heroes.sort((a, b) => b.currentPower - a.currentPower);
  }
}
