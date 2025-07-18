import { Component, Input } from '@angular/core';
import { Hero } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-other-hero-card',
  standalone: true,
  imports: [],
  templateUrl: './other-hero-card.component.html',
  styleUrls: ['./other-hero-card.component.css'],
})
export class OtherHeroCardComponent {
  @Input() hero!: Hero;
}
