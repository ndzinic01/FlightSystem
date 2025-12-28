import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {DestinationService, DestinationDTO} from '../../Services/destination-service';

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

  constructor(private destinationService: DestinationService,
              private cd: ChangeDetectorRef) {}

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
}
