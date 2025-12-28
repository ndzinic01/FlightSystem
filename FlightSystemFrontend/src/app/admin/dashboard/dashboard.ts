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

    const now = new Date();
    const daysInMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();

    const labels = Array.from({ length: daysInMonth }, (_, i) => `${i + 1}`);
    const data = labels.map(day =>
      this.stats?.monthlyReservations[Number(day)] || 0
    );

    const ctx = (document.getElementById('monthlyReservationsChart') as HTMLCanvasElement)
      ?.getContext('2d');

    if (!ctx) return;

    if (this.chart) {
      this.chart.destroy();
    }

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels,
        datasets: [
          {
            label: 'Rezervacije',
            data,
            borderColor: '#3b82f6',              // plava linija
            backgroundColor: 'rgba(59,130,246,0.1)', // blagi fill
            borderWidth: 2,
            fill: true,
            tension: 0.4,                        // glatka linija
            pointRadius: 0,                      // 👈 NEMA TAČKICA
            pointHoverRadius: 4
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false                      // 👈 nema legendu (kao na slici)
          },
          tooltip: {
            mode: 'index',
            intersect: false
          }
        },
        scales: {
          x: {
            grid: {
              display: false                    // 👈 nema vertikalnih linija
            }
          },
          y: {
            beginAtZero: true,
            ticks: {
              stepSize: 10
            },
            grid: {
              color: '#e5e7eb'                  // blage horizontalne linije
            }
          }
        }
      }
    });
  }

}
