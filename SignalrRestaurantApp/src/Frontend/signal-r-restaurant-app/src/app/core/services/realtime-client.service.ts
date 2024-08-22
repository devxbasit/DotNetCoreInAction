import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { FoodRequest, Order, OrderState } from '../interfaces/core.interface';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RealtimeClientService {
  private foodHubConnection: signalR.HubConnection;
  private pendingFoodUpdatedSubject: Subject<Order[]> = new Subject<Order[]>();
  ordersUpdated$: Observable<Order[]> =
    this.pendingFoodUpdatedSubject.asObservable();

  env = environment;

  constructor() {
    console.error('cont called onece', new Date());

    this.foodHubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl(`${this.env.apiBaseUrl}/hub/foodhub`)
      .build();

    this.foodHubConnection
      .start()
      .then(() => {
        console.log('Connected to signalR hub');
      })
      .catch((err) => {
        console.error('Error connecting to signal hub:', err);
      });

    this.foodHubConnection.on('PendingFoodUpdated', (orders: Order[]) => {
      this.pendingFoodUpdatedSubject.next(orders);
    });
  }

  async orderFoodItem(foodRequest: FoodRequest) {
    console.log('Ordering food! ', foodRequest);
    await this.foodHubConnection.invoke('OrderFoodItem', foodRequest);
  }

  async updateOrderStatus(orderId: number, state: OrderState) {
    console.log('Updating order status');
    await this.foodHubConnection.invoke('UpdateFoodItemOrder', orderId, state);
  }
}
