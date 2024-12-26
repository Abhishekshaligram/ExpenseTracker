import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-show-error',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './show-error.component.html',
  styleUrl: './show-error.component.scss'
})
export class ShowErrorComponent {
  @Input() name?: string;

  fieldData?: AbstractControl | null;
  @Input()
  set field(field: AbstractControl | null) {
    this.fieldData = field;
  }

  constructor() {}

  ngOnInit(): void {}
}
