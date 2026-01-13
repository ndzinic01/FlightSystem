import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DestinationService, DestinationDTO } from '../../../Services/destination-service';
import { AirlineService, AirlineDTO} from '../../../Services/airline-service';
import { AircraftService, AircraftDTO} from '../../../Services/aircraft.service';

@Component({
  selector: 'app-add-flight-dialog',
  standalone: false,
  templateUrl: './add-flight-dialog.html',
  styleUrls: ['./add-flight-dialog.css']
})
export class AddFlightDialog implements OnInit {
  // Dropdown liste
  flightTypes: string[] = ['Direktan', 'Sa presjedanjem'];
  destinations: DestinationDTO[] = [];
  airlines: AirlineDTO[] = [];
  aircrafts: AircraftDTO[] = [];

  // Form data
  selectedFlightType = 'Direktan';
  selectedDestinationId: number | null = null;
  departureDate: string = '';
  departureTime: string = '00:00';
  arrivalTime: string = '00:00';
  selectedAirlineId: number | null = null;
  selectedAircraftId: number | null = null;
  flightCode = '';
  price: number | null = null;
  availableSeats: number | null = null;
  isActive = true;

  constructor(
    private dialogRef: MatDialogRef<AddFlightDialog>,
    private destinationService: DestinationService,
    private airlineService: AirlineService,
    private aircraftService: AircraftService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDestinations();
    this.loadAirlines();
    this.loadAircrafts();
  }

  loadDestinations() {
    this.destinationService.getAll().subscribe({
      next: (data) => {
        this.destinations = data;
        this.cd.detectChanges();
      },
      error: (err) => console.error('Greška pri učitavanju destinacija:', err)
    });
  }

  loadAirlines() {
    this.airlineService.getAll().subscribe({
      next: (data) => {
        this.airlines = data;
        this.cd.detectChanges();
      },
      error: (err) => console.error('Greška pri učitavanju aviokompanija:', err)
    });
  }

  loadAircrafts() {
    this.aircraftService.getAll().subscribe({
      next: (data) => {
        // Filtriraj samo aktivne avione
        this.aircrafts = data.filter(a => a.status);
        this.cd.detectChanges();
      },
      error: (err) => console.error('Greška pri učitavanju aviona:', err)
    });
  }

  save() {
    // Validacija
    if (!this.selectedDestinationId) {
      alert('Molimo odaberite destinaciju.');
      return;
    }

    if (!this.departureDate) {
      alert('Molimo unesite datum leta.');
      return;
    }

    if (!this.selectedAirlineId) {
      alert('Molimo odaberite aviokompaniju.');
      return;
    }

    if (!this.flightCode || !this.price || !this.availableSeats) {
      alert('Molimo popunite sva obavezna polja.');
      return;
    }

    // Kombinuj datum i vrijeme
    const departureDateTime = `${this.departureDate}T${this.departureTime}:00`;
    const arrivalDateTime = `${this.departureDate}T${this.arrivalTime}:00`;

    // Provjeri da li je vrijeme dolaska nakon polaska
    if (new Date(arrivalDateTime) <= new Date(departureDateTime)) {
      alert('Vrijeme dolaska mora biti nakon vremena polaska.');
      return;
    }

    // Kreiraj DTO
    const flightData = {
      code: this.flightCode,
      destinationId: this.selectedDestinationId,
      airlineId: this.selectedAirlineId,
      aircraftId: this.selectedAircraftId || this.aircrafts[0]?.id || 1, // Prvi avion ako nije odabran
      departureTime: departureDateTime,
      arrivalTime: arrivalDateTime,
      status: this.isActive ? 0 : 3, // 0 = Scheduled, 3 = Cancelled
      price: this.price,
      availableSeats: this.availableSeats
    };

    this.dialogRef.close(flightData);
  }

  cancel() {
    this.dialogRef.close();
  }
}

