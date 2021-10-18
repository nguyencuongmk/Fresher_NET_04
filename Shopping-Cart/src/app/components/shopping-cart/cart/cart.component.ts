import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CartItem, Product } from 'src/app/models/product';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  cartItems: CartItem[] = [];
  formPayment!: FormGroup;
  isSuccess: boolean = false;

  constructor(private cartService: CartService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.formPayment = this.fb.group({
      fullName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      products: this.fb.array([])
    })
    this.getCartItems();
  }

  getCartItems() {
    this.cartItems = this.cartService.getCartItems();
  }

  grandTotal() {
    var total: number = 0;
    for (let i = 0; i < this.cartItems.length; i++) {
      var actualPrice = this.cartItems[i].product.price - this.cartItems[i].product.price * (this.cartItems[i].product.promotionPrice / 100);
      total += this.cartItems[i].quantity * actualPrice;
    }
    return total;
  }

  deleteCartItem(item: CartItem) {
    return this.cartService.deleteCartItem(item);
  }

  addUserProducts() {
    var products = this.formPayment.get('products') as FormArray;
    products?.reset();
    this.cartItems.forEach
      (
        x => {
          x.product.quantity = x.quantity;
          products.push(new FormControl(x.product));
        }
      )
  }

  onSubmit(): void {
    this.addUserProducts();
    this.cartService.submitForm(this.formPayment.value).subscribe(res => {
      console.log(res);
      if (res.status == 200) {
        this.isSuccess = true;
        alert("Đơn hàng đã được gửi");
      }
    });
  }
}
