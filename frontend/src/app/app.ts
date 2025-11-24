import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Footer } from "./shared/components/footer/footer";
import { Navbar } from "./shared/components/navbar/navbar";
import { NotificationHub } from './core/services/notification-hub';
import { MessageHub } from './core/services/message-hub';
import { NotificationStoreService } from './core/services/notification-store';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
   protected readonly title = signal('frontend');
   constructor(private hub: NotificationHub, private store: NotificationStoreService, private messageHub: MessageHub, private auth: AuthService) {}

ngOnInit() {
  // Start hubs and load notifications when user becomes authenticated
  this.auth.isAuthenticated$.subscribe(isAuth => {
    if (isAuth) {
      this.hub.startConnection();
      this.messageHub.startConnection();
      this.store.loadInitial();
    }
  });
}

}
