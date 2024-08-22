import { Component, inject, OnInit } from '@angular/core';
import { RealtimeClientService } from 'src/app/core/services/realtime-client.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  realtimeClientService = inject(RealtimeClientService);
  toastrService = inject(ToastrService);
  messages: { username: string; message: string }[] = [];

  ngOnInit(): void {
    this.realtimeClientService.GroupNotification$.subscribe((next) => {
      const [sender, notification] = next;
      this.toastrService.success(notification, sender);
    });

    this.realtimeClientService.GroupMessage$.subscribe((next) => {
      const [username, message] = next;

      this.messages.push({ username, message });
      this.toastrService.success(username, message);
    });
  }

  sendMessage(message: string): void {
    this.realtimeClientService.sendNewMessage(message);
  }
}
