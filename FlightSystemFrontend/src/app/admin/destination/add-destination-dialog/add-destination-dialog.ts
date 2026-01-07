import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CountryService, Country } from '../../../Services/country.service';
import { CityService, City } from '../../../Services/city.service';
import { AirportService, AirportDTO } from '../../../Services/airport.service';

@Component({
  selector: 'app-add-destination-dialog',
  standalone: false,
  templateUrl: './add-destination-dialog.html',
  styleUrls: ['./add-destination-dialog.css']
})
export class AddDestinationDialog implements OnInit {
  // Polazak
  fromCountries: Country[] = [];
  fromCities: City[] = [];
  fromAirports: AirportDTO[] = [];

  selectedFromCountryId: number | null = null;
  selectedFromCityId: number | null = null;
  selectedFromAirportId: number | null = null;

  // Dolazak
  toCountries: Country[] = [];
  toCities: City[] = [];
  toAirports: AirportDTO[] = [];

  selectedToCountryId: number | null = null;
  selectedToCityId: number | null = null;
  selectedToAirportId: number | null = null;

  isActive = true;

  constructor(
    private dialogRef: MatDialogRef<AddDestinationDialog>,
    private countryService: CountryService,
    private cityService: CityService,
    private airportService: AirportService,
    private cd: ChangeDetectorRef  // 🔥 DODANO
  ) {}

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries() {
    this.countryService.getAll().subscribe({
      next: (data) => {
        this.fromCountries = data;
        this.toCountries = data;
        this.cd.detectChanges();  // 🔥 DODANO - ručno trigguj change detection
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja država:', err);
      }
    });
  }

  // ========== POLAZAK CASCADE ==========
  onFromCountryChange() {
    this.selectedFromCityId = null;
    this.selectedFromAirportId = null;
    this.fromCities = [];
    this.fromAirports = [];

    if (!this.selectedFromCountryId) return;

    this.cityService.getByCountryId(this.selectedFromCountryId).subscribe({
      next: (data) => {
        this.fromCities = data;
        this.cd.detectChanges();  // 🔥 DODANO
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja gradova:', err);
      }
    });
  }

  onFromCityChange() {
    this.selectedFromAirportId = null;
    this.fromAirports = [];

    if (!this.selectedFromCityId) return;

    this.airportService.getByCityId(this.selectedFromCityId).subscribe({
      next: (data) => {
        this.fromAirports = data;
        this.cd.detectChanges();  // 🔥 DODANO
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja aerodroma:', err);
      }
    });
  }

  // ========== DOLAZAK CASCADE ==========
  onToCountryChange() {
    this.selectedToCityId = null;
    this.selectedToAirportId = null;
    this.toCities = [];
    this.toAirports = [];

    if (!this.selectedToCountryId) return;

    this.cityService.getByCountryId(this.selectedToCountryId).subscribe({
      next: (data) => {
        this.toCities = data;
        this.cd.detectChanges();  // 🔥 DODANO
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja gradova:', err);
      }
    });
  }

  onToCityChange() {
    this.selectedToAirportId = null;
    this.toAirports = [];

    if (!this.selectedToCityId) return;

    this.airportService.getByCityId(this.selectedToCityId).subscribe({
      next: (data) => {
        this.toAirports = data;
        this.cd.detectChanges();  // 🔥 DODANO
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja aerodroma:', err);
      }
    });
  }

  // ========== VALIDACIJA I SPREMANJE ==========
  save() {
    if (!this.selectedFromAirportId || !this.selectedToAirportId) {
      alert('Molimo odaberite oba aerodroma.');
      return;
    }

    if (this.selectedFromAirportId === this.selectedToAirportId) {
      alert('Polazni i dolazni aerodrom ne mogu biti isti.');
      return;
    }

    this.dialogRef.close({
      fromAirportId: this.selectedFromAirportId,
      toAirportId: this.selectedToAirportId,
      isActive: this.isActive
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}
