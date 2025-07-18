import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Hero } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-hero-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hero-card.component.html',
  styleUrls: ['./hero-card.component.css'],
})
export class HeroCardComponent {
  @Input() hero!: Hero;
  @Input() currentUserId!: string;

  @Output() train = new EventEmitter<Hero>();

  onTrainClick(): void {
    if (this.hero.currentPower < 1000) {
      this.train.emit(this.hero);
    }
  }
}
