import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { ReviewService } from '../../services/review.service';
import { ImageService } from '../../services/image.service'
import { CartService } from '../../services/cart.service';
import { translateColor } from '../../utils/color-translator';

@Component({
  selector: 'app-productpage',
  standalone: false,
  
  templateUrl: './productpage.component.html',
  styleUrls: ['./productpage.component.css']
})
export class ProductPageComponent implements OnInit {
  product: any;

  images: any[] = [];
  currentIndex: number = 0;

  availableColors: string[] = [];
  availableRam: number[] = [];
  availableMemory: number[] = [];

  reviews: any[] = [];
  averageRating: number = 0;
  totalReviews: number = 0;
  stars: number[] = [1, 2, 3, 4, 5];
  popupVisible: boolean = false;
  starRatings: { stars: number; percentage: number }[] = [];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private imageService: ImageService,
    private reviewService: ReviewService,
    private cartService: CartService,
  ) { }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.route.paramMap.subscribe(params => {
      const productName = params.get('productName');
      const color = params.get('color');
      const ram = Number(params.get('ram'));
      const memory = Number(params.get('memory'));

      if (productName && color && ram && memory) {
        this.loadProductByAttributes(productName, color, ram, memory);
      }
    });
  }

  loadImages(productName: string, color: string): void {
    this.imageService.getImagesByAttributes(productName, color).subscribe({
      next: data => {
        this.images = data;
      },
      error: error => {
        console.error('Fehler beim Laden der Bilder:', error);
      }
    });
  }

  loadProductByAttributes(productName: string, color: string, ram: number, memory: number): void {
    this.productService.getProductByAttributes(productName, color, ram, memory).subscribe({
      next: data => {
        this.product = data;
        this.product.shareUrl = window.location.href

        this.loadImages(data.productName, data.color);
        this.loadReviewData(data.productId);
        this.loadAverageRating(data.productId);

        this.productService.getProductVariants(this.product.productName).subscribe({
          next: variants => {
            this.availableColors = variants.availableColors;
            this.availableRam = variants.availableRam.sort((a: number, b: number) => a - b);
            this.availableMemory = variants.availableMemory.sort((a: number, b: number) => a - b);
          },
          error: error => {
            console.error('Fehler beim Laden der Varianten:', error);
          }
        });
      },
      error: error => {
        console.error('Fehler beim Laden des Produkts:', error);
      }
    });
  }

  addToCart(productId: number, productName: string,): void {
    if (this.product.stock > 0) {
      this.cartService.addToCart(productId).subscribe({
        next: () => {
          this.cartService.updateCartCount();
          this.product.stock--;
          this.showCartPopup(productName);
        },
        error: error => {
          console.error('Fehler beim Hinzufügen zum Warenkorb:', error);
        }
      });
    }
  }

  copyURL(url: string): void {
    navigator.clipboard.writeText(url).then(() => {
      this.showURLPopup();
    });
  }

  loadReviewData(productId: number): void {
    this.reviewService.getReviewsByProductId(productId).subscribe({
      next: reviews => {
        const total = reviews.length;
        const ratingsCount = [0, 0, 0, 0, 0];

        reviews.forEach((review) => {
          ratingsCount[review.rating - 1]++;
        });

        this.starRatings = ratingsCount
          .map((count, index) => ({
            stars: index + 1,
            percentage: total > 0 ? Math.round((count / total) * 100) : 0,
          }))
          .reverse();

        this.reviews = reviews;
        this.totalReviews = total;
      },
      error: error => {
        console.error('Fehler beim Laden der Reviews:', error);
      }
    });
  }

  loadAverageRating(productId: number): void {
    this.reviewService.getAverageRatingByProductId(productId).subscribe({
      next: (data) => {
        this.averageRating = data.averageRating;
      },
      error: (error) => {
        console.error('Fehler beim Laden der Durchschnittsbewertung:', error);
      }
    });
  }

  showPopup(): void {
    this.popupVisible = true;
  }

  closePopup(): void {
    this.popupVisible = false;
  }

  showCartPopup(productName: string): void {
    const popup = document.getElementById('popup_cart');
    if (popup) {
      popup.innerText = `${productName} wurde zum Warenkorb hinzugefügt!`;
      popup.style.display = 'block';
      setTimeout(() => {
        popup.style.display = 'none';
      }, 3000);
    }
  }

  showURLPopup(): void {
    const popup = document.getElementById('popup_link');
    if (popup) {
      popup.style.display = 'block';
      setTimeout(() => {
        popup.style.display = 'none';
      }, 3000);
    }
  }

  // Helper-Funktionen
  selectSlide(index: number): void {
    this.currentIndex = index;
    const carousel = document.getElementById('product-carousel') as any;
    if (carousel) {
      carousel.querySelectorAll('.carousel-item').forEach((item: HTMLElement, idx: number) => {
        item.classList.toggle('active', idx === index);
      });
    }
  }

  translateColor(color: string): string {
    return translateColor(color);
  }

  checkIfProductIsInStock(): boolean {
    return this.product && this.product.stock > 0;
  }

  isCurrentColor(color: string): boolean {
    return this.product.color === color;
  }

  isCurrentRam(ram: number): boolean {
    return this.product.ram === ram;
  }

  isCurrentMemory(memory: number): boolean {
    return this.product.physicalMemory === memory;
  }
}
