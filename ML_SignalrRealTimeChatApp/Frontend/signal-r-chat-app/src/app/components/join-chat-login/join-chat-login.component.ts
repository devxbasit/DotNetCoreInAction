import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { IUserConnection } from 'src/app/core/interfaces/core.interface';
import { RealtimeClientService } from 'src/app/core/services/realtime-client.service';

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

  realtimeClient = inject(RealtimeClientService);
  router = inject(Router);

  async onJoinChat() {
    const isJoinedChatRoom = await this.realtimeClient.joinChatRoom(
      this.ngModel
    );

    if (isJoinedChatRoom) {
      this.router.navigateByUrl('/chat');
    }
  }
}
