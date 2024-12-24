import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private apiUrl = '/api/review';

  constructor(private http: HttpClient) { }

  getReviewsByProductId(productId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${productId}/reviews`);
  }

  getAverageRatingByProductId(productId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${productId}/average-rating`);
  }
}
