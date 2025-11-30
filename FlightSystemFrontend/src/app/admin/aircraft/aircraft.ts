import {AfterViewInit, Component, OnInit} from '@angular/core';
import {AircraftGet, AircraftService} from '../../Services/aircraft.service';
import {Observable} from 'rxjs';


@Component({
  selector: 'app-aircraft',
  standalone: false,
  templateUrl: './aircraft.html',
  styleUrls: ['./aircraft.css']
})
export class Aircraft implements OnInit {
  aircraftList: AircraftGet[] = [];
  loading: boolean = true;
  aircrafts$!: Observable<AircraftGet[]>;

  constructor(private aircraftService: AircraftService) {}

  ngOnInit(): void {
    this.loadAircrafts();
    this.aircrafts$ = this.aircraftService.getAll();
  }

  loadAircrafts() {
    this.aircraftService.getAll().subscribe({
      next: (res) => {
        this.aircraftList = res;
        this.loading = false;
      },
      error: () => alert("Greška pri učitavanju aviona")
    });
  }
}
