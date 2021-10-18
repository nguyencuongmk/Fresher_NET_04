import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-view-customer',
  templateUrl: './view-customer.component.html',
  styleUrls: ['./view-customer.component.css']
})
export class ViewCustomerComponent implements OnInit {

  viewCustomerForm!: FormGroup;
  customerId: string = '';

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private customerService: CustomerService) { }

  ngOnInit(): void {
    this.viewCustomerForm = this.fb.group({
      id: [''],
      firstName: [''],
      lastName: [''],
      avatar: [''],
      age: [''],
      address: [''],
      city: ['']
    })

    this.customerId = this.route.snapshot.params['id'];

    this.getCustomerById();
  }

  getCustomerById() {
    this.customerService.getCustomer(this.customerId).subscribe(res => {
      this.viewCustomerForm.controls.id.setValue(res.id);
      this.viewCustomerForm.controls.firstName.setValue(res.firstName);
      this.viewCustomerForm.controls.lastName.setValue(res.lastName);
      this.viewCustomerForm.controls.avatar.setValue(res.avatar);
      this.viewCustomerForm.controls.age.setValue(res.age);
      this.viewCustomerForm.controls.address.setValue(res.address);
      this.viewCustomerForm.controls.city.setValue(res.city);
    });
  }
}
