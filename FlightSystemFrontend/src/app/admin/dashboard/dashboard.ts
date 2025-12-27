import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {DashboardService, DashboardStats} from '../../Services/dashboard-service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard  implements OnInit {

  stats?: DashboardStats;
  loading = true;
  error: string | null = null;

  constructor(private dashboardService: DashboardService,
              private cd: ChangeDetectorRef) {}

  ngOnInit(): void {

    this.loadStats();
  }

  private loadStats(): void {
    this.dashboardService.getStats().subscribe({
      next: (data) => {
        this.stats = data;
        this.loading = false;
        this.cd.detectChanges(); // 👈 ISTO KAO AIRPORTS
      },
      error: (err) => {
        console.error(err);
        this.error = 'Greška prilikom učitavanja dashboard podataka.';
        this.loading = false;
      }
    });
  }
}
