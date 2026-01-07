import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FlightService, FlightDTO } from '../../Services/flight-service';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../Services/Notifications/snackbar-service';
import { ConfirmDialog } from '../../Shared/confirm-dialog/confirm-dialog';

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
      error: () => {
        this.loading = false;
        this.snack.error('Greška prilikom učitavanja letova.');
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
    //this.snack.info('Add flight dialog - TODO');
  }

  editFlight(flight: FlightDTO) {
   // this.snack.info('Edit flight dialog - TODO');
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

  // 🔥 PREVOD STATUSA na bosanski
  getStatusLabel(status: string): string {
    const translations: Record<string, string> = {
      'Scheduled': 'Aktivan',
      'Boarding': 'Ukrcavanje',
      'Delayed': 'Odgođen',
      'Cancelled': 'Otkazan',
      'Completed': 'Završen'
    };
    return translations[status] || status;
  }

  // 🔥 CSS klasa za status
  getStatusClass(status: string): string {
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
