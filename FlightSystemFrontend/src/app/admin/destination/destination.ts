import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { DestinationService, DestinationDTO, DestinationAddDTO } from '../../Services/destination-service';
import { FlightService } from '../../Services/flight-service';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../Services/Notifications/snackbar-service';
import { ConfirmDialog } from '../../Shared/confirm-dialog/confirm-dialog';
import { AddDestinationDialog } from './add-destination-dialog/add-destination-dialog';

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

  constructor(
    private destinationService: DestinationService,
    private cd: ChangeDetectorRef,
    private flightService: FlightService,
    private snack: SnackbarService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadDestinations();
  }

  loadDestinations() {
    this.loading = true;
    this.destinationService.getAll().subscribe({
      next: (data) => {
        this.destinations = data;
        this.filteredDestinations = data;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.snack.error('Greška prilikom učitavanja destinacija.');
      }
    });
  }

  applyFilter() {
    const term = this.searchTerm.trim().toLowerCase();

    if (!term) {
      this.filteredDestinations = this.destinations;
      return;
    }

    this.filteredDestinations = this.destinations.filter(
      (d) =>
        d.fromCity.toLowerCase().includes(term) ||
        d.toCity.toLowerCase().includes(term) ||
        d.fromAirportCode.toLowerCase().includes(term) ||
        d.toAirportCode.toLowerCase().includes(term)
    );
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(AddDestinationDialog, {
      width: '750px',  // 🔥 Povećano sa 700px
      maxWidth: '90vw',  // 🔥 Maksimalno 90% ekrana
      disableClose: true,
      autoFocus: false  // 🔥 Sprečava automatski fokus na prvi input
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.destinationService.create(result).subscribe({
        next: () => {
          this.snack.success('Destinacija je uspješno dodana.');
          this.loadDestinations();
        },
        error: () => {
          this.snack.error('Greška prilikom kreiranja destinacije.');
        }
      });
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

    this.flightService.getByDestination(destinationId).subscribe({
      next: (res) => {
        this.flights = res;
        this.loadingFlights = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.loadingFlights = false;
        this.snack.error('Greška prilikom učitavanja letova.');
      }
    });
  }

  editDestination(dest: DestinationDTO) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '360px',
      data: {
        message: `Da li želite ${dest.isActive ? 'deaktivirati' : 'aktivirati'} ovu destinaciju?`
      }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.destinationService
        .toggleStatus({ id: dest.id, isActive: !dest.isActive })
        .subscribe({
          next: (updated) => {
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

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.destinationService.delete(id).subscribe({
        next: () => {
          this.destinations = this.destinations.filter((d) => d.id !== id);
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
