import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../interfaces/core.interface';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  httpClient = inject(HttpClient);
  env = environment;

  existingOrders$: Observable<Order[]> = this.httpClient.get<Order[]>(
    `${this.env.apiBaseUrl}/api/kitchen/order/existing`
  );
}
