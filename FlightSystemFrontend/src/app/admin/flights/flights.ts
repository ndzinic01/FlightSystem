import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FlightService, FlightDTO } from '../../Services/flight-service';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../Services/Notifications/snackbar-service';
import { ConfirmDialog } from '../../Shared/confirm-dialog/confirm-dialog';
import {AddFlightDialog} from './add-flight-dialog/add-flight-dialog';

@Component({
  selector: 'app-flights',
  standalone: false,
  templateUrl: './flights.html',
  styleUrl: './flights.css',
})
export class Flights implements OnInit {
  flights: FlightDTO[] = [];
  filteredFlights: FlightDTO[] = [];
  loading = false;

  destinationSearchTerm = '';
  selectedDate: string | null = null;

  constructor(
    private flightService: FlightService,
    private cd: ChangeDetectorRef,
    private dialog: MatDialog,
    private snack: SnackbarService
  ) {}

  ngOnInit(): void {
    this.loadFlights();
  }

  loadFlights() {
    this.loading = true;
    this.flightService.getAll().subscribe({
      next: (data) => {
        this.flights = data;
        this.filteredFlights = data;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Create flight error:', err);
        this.snack.error(
          err?.error?.message ||
          err?.error ||
          'Greška prilikom dodavanja leta.'
        );
      }
    });
  }

  applyFilters() {
    let filtered = [...this.flights];

    if (this.destinationSearchTerm.trim()) {
      const term = this.destinationSearchTerm.trim().toLowerCase();
      filtered = filtered.filter((f) =>
        f.destination.toLowerCase().includes(term) ||
        f.code.toLowerCase().includes(term)
      );
    }

    if (this.selectedDate) {
      filtered = filtered.filter((f) => {
        const flightDate = new Date(f.departureTime).toISOString().split('T')[0];
        return flightDate === this.selectedDate;
      });
    }

    this.filteredFlights = filtered;
  }

  onDestinationSearch() {
    this.applyFilters();
  }

  onDateChange(event: any) {
    this.selectedDate = event.target.value;
    this.applyFilters();
  }

  clearDateFilter() {
    this.selectedDate = null;
    this.applyFilters();
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(AddFlightDialog, {
      width: '550px',
      maxWidth: '90vw',
      disableClose: true,
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.flightService.create(result).subscribe({
        next: () => {
          this.snack.success('Let je uspješno dodan.');
          this.loadFlights();
        },
        error: () => {
          this.snack.error('Greška prilikom dodavanja leta.');
        }
      });
    });
  }

  deleteFlight(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '360px',
      data: {
        message: 'Da li ste sigurni da želite obrisati ovaj let?'
      }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.flightService.delete(id).subscribe({
        next: () => {
          this.flights = this.flights.filter((f) => f.id !== id);
          this.applyFilters();
          this.snack.success('Let je uspješno obrisan.');
        },
        error: () => {
          this.snack.error('Greška prilikom brisanja leta.');
        }
      });
    });
  }

  // 🔥 MAPIRANJE ENUM BROJA NA BOSANSKI STRING
  getStatusLabel(status: number | string): string {
    // Ako je već string, vrati ga
    if (typeof status === 'string') {
      return this.translateStatus(status);
    }

    // Mapiranje broja na enum naziv
    const statusMap: Record<number, string> = {
      0: 'Scheduled',
      1: 'Boarding',
      2: 'Delayed',
      3: 'Cancelled',
      4: 'Completed'
    };

    const statusName = statusMap[status] || 'Scheduled';
    return this.translateStatus(statusName);
  }

  // 🔥 PREVOD NA BOSANSKI
  private translateStatus(status: string): string {
    const translations: Record<string, string> = {
      'Scheduled': 'Aktivan',
      'Boarding': 'Ukrcavanje',
      'Delayed': 'Odgođen',
      'Cancelled': 'Otkazan',
      'Completed': 'Završen'
    };
    return translations[status] || status;
  }

  // 🔥 CSS KLASA
  getStatusClass(status: number | string): string {
    // Ako je broj, konvertuj u string
    if (typeof status === 'number') {
      const statusMap: Record<number, string> = {
        0: 'scheduled',
        1: 'boarding',
        2: 'delayed',
        3: 'cancelled',
        4: 'completed'
      };
      return statusMap[status] || 'scheduled';
    }

    // Ako je string, lowercase
    return status.toLowerCase();
  }

  formatTime(time: string): string {
    return new Date(time).toLocaleTimeString('sr-Latn-BA', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }
}
