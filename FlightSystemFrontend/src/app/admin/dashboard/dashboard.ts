import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {DashboardService, DashboardStats} from '../../Services/dashboard-service';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);

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
  chart: Chart | null = null;

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
        this.renderMonthlyReservationsChart();
        this.cd.detectChanges(); // 👈 ISTO KAO AIRPORTS
      },
      error: (err) => {
        console.error(err);
        this.error = 'Greška prilikom učitavanja dashboard podataka.';
        this.loading = false;
      }
    });
  }
  private renderMonthlyReservationsChart(): void {
    if (!this.stats?.monthlyReservations) return;

    const labels = Array.from({length: new Date().getDate()}, (_, i) => i + 1);
    const data = labels.map(day => this.stats?.monthlyReservations[day] || 0);

    const ctx = (document.getElementById('monthlyReservationsChart') as HTMLCanvasElement).getContext('2d');
    if (ctx) {
      this.chart = new Chart(ctx, {
        type: 'line',
        data: {
          labels,
          datasets: [{
            label: 'Rezervacije po danu',
            data,
            backgroundColor: '#1e5aa8'
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { display: false },
          },
          scales: {
            y: { beginAtZero: true }
          }
        }
      });
    }
  }
}
