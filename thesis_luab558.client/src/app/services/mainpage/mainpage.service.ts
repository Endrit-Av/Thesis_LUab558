// src/app/services/mainpage/mainpage.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MainpageService {
  private apiUrl = '/api/mainpage';

  constructor(private http: HttpClient) { }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/categories`);
  }

  getProductsByCategory(category: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/products/${category}`);
  }

  getBannerImages(): Observable<string[]> {
    const url = 'https://localhost:7219/api/mainpage/banner-images';
    return this.http.get<string[]>(url);
    //return this.http.get<string[]>(`${this.apiUrl}/banner-images`);
  }

  //Diese Funktionen in einen anderen Service später umziehen
  getProductById(productId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/product/${productId}`);
  }

  getProductVariants(productName: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/product/variants/${productName}`);
  }

  getProductByAttributes(productName: string, color: string, ram: number, physicalMemory: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/product/details`, {
      params: { productName, color, ram: ram.toString(), physicalMemory: physicalMemory.toString() }
    });
  }
}
