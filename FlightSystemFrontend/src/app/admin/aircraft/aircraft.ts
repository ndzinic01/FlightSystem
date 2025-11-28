import { Component } from '@angular/core';
import {AircraftService} from './aircraft.service';

@Component({
  selector: 'app-aircraft',
  standalone: false,
  templateUrl: './aircraft.html',
  styleUrl: './aircraft.css',
})
export class Aircraft {
  aircraftList: any[] = [];

  constructor(private aircraftService: AircraftService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.aircraftService.getAll().subscribe({
      next: res => this.aircraftList = res,
      error: err => console.error(err)
    });
  }
}
