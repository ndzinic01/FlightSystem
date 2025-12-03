import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AirportAddUpdate, Airport, AirportService} from '../../Services/airport.service';

@Component({
  selector: 'app-airports',
  standalone: false,
  templateUrl: './airports.html',
  styleUrl: './airports.css',
})
export class Airports implements OnInit{

  airports: Airport[] = [];
  form!: FormGroup;
  selectedId: number | null = null;

  cities = [
    { id: 1, name: 'Sarajevo' },
    { id: 2, name: 'Zagreb' },
    { id: 3, name: 'Istanbul' },
  ]; // TODO: zamijeniti sa GET City kada završimo City CRUD

  constructor(private service: AirportService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.loadAirports();
    this.initForm();
  }

  loadAirports() {
    this.service.getAll().subscribe(res => {
      this.airports = res;
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
    if (!confirm('Delete airport?')) return;
    this.service.delete(id).subscribe(() => {
      this.loadAirports();
    });
  }

  save() {
    const dto: AirportAddUpdate = this.form.value;

    if (this.selectedId) {
      this.service.update(this.selectedId, dto).subscribe(() => {
        this.reset();
        this.loadAirports();
      });
    } else {
      this.service.create(dto).subscribe(() => {
        this.reset();
        this.loadAirports();
      });
    }
  }

  reset() {
    this.selectedId = null;
    this.form.reset({ isActive: true });
  }
}
