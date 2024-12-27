import { Component, OnInit } from '@angular/core';
import { CartService } from '../cart/cart.service';

@Component({
  selector: 'app-header',
  standalone: false,
  
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  countCart: number = 0;
  countWishlist: number = 0;
  showPopup: boolean = false;

  email: string = '';
  password: string = '';
  emailError: string = '';
  passwordError: string = '';

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.cartService.cartCount$.subscribe(count => {
      this.countCart = count; // Header-Zähler aktualisieren
    });
  }

  togglePopup(): void {
    this.showPopup = !this.showPopup;
  }

  login(): void {
    if (!this.email || !this.password) {
      this.emailError = 'E-Mail ist erforderlich.';
      this.passwordError = 'Passwort ist erforderlich.';
    } else {
      console.log('Login mit:', this.email, this.password);
      this.emailError = '';
      this.passwordError = '';
      this.togglePopup();
    }
  }
}
