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
  @Output() create = new EventEmitter<HeroCreateRequest>();
  @Output() cancel = new EventEmitter<void>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      ability: ['', Validators.required],
      suitColors: ['', Validators.required],
      startingPower: [0, [Validators.required, Validators.min(0)]],
    });
  }

  submit(): void {
    if (this.form.valid) {
      this.create.emit(this.form.value);
    }
  }

  resetForm(): void {
    this.form.reset();
    this.cancel.emit();
  }
  isInvalid(field: string): boolean {
    const control = this.form.get(field);
    return !!control && control.invalid && (control.dirty || control.touched);
  }
}
