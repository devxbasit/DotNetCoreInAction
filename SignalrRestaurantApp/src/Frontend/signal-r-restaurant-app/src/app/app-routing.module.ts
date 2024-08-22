import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerDashboardComponent } from './components/customer-dashboard/customer-dashboard.component';
import { KitchenDashboardComponent } from './components/kitchen-dashboard/kitchen-dashboard.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'customer/dashboard',
    pathMatch: 'full',
  },
  { path: 'customer/dashboard', component: CustomerDashboardComponent },
  { path: 'kitchen/dashboard', component: KitchenDashboardComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
