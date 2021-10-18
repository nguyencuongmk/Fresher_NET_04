import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CartItem, Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private static cartItems: CartItem[] = [];
  constructor(private http: HttpClient) { }

  addToCart(product: Product) {
    var itemExisting = this.getCartItems().find(x => x.product.id == product.id);
    if (itemExisting == undefined) {
        CartService.cartItems.push({
          product: product, quantity: 1
        })
    }
    else
      itemExisting.quantity++;
  }

  getCartItems() {
    return CartService.cartItems;
  }

  deleteCartItem(cartItem: CartItem){
    const index = this.getCartItems().findIndex(item => item.product.id === cartItem.product.id);
    this.getCartItems().splice(index, 1);
  }

  submitForm(form: any){
    const apiUrl = "https://steelsoftware.azurewebsites.net/api/FresherFPT/CheckOut";
    const option={
      headers: new HttpHeaders({
        'Content-Type':'application/json'
      }),
      observe: 'response' as const
    }
    return this.http.post(apiUrl, form, option);
  }
}
