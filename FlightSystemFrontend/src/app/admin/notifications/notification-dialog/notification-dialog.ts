import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {NotificationDTO, NotificationService} from '../../../Services/notification-service';
import {SnackbarService} from '../../../Services/Notifications/snackbar-service';

@Component({
  selector: 'app-notification-dialog',
  standalone: false,
  templateUrl: './notification-dialog.html',
  styleUrl: './notification-dialog.css',
})
export class NotificationDialog {
  replyText = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: NotificationDTO,
    public dialogRef: MatDialogRef<NotificationDialog>, // 👈 PUBLIC
    private service: NotificationService,
    private snack: SnackbarService
  ) {}

  sendReply() {
    this.service.reply({
      id: this.data.id,
      reply: this.replyText
    }).subscribe({
      next: () => {
        this.snack.success('Odgovor poslan');
        this.dialogRef.close(true);
      },
      error: () => this.snack.error('Greška pri slanju odgovora')
    });
  }
}
