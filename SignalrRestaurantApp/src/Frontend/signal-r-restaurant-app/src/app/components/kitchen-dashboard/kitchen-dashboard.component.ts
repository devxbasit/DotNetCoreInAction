import { Component, inject, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { Order, OrderState } from 'src/app/core/interfaces/core.interface';
import { OrderService } from 'src/app/core/services/order.service';
import { RealtimeClientService } from 'src/app/core/services/realtime-client.service';

@Component({
  selector: 'app-kitchen-dashboard',
  templateUrl: './kitchen-dashboard.component.html',
  styleUrls: ['./kitchen-dashboard.component.css'],
})
export class KitchenDashboardComponent implements OnInit {
  realtimeClientService = inject(RealtimeClientService);
  orderService = inject(OrderService);
  orderState = OrderState;

  activeOrders: Order[] = [];

  ngOnInit(): void {
    this.orderService.existingOrders$.pipe(first()).subscribe((orders) => {
      this.activeOrders = orders;

      console.log(orders);
    });

    this.realtimeClientService.ordersUpdated$.subscribe((orders) => {
      this.activeOrders = orders;
    });
  }

  updateOrderStatus(orderId: number, event: Event) {
    const newState = (event.target as HTMLSelectElement)?.value as OrderState;

    this.realtimeClientService.updateOrderStatus(orderId, newState);
  }
}
