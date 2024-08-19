import { Component, OnInit } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
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
      console.log('value', value);
    });
  }

  ngOnInit(): void {}

  onSyncTextBox() {
    console.log(this.ngModel);
    this._hubConnection.invoke('SyncTextBox', this.ngModel.name);
  }
}
