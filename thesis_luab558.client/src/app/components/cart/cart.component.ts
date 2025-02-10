import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service'

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
    this.cartService.getCartItems().subscribe({
      next: items => {
        this.cartItems = items;
        this.calculateTotalPrice();
      },
      error: error => {
        console.error('Fehler beim Laden der Warenkorbdaten:', error);
      }
    });
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
      this.cartService.increaseQuantity(item.product.productId).subscribe(() => {
        item.quantity++;
        item.product.stock--;
        this.calculateTotalPrice();
      });
    } else {
      console.warn('Nicht genügend Vorrat vorhanden!');
    }
  }

  decreaseQuantity(item: any): void {
    if (item.quantity > 1) {
      this.cartService.decreaseQuantity(item.product.productId).subscribe(() => {
        item.quantity--;
        item.product.stock++;
        this.calculateTotalPrice();
      });
    } else {
      this.removeFromCart(item.productId);
    }
  }

  removeFromCart(productId: number): void {
    this.cartService.removeFromCart(productId).subscribe(() => {
      this.loadCartItems();
    });
  }

}
