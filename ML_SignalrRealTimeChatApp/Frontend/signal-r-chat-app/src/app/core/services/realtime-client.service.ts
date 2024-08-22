import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUserConnection } from '../interfaces/core.interface';

@Injectable({
  providedIn: 'root',
})
export class RealtimeClientService {
  private chatHubConnection: signalR.HubConnection;
  env = environment;
  private newGroupNotificationSubject: Subject<[string, string]> = new Subject<
    [string, string]
  >();
  private newGroupMessageSubject: Subject<[string, string]> = new Subject<
    [string, string]
  >();

  GroupNotification$ = this.newGroupNotificationSubject.asObservable();
  GroupMessage$ = this.newGroupMessageSubject.asObservable();

  constructor() {
    this.chatHubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl(`${this.env.apiBaseUlr}/hub/chathub`)
      .build();

    this.chatHubConnection.on(
      'NewGroupNotification',
      (sender, notification) => {
        this.newGroupNotificationSubject.next([sender, notification]);
      }
    );

    this.chatHubConnection.on('NewGroupMessage', (username, message) => {
      this.newGroupMessageSubject.next([username, message]);
    });
  }

  async joinChatRoom(userConnection: IUserConnection): Promise<boolean> {
    await this.chatHubConnection.start().then(() => {
      console.log('Successfully connected to chathub');
    });

    await this.chatHubConnection.invoke('JoinSpecificChatRoom', userConnection);

    return true;
  }

  async sendNewMessage(message: string): Promise<void> {
    this.chatHubConnection.invoke('SendMessage', message);
  }
}
