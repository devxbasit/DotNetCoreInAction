import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private chatHubConnection?: signalR.HubConnection | null = null;

  getChatHubConnection() {
    if (!this.chatHubConnection) this.initChatHubConnection();

    return this.chatHubConnection;
  }

  initChatHubConnection(): boolean {
    if (this.chatHubConnection) return true;

    const conn = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl('http://localhost:5656/hubs/chat')
      .build();

    if (conn) {
      this.chatHubConnection = conn;
      return true;
    } else {
      return false;
    }
  }
}
