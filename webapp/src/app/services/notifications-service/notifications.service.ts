import { MatSnackBar } from '@angular/material';
import { Injectable } from '@angular/core';

@Injectable()
export class NotificationsService {

  constructor(private snackBar: MatSnackBar) { }

  notification(message: string, submessage: string, error: boolean = false) {
    this.snackBar.open(message, submessage, { duration: 2000, extraClasses: (error ? ['color: red'] : ['']) });
  }

}
