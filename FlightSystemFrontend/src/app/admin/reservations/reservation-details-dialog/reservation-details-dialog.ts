import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ReservationDTO } from '../../../Services/reservation-service';

@Component({
  selector: 'app-reservation-details-dialog',
  standalone: false,
  templateUrl: './reservation-details-dialog.html',
  styleUrl: './reservation-details-dialog.css',
})
export class ReservationDetailsDialog {
  constructor(
    public dialogRef: MatDialogRef<ReservationDetailsDialog>,
    @Inject(MAT_DIALOG_DATA) public data: ReservationDTO
  ) {}

  close() {
    this.dialogRef.close();
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }

  // Ekstraktuj gradove iz destinacije
  getFromCity(): string {
    return this.data.destination.split(' → ')[0] || '';
  }

  getToCity(): string {
    return this.data.destination.split(' → ')[1] || '';
  }

}
