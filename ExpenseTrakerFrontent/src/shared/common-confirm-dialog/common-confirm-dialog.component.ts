import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-common-confirm-dialog',
  standalone: true,
  imports: [DialogModule,CommonModule,ButtonModule],
  templateUrl: './common-confirm-dialog.component.html',
  styleUrl: './common-confirm-dialog.component.scss'
})
export class CommonConfirmDialogComponent {
  constructor(
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef
  ) { }
  @Input() visible: boolean = false;
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() confirmButtonText: string = 'Confirm';
  @Input() cancelButtonText: string = 'Cancel';
  @Output() onConfirm = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  ngOnInit(): void {
    if (this.config.data) {
      this.visible = this.config.data.visible ? this.config.data.visible : null;
      this.title = this.config.data.title ? this.config.data.title : null;
      this.message = this.config.data.message ? this.config.data.message : null;
      this.confirmButtonText = this.config.data.confirmButtonText ? this.config.data.confirmButtonText : null;
      this.cancelButtonText = this.config.data.cancelButtonText ? this.config.data.cancelButtonText : null;
    }
  }
  confirm() {
    this.onConfirm.emit();
    this.ref.close('confirm');
  }

  cancel() {
    this.onCancel.emit();
    this.ref.close('hide');
  }
}
