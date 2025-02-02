import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { ImageService } from '../../services/image.service'

@Component({
  selector: 'app-mainpage',
  standalone: false,
  
  templateUrl: './mainpage.component.html',
  styleUrls: ['./mainpage.component.css']
})
export class MainpageComponent implements OnInit {
  categories: string[] = [];
  products: { [key: string]: any[] } = {};
  bannerImages: string[] = [];

  constructor(private productService: ProductService, private imageService: ImageService) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadBannerImages();
  }

  loadCategories(): void {
    this.productService.getCategories().subscribe({
      next: data => {
        const desiredOrder = ['Smartphone', 'Notebook', 'Tablet'];
        this.categories = data.sort((a, b) => desiredOrder.indexOf(a) - desiredOrder.indexOf(b));
        this.categories.forEach((category) => {
          this.loadProducts(category);
        });
      },
      error: error => {
        console.error('Fehler beim Laden der Kategorien:', error);
      }
    });
  }

  loadProducts(category: string): void {
    this.productService.getProductsByCategory(category).subscribe({
      next: data => {
        this.products[category] = data;

      },
      error: error => {
        console.error(`Fehler beim Laden der Produkte für Kategorie ${category}:`, error);
      }
    });
  }

  loadBannerImages(): void {
    this.imageService.getBannerImages().subscribe({
      next: images => {
        this.bannerImages = images;
      },
      error: error => {
        console.error('Fehler beim Laden der Banner-Bilder:', error);
      }
    });
  }
}
