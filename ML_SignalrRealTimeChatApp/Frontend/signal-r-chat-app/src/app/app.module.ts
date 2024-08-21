import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JoinChatLoginComponent } from './components/join-chat-login/join-chat-login.component';
import { ChatComponent } from './components/chat/chat.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [AppComponent, JoinChatLoginComponent, ChatComponent],
  imports: [BrowserModule, AppRoutingModule, FormsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
