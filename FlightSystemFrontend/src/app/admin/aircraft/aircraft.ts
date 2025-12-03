import { Component, ChangeDetectorRef } from '@angular/core';
import { AircraftService, AircraftGetDTO, AircraftAddUpdateDTO } from '../../Services/aircraft.service';

@Component({
  selector: 'app-aircraft',
  standalone: false,
  templateUrl: './aircraft.html',
  styleUrls: ['./aircraft.css']
})
export class Aircraft {
  aircrafts: AircraftGetDTO[] = [];
  loading: boolean = false;
  showModal: boolean = false;
  isEditMode: boolean = false;
  selectedAircraftId: number | null = null;
  error: string = '';


  formData: AircraftAddUpdateDTO = {
    model: '',
    registrationNumber: '',
    yearManufacturer: new Date().getFullYear(),
    manufacturer: '',
    capacity: 0,
    status: true
  };

  constructor(
    private aircraftService: AircraftService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('ngOnInit pozvan - počinjem učitavanje');
    this.loadAircrafts();
  }

  loadAircrafts(): void {
    console.log('loadAircrafts pozvan');
    this.loading = true;
    this.error = '';
    this.aircrafts = [];
    this.cdr.detectChanges(); // Force update prije HTTP poziva

    this.aircraftService.getAll().subscribe({
      next: (data) => {
        console.log('Učitani avioni:', data);
        this.aircrafts = data;
        this.loading = false;
        console.log('Loading postavljen na false');
        this.cdr.detectChanges(); // Force update nakon učitavanja
      },
      error: (err) => {
        console.error('Greška pri učitavanju aviona:', err);
        this.error = 'Greška pri učitavanju podataka. Provjerite da li backend radi.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openAddModal(): void {
    this.isEditMode = false;
    this.selectedAircraftId = null;
    this.resetForm();
    this.showModal = true;
  }

  openEditModal(aircraft: AircraftGetDTO): void {
    this.isEditMode = true;
    this.selectedAircraftId = aircraft.id;
    this.formData = {
      model: aircraft.model,
      registrationNumber: aircraft.registrationNumber,
      yearManufacturer: aircraft.yearManufacturer,
      manufacturer: aircraft.manufacturer,
      capacity: aircraft.capacity,
      status: aircraft.status
    };
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.resetForm();
  }

  resetForm(): void {
    this.formData = {
      model: '',
      registrationNumber: '',
      yearManufacturer: new Date().getFullYear(),
      manufacturer: '',
      capacity: 0,
      status: true
    };
  }

  onSubmit(): void {
    if (this.isEditMode && this.selectedAircraftId) {
      this.updateAircraft();
    } else {
      this.addAircraft();
    }
  }

  addAircraft(): void {
    this.aircraftService.create(this.formData).subscribe({
      next: (data) => {
        this.aircrafts.push(data);
        this.closeModal();
        this.cdr.detectChanges(); // Ažuriraj prikaz
        alert('Avion uspješno dodan!');
      },
      error: (err) => {
        console.error('Greška pri dodavanju:', err);
        alert('Greška pri dodavanju aviona!');
      }
    });
  }

  updateAircraft(): void {
    if (!this.selectedAircraftId) return;

    this.aircraftService.update(this.selectedAircraftId, this.formData).subscribe({
      next: (data) => {
        const index = this.aircrafts.findIndex(a => a.id === this.selectedAircraftId);
        if (index !== -1) {
          this.aircrafts[index] = data;
        }
        this.closeModal();
        this.cdr.detectChanges(); // Ažuriraj prikaz
        alert('Avion uspješno ažuriran!');
      },
      error: (err) => {
        console.error('Greška pri ažuriranju:', err);
        alert('Greška pri ažuriranju aviona!');
      }
    });
  }

  deleteAircraft(id: number): void {
    if (!confirm('Da li ste sigurni da želite obrisati ovaj avion?')) {
      return;
    }

    this.aircraftService.delete(id).subscribe({
      next: () => {
        this.aircrafts = this.aircrafts.filter(a => a.id !== id);
        this.cdr.detectChanges(); // Ažuriraj prikaz
        alert('Avion uspješno obrisan!');
      },
      error: (err) => {
        console.error('Greška pri brisanju:', err);
        alert('Greška pri brisanju aviona!');
      }
    });
  }
}
