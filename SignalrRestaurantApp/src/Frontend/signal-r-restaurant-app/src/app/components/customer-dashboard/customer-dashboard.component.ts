import { Component, inject, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { FoodItem, Order } from 'src/app/core/interfaces/core.interface';
import { FoodService } from 'src/app/core/services/food.service';
import { OrderService } from 'src/app/core/services/order.service';
import { RealtimeClientService } from 'src/app/core/services/realtime-client.service';

@Component({
  selector: 'app-customer-dashboard',
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.css'],
})
export class CustomerDashboardComponent implements OnInit {
  realtimeClientService = inject(RealtimeClientService);
  foodService = inject(FoodService);
  orderService = inject(OrderService);

  availableFood: FoodItem[] = [];
  customerActiveOrders: Order[] = [];
  tableNumber = Math.floor(Math.random() * (1000 - 100) + 100);

  ngOnInit(): void {
    this.foodService.food$.pipe(first()).subscribe((availableFood) => {
      this.availableFood = availableFood;
    });

    this.orderService.existingOrders$
      .pipe(first())
      .subscribe((existingOrders) => {
        this.customerActiveOrders = existingOrders.filter(
          (x) => x.tableNumber === this.tableNumber
        );
      });

    this.realtimeClientService.ordersUpdated$.subscribe((orders) => {
      this.customerActiveOrders = orders.filter(
        (x) => x.tableNumber === this.tableNumber
      );
    });
  }

  orderFoodItem(foodItemsId: number, tableNumber: number) {
    this.realtimeClientService.orderFoodItem({
      FoodItemId: foodItemsId,
      TableNumber: tableNumber,
    });
  }
}
