import { Component, OnInit } from '@angular/core';
import { MainpageService } from './mainpage.service';
import { ImageService } from '../image/image.service'

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

  constructor(private mainpageService: MainpageService, private imageService: ImageService) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadBannerImages();
  }

  loadCategories(): void {
    this.mainpageService.getCategories().subscribe(
      (data) => {
        const desiredOrder = ['Smartphone', 'Notebook', 'Tablet'];
        this.categories = data.sort((a, b) => desiredOrder.indexOf(a) - desiredOrder.indexOf(b));
        this.categories.forEach((category) => {
          this.loadProducts(category);
        });
      },
      (error) => {
        console.error('Fehler beim Laden der Kategorien:', error);
      }
    );
  }

  loadProducts(category: string): void {
    this.mainpageService.getProductsByCategory(category).subscribe(
      (data) => {
        this.products[category] = data;
      },
      (error) => {
        console.error(`Fehler beim Laden der Produkte für Kategorie ${category}:`, error);
      }
    );
  }

  loadBannerImages(): void {
    this.imageService.getBannerImages().subscribe(
      (images) => {
        this.bannerImages = images;
      },
      (error) => {
        console.error('Fehler beim Laden der Banner-Bilder:', error);
      }
    );
  }
}
