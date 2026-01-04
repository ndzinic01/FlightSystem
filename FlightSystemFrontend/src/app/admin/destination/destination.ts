import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {DestinationService, DestinationDTO, DestinationUpdateDTO} from '../../Services/destination-service';
import {FlightService} from '../../Services/flight-service';
import { MatDialog } from '@angular/material/dialog';
import {SnackbarService} from '../../Services/Notifications/snackbar-service';
import {ConfirmDialog} from '../../Shared/confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-destination',
  standalone: false,
  templateUrl: './destination.html',
  styleUrl: './destination.css',
})
export class Destination implements OnInit {
  destinations: DestinationDTO[] = [];
  loading = false;

  filteredDestinations: DestinationDTO[] = [];
  searchTerm: string = '';

  selectedDestinationId: number | null = null;
  flights: any[] = [];
  loadingFlights = false;

  editingDestination: any | null = null;


  constructor(private destinationService: DestinationService,
              private cd: ChangeDetectorRef,
              private flightService: FlightService,
              private snack: SnackbarService,
              private dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadDestinations();
  }

  loadDestinations() {
    this.loading = true;
    this.destinationService.getAll().subscribe({
      next: (data) => {
        this.destinations = data;
        this.loading = false;
        this.filteredDestinations = data;
        this.cd.detectChanges(); // 👈 ISTO KAO AIRPORTS
      },
      error: () => {
        this.loading = false;
      }
    });
  }
  applyFilter() {
    const term = this.searchTerm.trim().toLowerCase();

    if (!term) {
      // 🔑 ako je search prazan → vrati sve iz baze
      this.filteredDestinations = this.destinations;
      return;
    }

    this.filteredDestinations = this.destinations.filter(d =>
      d.fromCity.toLowerCase().includes(term) ||
      d.toCity.toLowerCase().includes(term) ||
      d.fromAirportCode.toLowerCase().includes(term) ||
      d.toAirportCode.toLowerCase().includes(term)
    );
  }

  delete(id: number) {
    if (!confirm('Da li ste sigurni?')) return;

    this.destinationService.delete(id).subscribe(() => {
      this.loadDestinations();
    });
  }
  showFlights(destinationId: number) {
    if (this.selectedDestinationId === destinationId) {
      this.selectedDestinationId = null;
      this.flights = [];
      return;
    }

    this.selectedDestinationId = destinationId;
    this.loadingFlights = true;

    this.flightService
      .getByDestination(destinationId)
      .subscribe({
        next: res => {
          this.flights = res;
          this.loadingFlights = false;
          this.cd.detectChanges(); // 👈 ISTO KAO AIRPORTS
        },
        error: () => this.loadingFlights = false
      });
  }
  editDestination(dest: DestinationDTO) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '360px',
      data: {
        message: `Da li želite ${dest.isActive ? 'deaktivirati' : 'aktivirati'} ovu destinaciju?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;

      this.destinationService
        .toggleStatus({ id: dest.id, isActive: !dest.isActive })
        .subscribe({
          next: updated => {
            dest.isActive = updated.isActive;
            this.snack.success('Status destinacije je uspješno izmijenjen.');
            this.cd.detectChanges();
          },
          error: () => {
            this.snack.error('Greška prilikom izmjene statusa.');
          }
        });
    });
  }



  deleteDestination(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '360px',
      data: {
        message: 'Da li ste sigurni da želite obrisati ovu destinaciju?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;

      this.destinationService.delete(id).subscribe({
        next: () => {
          this.destinations = this.destinations.filter(d => d.id !== id);
          this.applyFilter();
          this.snack.success('Destinacija je uspješno obrisana.');
        },
        error: () => {
          this.snack.error('Nije moguće obrisati destinaciju.');
        }
      });
    });
  }


}
