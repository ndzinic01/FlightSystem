import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NotificationDTO, NotificationService, NotificationType } from '../../Services/notification-service';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../Services/Notifications/snackbar-service';
import { NotificationDialog } from './notification-dialog/notification-dialog';
import { BroadcastDialog } from './broadcast-dialog/broadcast-dialog'; // 🔥 NOVI

@Component({
  selector: 'app-notifications',
  standalone: false,
  templateUrl: './notifications.html',
  styleUrl: './notifications.css',
})
export class Notifications implements OnInit {
  notifications: NotificationDTO[] = [];
  filtered: NotificationDTO[] = [];
  loading = false;

  // Filteri
  userSearch = '';
  notificationType: string = 'all'; // 🔥 PROMIJENJENO
  statusSearch = 'all';
  dateSearch: string | null = null;

  // Enum za template
  NotificationType = NotificationType;

  get totalCount(): number {
    return this.notifications.length;
  }

  get unreadCount(): number {
    return this.notifications.filter(n => !n.isRead).length;
  }

  // 🔥 NOVA - Sistemske notifikacije
  get systemNotificationsCount(): number {
    return this.notifications.filter(n => n.isSystemGenerated).length;
  }

  constructor(
    private service: NotificationService,
    private cdr: ChangeDetectorRef,
    private dialog: MatDialog,
    private snack: SnackbarService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (data) => {
        this.notifications = data;
        this.filtered = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.snack.error('Greška prilikom učitavanja notifikacija.');
      }
    });
  }

  applyFilters() {
    let result = [...this.notifications];

    // Filter po tipu notifikacije
    if (this.notificationType !== 'all') {
      if (this.notificationType === 'user') {
        result = result.filter(n => n.type === NotificationType.UserInquiry);
      } else if (this.notificationType === 'system') {
        result = result.filter(n => n.isSystemGenerated);
      }
    }

    // Filter po korisniku
    if (this.userSearch.trim()) {
      const term = this.userSearch.trim().toLowerCase();
      result = result.filter(
        (n) =>
          n.userFullName?.toLowerCase().includes(term) ||
          n.userEmail?.toLowerCase().includes(term)
      );
    }

    // Filter po statusu
    if (this.statusSearch !== 'all') {
      result = result.filter((n) =>
        this.statusSearch === 'new' ? !n.isRead : n.isRead
      );
    }

    // Filter po datumu
    if (this.dateSearch) {
      result = result.filter(
        (n) =>
          new Date(n.sentAt).toISOString().split('T')[0] === this.dateSearch
      );
    }

    this.filtered = result;
  }

  onNotificationTypeChange() {
    this.applyFilters();
  }

  onUserSearch() {
    this.applyFilters();
  }

  onStatusChange() {
    this.applyFilters();
  }

  onDateChange(event: any) {
    this.dateSearch = event.target.value;
    this.applyFilters();
  }

  clearDateFilter() {
    this.dateSearch = null;
    this.applyFilters();
  }

  // 🔥 NOVA METODA - Otvori broadcast dialog
  openBroadcastDialog() {
    const dialogRef = this.dialog.open(BroadcastDialog, {
      width: '600px',
      maxWidth: '90vw'
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.snack.success('Broadcast notifikacija je poslana.');
        this.load();
      }
    });
  }

  statusLabel(n: NotificationDTO): string {
    if (n.isSystemGenerated) {
      return this.getNotificationTypeLabel(n.type);
    }
    return n.isRead ? 'Odgovoreno' : 'Novo';
  }

  statusClass(n: NotificationDTO): string {
    if (n.isSystemGenerated) {
      return 'status-system';
    }
    return n.isRead ? 'status-replied' : 'status-new';
  }

  // 🔥 NOVA METODA - Labele za tipove
  getNotificationTypeLabel(type: NotificationType): string {
    const labels: Record<NotificationType, string> = {
      [NotificationType.UserInquiry]: 'Korisnički upit',
      [NotificationType.FlightDelay]: 'Zakašnjenje',
      [NotificationType.FlightCancellation]: 'Otkazan let',
      [NotificationType.FlightReschedule]: 'Promjena vremena',
      [NotificationType.SystemBroadcast]: 'Sistemska poruka'
    };
    return labels[type] || 'Nepoznato';
  }

  openDialog(notification: NotificationDTO) {
    const dialogRef = this.dialog.open(NotificationDialog, {
      width: '600px',
      maxWidth: '90vw',
      disableClose: false,
      data: notification
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.snack.success('Notifikacija je uspješno ažurirana.');
        this.load();
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }
}
