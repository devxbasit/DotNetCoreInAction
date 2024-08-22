export interface IEnvironment {
  apiBaseUrl: string;
}
export interface FoodItem {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
}

export interface Order {
  id: number;
  tableNumber: number;
  foodItemId: number;

  foodItem: FoodItem;
  orderDate: Date; // Using Date for DateTimeOffset
  orderState: OrderState;
}

export enum OrderState {
  Ordered = 'Ordered',
  Preparing = 'Preparing',
  AwaitingDelivery = 'AwaitingDelivery',
  Completed = 'Completed',
}

export interface FoodRequest {
  FoodItemId: number;
  TableNumber: number;
}
