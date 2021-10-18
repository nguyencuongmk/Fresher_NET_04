import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent implements OnInit {

  @Input() productItem!: Product;
  @Output() addToCartEvent = new EventEmitter<Product>();


  constructor() { }

  ngOnInit(): void {
  }

  addToCart(){
    alert(`Đã thêm sản phẩm ${this.productItem.productName} vào giỏ hàng`);
    return this.addToCartEvent.emit(this.productItem);
  }
}
