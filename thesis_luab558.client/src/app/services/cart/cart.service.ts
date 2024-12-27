import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private apiUrl = '/api/cart';
  private cartCount = new BehaviorSubject<number>(0); // Artikelanzahl im Warenkorb
  cartCount$ = this.cartCount.asObservable();

  constructor(private http: HttpClient) {
    this.updateCartCountOnInit();
  }

  addToCart(productId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/add?productId=${productId}`, {});
  }

  getCartItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  removeFromCart(productId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/remove?productId=${productId}`);
  }

  increaseQuantity(productId: number): Observable<void> { // Keine Funktion gerade --> beim cleanup löschen
    return this.http.post<void>(`${this.apiUrl}/add?productId=${productId}`, {});
  }

  decreaseQuantity(productId: number): Observable<void> { // Keine Funktion gerade --> beim cleanup löschen
    return this.http.post<void>(`${this.apiUrl}/add?productId=${productId}`, {});
    return this.http.put<void>(`${this.apiUrl}/decrease?productId=${productId}`, {});
  }

  //Header zähler für den Warenkorb, kann nicht über Komponenten geteilt werden
  //Komponenten sind keine Services und können daher nicht als Dependency Injection Provider verwendet werden. Stattdessen sollten Daten über Services geteilt werden.
  updateCartCount(): void {
    this.getCartItems().subscribe(items => {
      const totalQuantity = items.reduce((sum, item) => sum + item.quantity, 0);
      this.cartCount.next(totalQuantity);
    });
  }

  private updateCartCountOnInit(): void {
    this.updateCartCount();
  }
}
