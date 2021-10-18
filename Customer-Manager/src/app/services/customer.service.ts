import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  customers!: Customer[];

  constructor(private httpClient: HttpClient) { }

  getCustomers(): Observable<Customer[]> {
    const apiUrl = "https://localhost:44309/api/Customers";
    return this.httpClient.get<Customer[]>(apiUrl);
  }  

  updateCustomer(customer: Customer) {
    const apiUrl = "https://localhost:44309/api/Customers";
    var option = {
        headers: new HttpHeaders({
          'Content-Type':'application/json'
        }),
        observe: 'response' as const
    }
    return this.httpClient.put(apiUrl, customer, option);
  }

  getCustomer(id: string){
    const apiUrl = `https://localhost:44309/api/Customers/${id}`;
    return this.httpClient.get<Customer>(apiUrl);
  }

  deleteCustomer(id: string){
    const apiUrl = `https://localhost:44309/api/Customers/${id}`;
    return this.httpClient.delete(apiUrl, {observe: 'body'});
  }

  addCustomer(customer: any) : Observable<HttpResponse<any>>{
    const apiUrl = "https://localhost:44309/api/Customers";
    var option = {
      headers: new HttpHeaders({
        'Content-Type':'application/json'
      }),
      observe: 'response' as const
  }
  return this.httpClient.post<HttpResponse<any>>(apiUrl, customer, option);
  }
}
