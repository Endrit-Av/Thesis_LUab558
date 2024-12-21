// src/app/services/mainpage.service.ts
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
}