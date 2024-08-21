import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JoinChatLoginComponent } from './components/join-chat-login/join-chat-login.component';
import { ChatComponent } from './components/chat/chat.component';

const routes: Routes = [
  {
    path: '',
    component: JoinChatLoginComponent,
    pathMatch: 'full',
  },
  { path: 'chat', component: ChatComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
