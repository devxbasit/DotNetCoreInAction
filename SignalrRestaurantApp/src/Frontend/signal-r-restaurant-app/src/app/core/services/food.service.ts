import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FoodItem } from '../interfaces/core.interface';

@Injectable({
  providedIn: 'root',
})
export class FoodService {
  httpClient = inject(HttpClient);
  env = environment;

  food$: Observable<FoodItem[]> = this.httpClient.get<FoodItem[]>(
    `${this.env.apiBaseUrl}/api/food/available/all`
  );
}
