import { Component, inject } from '@angular/core';
import { IUserConnection } from 'src/app/interface/iuser-connection.interface';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-join-chat-login',
  templateUrl: './join-chat-login.component.html',
  styleUrls: ['./join-chat-login.component.css'],
})
export class JoinChatLoginComponent {
  ngModel: IUserConnection = {
    username: '',
    chatRoom: '',
  };

  signalRService = inject(SignalrService);

  onJoinChat() {
    const chatHubConnection = this.signalRService.getChatHubConnection();

    if (chatHubConnection) {
      chatHubConnection.start().then(() => {
        chatHubConnection.invoke('JoinSpecificChatRoom', this.ngModel);
      });
    }
  }
}
