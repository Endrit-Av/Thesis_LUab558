import { Component, OnInit } from '@angular/core';
import { MainpageService } from '../mainpage.service';

@Component({
  selector: 'app-mainpage',
  standalone: false,
  
  templateUrl: './mainpage.component.html',
  styleUrl: './mainpage.component.css'
})
export class MainpageComponent implements OnInit {
  categories: string[] = [];
  products: { [key: string]: any[] } = {};

  constructor(private mainpageService: MainpageService) { }

  ngOnInit(): void {
    this.loadCategories();
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
}
