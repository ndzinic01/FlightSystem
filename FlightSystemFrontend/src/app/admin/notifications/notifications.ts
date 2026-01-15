import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {NotificationDTO, NotificationService} from '../../Services/notification-service';
import {MatDialog} from '@angular/material/dialog';
import {SnackbarService} from '../../Services/Notifications/snackbar-service';
import {NotificationDialog} from './notification-dialog/notification-dialog';

@Component({
  selector: 'app-notifications',
  standalone: false,
  templateUrl: './notifications.html',
  styleUrl: './notifications.css',
})
export class Notifications implements OnInit {

  notifications: NotificationDTO[] = [];
  filtered: NotificationDTO[] = [];

  userSearch = '';
  statusSearch = '';
  dateSearch: string | null = null;

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
    this.service.getAll().subscribe(data => {
      this.notifications = data;
      this.filtered = data;
      this.cdr.detectChanges();
    });
  }

  applyFilters() {
    let result = [...this.notifications];

    if (this.userSearch) {
      const term = this.userSearch.toLowerCase();
      result = result.filter(n =>
        n.userFullName.toLowerCase().includes(term)
      );
    }

    if (this.statusSearch) {
      result = result.filter(n =>
        this.statusSearch === 'new' ? !n.isRead : n.isRead
      );
    }

    if (this.dateSearch) {
      result = result.filter(n =>
        new Date(n.sentAt).toISOString().split('T')[0] === this.dateSearch
      );
    }

    this.filtered = result;
  }

  clearDate() {
    this.dateSearch = null;
    this.applyFilters();
  }

  actionLabel(n: NotificationDTO): string {
    return n.isRead ? 'Pregled' : 'Odgovori';
  }

  statusLabel(n: NotificationDTO): string {
    return n.isRead ? 'Odgovoreno' : 'Novo';
  }
  openDialog(notification: NotificationDTO) {
    const dialogRef = this.dialog.open(NotificationDialog, {
      width: '500px',
      maxWidth: '90vw',
      data: notification
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.load();
    });
  }
}
