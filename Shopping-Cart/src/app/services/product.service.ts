import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

const apiUrl = "https://steelsoftware.azurewebsites.net/api/FresherFPT";


@Injectable({
  providedIn: 'root'
})

export class ProductService {


  products!: Product[];
  constructor(private http: HttpClient) { }

  getProducts() : Observable<Product[]>{
    return this.http.get<Product[]>(apiUrl);
  }
}
