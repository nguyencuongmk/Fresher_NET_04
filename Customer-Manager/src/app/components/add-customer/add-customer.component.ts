import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-add-customer',
  templateUrl: './add-customer.component.html',
  styleUrls: ['./add-customer.component.css']
})
export class AddCustomerComponent implements OnInit {

  addCustomerForm!: FormGroup;

  constructor(private fb: FormBuilder, private customerService: CustomerService, private router: Router) { }

  ngOnInit(): void {
    this.addCustomerForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      avatar: ['', Validators.required],
      age: [''],
      address: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      city: ['', [Validators.required, Validators.minLength(3)]]
    })
  }

  onSubmit() {
    this.customerService.addCustomer(this.addCustomerForm.value).subscribe(res => {
      if (res.status == 200) {
        alert('A new client has been successfully added');
        this.router.navigateByUrl("/");
      }
    })
  }
}
