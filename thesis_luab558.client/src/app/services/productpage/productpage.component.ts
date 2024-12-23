import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MainpageService } from '../mainpage/mainpage.service';

@Component({
  selector: 'app-productpage',
  standalone: false,
  
  templateUrl: './productpage.component.html',
  styleUrls: ['./productpage.component.css']
})
export class ProductPageComponent implements OnInit {
  product: any;

  constructor(
    private route: ActivatedRoute,
    private mainpageService: MainpageService
  ) { }

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.mainpageService.getProductById(Number(productId)).subscribe(
        (data) => {
          this.product = data;
        },
        (error) => {
          console.error('Fehler beim Laden des Produkts:', error);
        }
      );
    }
  }

  addToCart(productId: number): void {
    console.log('Produkt zum Warenkorb hinzugefügt:', productId);
  }

  copyURL(url: string): void {
    navigator.clipboard.writeText(url).then(() => {
      const popup = document.getElementById('popup_link');
      if (popup) {
        popup.style.display = 'block';
        setTimeout(() => {
          popup.style.display = 'none';
        }, 3000);
      }
    });
  }
}
