import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ReservationService, ReservationDTO } from '../../Services/reservation-service';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../Services/Notifications/snackbar-service';
import { ConfirmDialog } from '../../Shared/confirm-dialog/confirm-dialog';
import {ReservationDetailsDialog} from './reservation-details-dialog/reservation-details-dialog'; //🔥 DODAJ

@Component({
  selector: 'app-reservations',
  standalone: false,
  templateUrl: './reservations.html',
  styleUrl: './reservations.css',
})
export class Reservations implements OnInit {
  reservations: ReservationDTO[] = [];
  filteredReservations: ReservationDTO[] = [];
  loading = false;

  destinationSearchTerm = '';
  userSearchTerm = '';
  dateFrom: string | null = null;
  dateTo: string | null = null;

  constructor(
    private reservationService: ReservationService,
    private cd: ChangeDetectorRef,
    private dialog: MatDialog,
    private snack: SnackbarService
  ) {}

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations() {
    this.loading = true;
    this.reservationService.getAll().subscribe({
      next: (data) => {
        this.reservations = data;
        this.filteredReservations = data;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.snack.error('Greška prilikom učitavanja rezervacija.');
      }
    });
  }

  applyFilters() {
    let filtered = [...this.reservations];

    if (this.destinationSearchTerm.trim()) {
      const term = this.destinationSearchTerm.trim().toLowerCase();
      filtered = filtered.filter((r) =>
        r.destination.toLowerCase().includes(term) ||
        r.flightNumber.toLowerCase().includes(term)
      );
    }

    if (this.userSearchTerm.trim()) {
      const term = this.userSearchTerm.trim().toLowerCase();
      filtered = filtered.filter((r) =>
        r.userFullName.toLowerCase().includes(term)
      );
    }

    if (this.dateFrom) {
      filtered = filtered.filter((r) => {
        const resDate = new Date(r.createdAt).toISOString().split('T')[0];
        return resDate >= this.dateFrom!;
      });
    }

    if (this.dateTo) {
      filtered = filtered.filter((r) => {
        const resDate = new Date(r.createdAt).toISOString().split('T')[0];
        return resDate <= this.dateTo!;
      });
    }

    this.filteredReservations = filtered;
  }

  onDestinationSearch() {
    this.applyFilters();
  }

  onUserSearch() {
    this.applyFilters();
  }

  onDateFromChange(event: any) {
    this.dateFrom = event.target.value;
    this.applyFilters();
  }

  onDateToChange(event: any) {
    this.dateTo = event.target.value;
    this.applyFilters();
  }

  // 🔥 NOVA METODA - Otvori detalje
  openDetails(reservation: ReservationDTO) {
    this.dialog.open(ReservationDetailsDialog, {
      width: '550px',
      maxWidth: '90vw',
      data: reservation
    });
  }

  deleteReservation(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '360px',
      data: {
        message: 'Da li ste sigurni da želite obrisati ovu rezervaciju?'
      }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.reservationService.delete(id).subscribe({
        next: () => {
          this.reservations = this.reservations.filter((r) => r.id !== id);
          this.applyFilters();
          this.snack.success('Rezervacija je uspješno obrisana.');
        },
        error: () => {
          this.snack.error('Greška prilikom brisanja rezervacije.');
        }
      });
    });
  }

  getStatusLabel(status: number): string {
    const statusMap: Record<number, string> = {
      0: 'Potvrđeno',
      1: 'Otkazano',
      2: 'Na čekanju'
    };
    return statusMap[status] || 'Nepoznato';
  }

  getStatusClass(status: number): string {
    const classMap: Record<number, string> = {
      0: 'confirmed',
      1: 'cancelled',
      2: 'pending'
    };
    return classMap[status] || 'pending';
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }
}
