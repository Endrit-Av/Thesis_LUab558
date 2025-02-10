import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = '/api/product';

  constructor(private http: HttpClient) { }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/categories`);
  }

  getProductsByCategory(category: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${category}`);
  }

  getProductVariants(productName: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/variants/${productName}`);
  }

  getProductByAttributes(productName: string, color: string, ram: number, physicalMemory: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/details`, {
      params: { productName, color, ram: ram.toString(), physicalMemory: physicalMemory.toString() }
    });
  }
}
