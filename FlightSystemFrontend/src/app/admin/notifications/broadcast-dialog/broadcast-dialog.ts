import { Component, ChangeDetectorRef } from '@angular/core';
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
    private snack: SnackbarService,
    private cd: ChangeDetectorRef // 🔥 DODANO
  ) {
    this.loadFlights();
  }

  loadFlights() {
    this.flightService.getAll().subscribe({
      next: (data) => {
        this.flights = data;
        this.cd.detectChanges(); // 🔥 DODANO
      }
    });
  }

  send() {
    if (!this.message.trim()) {
      this.snack.error('Molimo unesite poruku.');
      return;
    }

    if (this.message.length > 500) {
      this.snack.error('Poruka ne može biti duža od 500 karaktera.');
      return;
    }

    this.loading = true;
    this.cd.detectChanges(); // 🔥 DODANO

    this.notificationService.broadcast({
      message: this.message,
      flightId: this.selectedFlightId || undefined
    }).subscribe({
      next: (res) => {
        // 🔥 Koristi setTimeout da izbjegneš change detection grešku
        setTimeout(() => {
          this.loading = false;
          this.snack.success(res.message || 'Broadcast notifikacija poslana.');
          this.dialogRef.close(true);
        }, 0);
      },
      error: () => {
        setTimeout(() => {
          this.loading = false;
          this.snack.error('Greška prilikom slanja notifikacije.');
          this.cd.detectChanges();
        }, 0);
      }
    });
  }

  cancel() {
    this.dialogRef.close();
  }

  getCurrentDate(): string {
    return new Date().toLocaleDateString('sr-Latn-BA', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
