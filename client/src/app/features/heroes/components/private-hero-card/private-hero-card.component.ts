import { Component, Input } from '@angular/core';
import { Hero } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-private-hero-card',
  standalone: true,
  imports: [],
  templateUrl: './private-hero-card.component.html',
  styleUrls: ['./private-hero-card.component.css'],
})
export class PrivateHeroCardComponent {
  @Input() hero!: Hero;
}
