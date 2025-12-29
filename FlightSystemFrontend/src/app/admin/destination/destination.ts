import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {DestinationService, DestinationDTO, DestinationUpdateDTO} from '../../Services/destination-service';
import {FlightService} from '../../Services/flight-service';

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
              private flightService: FlightService) {}

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
    const confirmEdit = confirm(
      `Da li želite ${dest.isActive ? 'deaktivirati' : 'aktivirati'} ovu destinaciju?`
    );
    if (!confirmEdit) return;

    this.destinationService.toggleStatus({ id: dest.id, isActive: !dest.isActive }).subscribe({
      next: updated => {
        dest.isActive = updated.isActive;
        this.cd.detectChanges();
      },
      error: err => console.error(err)
    });
  }


  deleteDestination(id: number) {
    const confirmDelete = confirm('Da li ste sigurni da želite obrisati destinaciju?');
    if (!confirmDelete) return;

    this.destinationService.delete(id).subscribe({
      next: () => {
        this.destinations = this.destinations.filter(d => d.id !== id);
        this.applyFilter();
      },
      error: err => console.error(err)
    });
  }

}
