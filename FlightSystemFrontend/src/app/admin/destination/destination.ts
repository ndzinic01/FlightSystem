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
        this.cd.detectChanges(); // 👈 ISTO KAO AIRPORTS
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  delete(id: number) {
    if (!confirm('Da li ste sigurni?')) return;

    this.destinationService.delete(id).subscribe(() => {
      this.loadDestinations();
    });
  }
}
