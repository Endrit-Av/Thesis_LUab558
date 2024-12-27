import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { MainpageService } from '../mainpage/mainpage.service';
import { ReviewService } from '../review/review.service';
import { ImageService } from '../image/image.service'
import { CartService } from '../cart/cart.service';
import { translateColor } from '../../utils/color-translator';

@Component({
  selector: 'app-productpage',
  standalone: false,
  
  templateUrl: './productpage.component.html',
  styleUrls: ['./productpage.component.css']
})
export class ProductPageComponent implements OnInit {
  product: any;

  //Karousel
  images: any[] = [];
  currentIndex: number = 0;

  //Eigenschaftsbuttons
  availableColors: string[] = [];
  availableRam: number[] = [];
  availableMemory: number[] = [];

  //Reviewsabschnitt
  reviews: any[] = [];
  averageRating: number = 0;
  totalReviews: number = 0;
  stars: number[] = [1, 2, 3, 4, 5]; // Für die Sterneanzeige
  popupVisible: boolean = false;
  starRatings: { stars: number; percentage: number }[] = [];
  //allReviewsLink: string = ''; //Für Potenzielle ReviewPage

  constructor(
    private route: ActivatedRoute,
    private mainpageService: MainpageService,
    private imageService: ImageService,
    private reviewService: ReviewService,
    private cartService: CartService,
    private router: Router //Aktuell ungenutzt --> bei cleanup entfernen
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
    this.imageService.getImagesByAttributes(productName, color).subscribe(
      (data) => {
        this.images = data;
      },
      (error) => {
        console.error('Fehler beim Laden der Bilder:', error);
      }
    );
  }

  loadProductByAttributes(productName: string, color: string, ram: number, memory: number): void {
    this.mainpageService.getProductByAttributes(productName, color, ram, memory).subscribe(
      (data) => {
        this.product = data;

        // Hole die Produkt-ID und lade die Reviews und Sternebewertung
        const productId = data.productId;
        this.loadImages(data.productName, data.color);
        this.loadReviewData(productId);
        this.loadAverageRating(productId);

        // Lade die Varianten basierend auf dem Produktnamen
        this.mainpageService.getProductVariants(this.product.productName).subscribe(
          (variants) => {
            this.availableColors = variants.availableColors;
            this.availableRam = variants.availableRam;
            this.availableMemory = variants.availableMemory;
          },
          (error) => {
            console.error('Fehler beim Laden der Varianten:', error);
          }
        );
      },
      (error) => {
        console.error('Fehler beim Laden des Produkts:', error);
      }
    );
  }

  //addToCart(productId: number): void {
  //  console.log('Produkt zum Warenkorb hinzugefügt:', productId);
  //  //Später logik und backend-service einbinden
  //}

  addToCart(productId: number): void {
    if (this.product.stock > 0) {
      this.cartService.addToCart(productId).subscribe(
        () => {
          this.cartService.updateCartCount(); //Updated Warenkorb zähler
          this.product.stock--;
        },
        (error) => {
          console.error('Fehler beim Hinzufügen zum Warenkorb:', error);
        }
      );
    }
  }

  copyURL(url: string): void {
    navigator.clipboard.writeText(url).then(() => {
      console.log('URL wurde kopiert:', url);
    });
  }

  // Review Logik
  loadReviewData(productId: number): void {
    this.reviewService.getReviewsByProductId(productId).subscribe(
      (reviews) => {
        const total = reviews.length;
        const ratingsCount = [0, 0, 0, 0, 0];

        reviews.forEach((review) => {
          ratingsCount[review.rating - 1]++;
        });

        // Berechnung für die Sternebewertung
        this.starRatings = ratingsCount
          .map((count, index) => ({
            stars: index + 1,
            percentage: total > 0 ? Math.round((count / total) * 100) : 0,
          }))
          .reverse(); // Reihenfolge der Sterne umkehren

        // Speichere die Reviews und Anzahl der Bewertungen
        this.reviews = reviews;
        this.totalReviews = total;

        // Falls später eine Review-Seite benötigt wird:
        // this.allReviewsLink = `/reviews/${productId}`;
      },
      (error) => {
        console.error('Fehler beim Laden der Reviews:', error);
      }
    );
  }

  loadAverageRating(productId: number): void {
    this.reviewService.getAverageRatingByProductId(productId).subscribe(
      (data) => {
        this.averageRating = data.averageRating;
      },
      (error) => {
        console.error('Fehler beim Laden der Durchschnittsbewertung:', error);
      }
    );
  }

  showPopup(): void {
    this.popupVisible = true;
  }

  closePopup(): void {
    this.popupVisible = false;
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
