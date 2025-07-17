import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeroCardComponent } from '../hero-card/hero-card.component';
import { HeroFormComponent } from '../hero-form/hero-form.component';
import { HeroService } from '../../../../core/services/hero.service';
import { Hero, HeroCreateRequest } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-hero-list',
  standalone: true,
  imports: [CommonModule, HeroCardComponent, HeroFormComponent],
  templateUrl: './heroes-list.component.html',
  styleUrls: ['./heroes-list.component.css'],
})
export class HeroListComponent implements OnInit {
  heroes: Hero[] = [];
  error: string | null = null;
  loaded = false;
  showForm = false;

  constructor(private heroService: HeroService) {}

  ngOnInit(): void {
    this.loadHeroes();
  }

  loadHeroes(): void {
    this.heroService.getMyHeroes().subscribe({
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

        this.heroes.push(newHero);
        this.heroes.sort((a, b) => b.currentPower - a.currentPower);
      },
      error: () => alert('Failed to create hero.'),
    });
  }

  trainHero(hero: Hero): void {
    this.heroService.trainHero(hero.id).subscribe({
      next: (res: any) => {
        const message = res?.message ?? `${hero.name} trained successfully!`;
        alert(message);

        if (res.updatedHero) {
          const index = this.heroes.findIndex((h) => h.id === hero.id);
          if (index > -1) {
            this.heroes[index] = res.updatedHero;
          }
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
}
