import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-update-customer',
  templateUrl: './update-customer.component.html',
  styleUrls: ['./update-customer.component.css']
})
export class UpdateCustomerComponent implements OnInit {
  
  updateCustomerForm!: FormGroup;
  customerId: string = '';

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private customerService: CustomerService) { }

  ngOnInit(): void {
    this.updateCustomerForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      avatar: [''],
      age: [''],
      address: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      city: ['', [Validators.required, Validators.minLength(3)]]
    })

    this.customerId = this.route.snapshot.params['id'];

    this.getCustomerById();
  }

  getCustomerById() {
    this.customerService.getCustomer(this.customerId).subscribe(res => {
      this.updateCustomerForm.controls.id.setValue(res.id);
      this.updateCustomerForm.controls.firstName.setValue(res.firstName);
      this.updateCustomerForm.controls.lastName.setValue(res.lastName);
      this.updateCustomerForm.controls.avatar.setValue(res.avatar);
      this.updateCustomerForm.controls.age.setValue(res.age);
      this.updateCustomerForm.controls.address.setValue(res.address);
      this.updateCustomerForm.controls.city.setValue(res.city);
    });
  }

  submitUpdate(){
    this.customerService.updateCustomer(this.updateCustomerForm.value).subscribe(res => {
      if (res.status==200) {
        alert("The client has been successfully updated");
        this.router.navigateByUrl("/");
      }
    })
  }
}
