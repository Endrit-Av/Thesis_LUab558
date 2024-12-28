import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private apiUrl = '/api/image';

  constructor(private http: HttpClient) { }

  getImagesByProductId(productId: number): Observable<any[]> {              //Bei Cleanup entfernen
    return this.http.get<any[]>(`${this.apiUrl}/product/${productId}`);
  }

  getImagesByAttributes(productName: string, color: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/attributes?productName=${productName}&color=${color}`);
  }

  getBannerImages(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/banner-images`);
  }
}
