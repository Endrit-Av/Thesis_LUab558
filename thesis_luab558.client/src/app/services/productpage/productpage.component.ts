import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { MainpageService } from '../mainpage/mainpage.service';
import { translateColor } from '../../utils/color-translator';

@Component({
  selector: 'app-productpage',
  standalone: false,
  
  templateUrl: './productpage.component.html',
  styleUrls: ['./productpage.component.css']
})
export class ProductPageComponent implements OnInit {
  product: any;
  availableColors: string[] = [];
  availableRam: number[] = [];
  availableMemory: number[] = [];

  constructor(
    private route: ActivatedRoute,
    private mainpageService: MainpageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Auf Änderungen der Routenparameter lauschen
    this.route.paramMap.subscribe(params => {
      const productId = params.get('id');
      if (productId) {
        this.loadProductById(Number(productId));
      }
    });
  }

  loadProductById(productId: number): void {
    this.mainpageService.getProductById(productId).subscribe(
      (data) => {
        this.product = data;

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

  loadProductByAttributes(color: string, ram: number, memory: number): void {
    this.mainpageService.getProductByAttributes(this.product.productName, color, ram, memory).subscribe((data) => {
      this.router.navigate(['/product', data.productId]);
    });
  }

  selectOption(optionType: string, value: any): void {
    console.log(`Ausgewählt: ${optionType} - ${value}`);
  }

  addToCart(productId: number): void {
    console.log('Produkt zum Warenkorb hinzugefügt:', productId);
    //Später logik und backend-service einbinden
  }

  copyURL(url: string): void {
    navigator.clipboard.writeText(url).then(() => {
      console.log('URL wurde kopiert:', url);
    });
  }

  // Helper-Funktionen
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
