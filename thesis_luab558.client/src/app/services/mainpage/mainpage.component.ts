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
  products: any[] = [];

  constructor(private mainpageService: MainpageService) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.mainpageService.getCategories().subscribe(
      (data) => {
        console.log('Kategorien:', data); // Prüfe die Daten in der Konsole
        this.categories = data;
      },
      (error) => {
        console.error('Fehler beim Laden der Kategorien:', error); // Fehler ausgeben
      }
    );
  }

  loadProducts(category: string): void {
    this.mainpageService.getProductsByCategory(category).subscribe(
      (data) => {
        console.log('Produkte:', data); // Prüfe die Daten in der Konsole
        this.products = data;
      },
      (error) => {
        console.error('Fehler beim Laden der Produkte:', error); // Fehler ausgeben
      }
    );
  }

}
