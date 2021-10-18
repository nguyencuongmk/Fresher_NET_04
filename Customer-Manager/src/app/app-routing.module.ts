import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddCustomerComponent } from './components/add-customer/add-customer.component';
import { CustomerListComponent } from './components/customer-list/customer-list.component';
import { UpdateCustomerComponent } from './components/update-customer/update-customer.component';
import { ViewCustomerComponent } from './components/view-customer/view-customer.component';

const routes: Routes = [
  {path: '', component: CustomerListComponent},
  {path: 'edit/:id', component: UpdateCustomerComponent},
  {path: 'detail/:id', component: ViewCustomerComponent},
  {path: 'add', component: AddCustomerComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
