import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../../Services/notification-service';
import { FlightService, FlightDTO } from '../../../Services/flight-service';
import { SnackbarService } from '../../../Services/Notifications/snackbar-service';

@Component({
  selector: 'app-broadcast-dialog',
  standalone: false,
  templateUrl: './broadcast-dialog.html',
  styleUrls: ['./broadcast-dialog.css']
})
export class BroadcastDialog {
  message = '';
  selectedFlightId: number | null = null;
  flights: FlightDTO[] = [];
  loading = false;

  constructor(
    public dialogRef: MatDialogRef<BroadcastDialog>,
    private notificationService: NotificationService,
    private flightService: FlightService,
    private snack: SnackbarService
  ) {
    this.loadFlights();
  }

  loadFlights() {
    this.flightService.getAll().subscribe({
      next: (data) => {
        this.flights = data;
      }
    });
  }

  send() {
    if (!this.message.trim()) {
      this.snack.error('Molimo unesite poruku.');
      return;
    }

    this.loading = true;

    this.notificationService.broadcast({
      message: this.message,
      flightId: this.selectedFlightId || undefined
    }).subscribe({
      next: (res) => {
        this.loading = false;
        this.snack.success(res.message || 'Broadcast notifikacija poslana.');
        this.dialogRef.close(true);
      },
      error: () => {
        this.loading = false;
        this.snack.error('Greška prilikom slanja notifikacije.');
      }
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}
