import { Component, OnInit } from '@angular/core';
import { CartService } from './cart.service'

@Component({
  selector: 'app-cart',
  standalone: false,
  
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})

export class CartComponent implements OnInit {
  cartItems: any[] = [];
  totalPrice: number = 0;
  shippingCost: number = 4.95;

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.loadCartItems();
  }

  loadCartItems(): void {
    this.cartService.getCartItems().subscribe(
      (items) => {
        this.cartItems = items;
        this.calculateTotalPrice();
      },
      (error) => {
        console.error('Fehler beim Laden der Warenkorbdaten:', error);
      }
    );
  }

  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce((sum, item) => sum + item.quantity * item.product.price, 0);
    this.cartService.updateCartCount();
  }

  isFreeShipping(): boolean {
    return this.totalPrice > 2000;
  }

  increaseQuantity(item: any): void {
    if (item.product.stock > 0) {
      item.quantity++;
      item.product.stock--;
      this.calculateTotalPrice();
    } else {
      console.warn('Nicht genügend Vorrat vorhanden!');
    }
  }

  decreaseQuantity(item: any): void {
    item.quantity--;
    item.product.stock++;

    if (item.quantity < 1) {
      this.removeFromCart(item.productId); // Ruft die removeFromCart-Methode auf wenn quantity unter 1 geht
    } else {
      this.calculateTotalPrice();
    }
  }

  removeFromCart(productId: number): void {
    this.cartService.removeFromCart(productId).subscribe(() => {
      this.loadCartItems();
    });
  }

}
