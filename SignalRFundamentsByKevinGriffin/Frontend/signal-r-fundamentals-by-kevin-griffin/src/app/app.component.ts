import { Component, inject, OnInit } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  toastrService = inject(ToastrService);
  isNotifyAdminNotificationOn = false;
  ngModel = {
    name: '',
  };

  private _hubConnection: HubConnection;

  constructor() {
    this._hubConnection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Debug)
      .withUrl('http://localhost:8585/hubs/sync')
      .build();

    this._hubConnection.start();

    this._hubConnection.on('SyncTextBox', (value) => {
      this.ngModel.name = value;
    });

    this._hubConnection.on('newAdminNotification', (notification: string) => {
      console.log(notification);
      this.toastrService.success(notification);
    });
  }

  ngOnInit(): void {}

  onSyncTextBox() {
    console.log(this.ngModel);
    this._hubConnection.invoke('SyncTextBox', this.ngModel.name);
  }

  onNotifyStateChange() {
    if (this.isNotifyAdminNotificationOn) {
      this._hubConnection.invoke('StopNotifyAdminNotification');
    } else {
      this._hubConnection.invoke('StartNotifyAdminNotification');
    }

    this.isNotifyAdminNotificationOn = !this.isNotifyAdminNotificationOn;
  }
}
