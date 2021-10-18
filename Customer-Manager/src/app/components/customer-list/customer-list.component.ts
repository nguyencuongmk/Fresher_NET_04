import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/models/customer';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {

  customerList!: Customer[];

  constructor(private customerService: CustomerService) { }

  ngOnInit(): void {
    this.getCustomers();
  }

  getCustomers() {
    this.customerService.getCustomers().subscribe(customers => {
      this.customerList = customers;
    })
  }

  deleteCustomer(id: string) {
    this.customerService.deleteCustomer(id).subscribe(res => {
      alert('The client has been successfully deleted');
      this.getCustomers();
    })
  }
}
