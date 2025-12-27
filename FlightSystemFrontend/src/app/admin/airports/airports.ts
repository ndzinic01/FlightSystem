import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AirportAddUpdate, Airport, AirportService} from '../../Services/airport.service';

@Component({
  selector: 'app-airports',
  standalone: false,
  templateUrl: './airports.html',
  styleUrl: './airports.css',
})
export class Airports implements OnInit {

  airports: Airport[] = [];
  form!: FormGroup;
  selectedId: number | null = null;
  isLoading = false;
  cities = [
    { id: 1, name: 'Sarajevo' },
    { id: 2, name: 'Zagreb' },
    { id: 3, name: 'Istanbul' },
  ];

  constructor(
    private service: AirportService,
    private fb: FormBuilder,
    private cd: ChangeDetectorRef // 👈 DODANO
  ) {}

  ngOnInit(): void {
    console.log("AirportsComponent INIT");
    this.initForm();
    this.loadAirports();
  }

  loadAirports() {
    this.isLoading = true;

    this.service.getAll().subscribe({
      next: res => {
        this.airports = res;
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  initForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      cityId: ['', Validators.required],
      isActive: [true]
    });
  }

  edit(airport: Airport) {
    this.selectedId = airport.id;
    this.form.patchValue({
      name: airport.name,
      cityId: airport.cityId,
      isActive: airport.isActive
    });
  }

  delete(id: number) {
    if (!confirm('Obrisati aerodrom?')) return;
    this.service.delete(id).subscribe(() => this.loadAirports());
  }

  save() {
    const dto: AirportAddUpdate = this.form.value;

    const request = this.selectedId
      ? this.service.update(this.selectedId, dto)
      : this.service.create(dto);

    request.subscribe(() => {
      this.reset();
      this.loadAirports();
    });
  }

  reset() {
    this.selectedId = null;
    this.form.reset({ isActive: true });
  }

}
