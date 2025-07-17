import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeroCreateRequest } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-hero-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './hero-form.component.html',
  styleUrls: ['./hero-form.component.css'],
})
export class HeroFormComponent {
  @Output() create = new EventEmitter<HeroCreateRequest>();
  @Output() cancel = new EventEmitter<void>();

  newHero: HeroCreateRequest = {
    name: '',
    ability: '',
    suitColors: '',
    startingPower: 0,
  };

  submit(): void {
    this.create.emit(this.newHero);
  }

  resetForm(): void {
    this.newHero = {
      name: '',
      ability: '',
      suitColors: '',
      startingPower: 0,
    };
    this.cancel.emit();
  }
}
