import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DestinationService, DestinationDTO } from '../../../Services/destination-service';
import { AirlineService, AirlineDTO} from '../../../Services/airline-service';
import { AircraftService, AircraftDTO} from '../../../Services/aircraft.service';
import { SnackbarService } from '../../../Services/Notifications/snackbar-service';

@Component({
  selector: 'app-add-flight-dialog',
  standalone: false,
  templateUrl: './add-flight-dialog.html',
  styleUrls: ['./add-flight-dialog.css']
})
export class AddFlightDialog implements OnInit {
  destinations: DestinationDTO[] = [];
  airlines: AirlineDTO[] = [];
  aircrafts: AircraftDTO[] = [];

  selectedDestinationId: number | null = null;
  selectedAirlineId: number | null = null;
  selectedAircraftId: number | null = null;
  selectedAircraftCapacity: number | null = null;

  departureDate = '';
  departureTime = '00:00';
  arrivalTime = '00:00';

  flightCode = '';
  price: number | null = null;
  availableSeats: number | null = null;

  constructor(
    private dialogRef: MatDialogRef<AddFlightDialog>,
    private destinationService: DestinationService,
    private airlineService: AirlineService,
    private aircraftService: AircraftService,
    private snack: SnackbarService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDestinations();
    this.loadAirlines();
    this.loadAircrafts();
  }

  loadDestinations() {
    this.destinationService.getAll().subscribe({
      next: data => {
        this.destinations = data;
        this.cd.detectChanges();
      },
      error: () => this.snack.error('Greška pri učitavanju destinacija.')
    });
  }

  loadAirlines() {
    this.airlineService.getAll().subscribe({
      next: data => {
        this.airlines = data;
        this.cd.detectChanges();
      },
      error: () => this.snack.error('Greška pri učitavanju aviokompanija.')
    });
  }

  loadAircrafts() {
    this.aircraftService.getAll().subscribe({
      next: data => {
        this.aircrafts = data.filter(a => a.status);
        this.cd.detectChanges();
      },
      error: () => this.snack.error('Greška pri učitavanju aviona.')
    });
  }

  save() {
    if (!this.selectedDestinationId) {
      this.snack.error('Molimo odaberite destinaciju.');
      return;
    }

    if (!this.departureDate) {
      this.snack.error('Molimo unesite datum leta.');
      return;
    }

    if (!this.selectedAirlineId) {
      this.snack.error('Molimo odaberite aviokompaniju.');
      return;
    }

    if (!this.selectedAircraftId) {
      this.snack.error('Molimo odaberite avion.');
      return;
    }

    if (!this.flightCode || this.price == null) {
      this.snack.error('Molimo popunite sva obavezna polja.');
      return;
    }

    const departureDateTime = `${this.departureDate}T${this.departureTime}:00`;
    const arrivalDateTime = `${this.departureDate}T${this.arrivalTime}:00`;

    if (new Date(arrivalDateTime) <= new Date(departureDateTime)) {
      this.snack.error('Vrijeme dolaska mora biti nakon vremena polaska.');
      return;
    }

    const flightData = {
      code: this.flightCode,
      destinationId: this.selectedDestinationId,
      airlineId: this.selectedAirlineId,
      aircraftId: this.selectedAircraftId,
      departureTime: departureDateTime,
      arrivalTime: arrivalDateTime,
      status: 0, // Scheduled
      price: this.price,
      availableSeats: this.selectedAircraftCapacity
    };

    this.dialogRef.close(flightData);
  }

  cancel() {
    this.dialogRef.close();
  }
  onAircraftChange() {
    const aircraft = this.aircrafts.find(
      a => a.id === this.selectedAircraftId
    );

    this.selectedAircraftCapacity = aircraft
      ? aircraft.capacity
      : null;
  }
}

