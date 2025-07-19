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

  constructor(private fb: FormBuilder) {
    this.addHeroForm = this.fb.group({
      name: ['', Validators.required],
      ability: ['', Validators.required],
      suitColors: ['', Validators.required],
      startingPower: [0, [Validators.required, Validators.min(0)]],
    });
  }

  onSubmit(): void {
    if (this.addHeroForm.valid) {
      this.onCreate.emit(this.addHeroForm.value);
    }
  }

  resetForm(): void {
    this.addHeroForm.reset();
    this.onCancel.emit();
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
