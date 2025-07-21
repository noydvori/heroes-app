import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { HeroCreateRequest } from '../../../../core/models/hero.model';

@Component({
  selector: 'app-hero-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './hero-form.component.html',
  styleUrls: ['./hero-form.component.css'],
})
export class HeroFormComponent {
  @Output() onCreate = new EventEmitter<HeroCreateRequest>();
  @Output() onCancel = new EventEmitter<void>();

  addHeroForm: FormGroup;
  availableColors = [
    'Red',
    'Blue',
    'Green',
    'Yellow',
    'Black',
    'Pink',
    'Orange',
    'Teal',
  ];

  constructor(private fb: FormBuilder) {
    this.addHeroForm = this.fb.group({
      name: ['', Validators.required],
      ability: ['', Validators.required],
      suitColors: [[], Validators.required],
      startingPower: [0, [Validators.required, Validators.min(0)]],
    });
  }

  onSubmit(): void {
    if (this.addHeroForm.valid) {
      const rawValue = this.addHeroForm.value;

      const heroRequest: HeroCreateRequest = {
        ...rawValue,
        suitColors: (rawValue.suitColors as string[]).join(','),
      };

      this.onCreate.emit(heroRequest);
    }
  }

  resetForm(): void {
    this.addHeroForm.reset();
    this.onCancel.emit();
  }
  onColorChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const value = checkbox.value;
    const current = this.suitColors.value as string[];

    if (checkbox.checked) {
      this.suitColors.setValue([...current, value]);
    } else {
      this.suitColors.setValue(current.filter((c) => c !== value));
    }

    this.suitColors.markAsTouched();
  }
  get name() {
    return this.addHeroForm.controls['name'];
  }
  get ability() {
    return this.addHeroForm.controls['ability'];
  }
  get suitColors() {
    return this.addHeroForm.controls['suitColors'];
  }
  get startingPower() {
    return this.addHeroForm.controls['startingPower'];
  }
}
