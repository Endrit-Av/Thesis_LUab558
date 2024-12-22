import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

//interface WeatherForecast {
//  date: string;
//  temperatureC: number;
//  temperatureF: number;
//  summary: string;
//}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent /*implements OnInit*/ {
  title = 'thesis_luab558.client';
  products: any[] = []; // Speichert die empfangenen Produkte
  users: any[] = []; // Speichert die empfangenen Benutzer
  reviews: any[] = []; // Speichert die empfangenen Bewertungen
  images: any[] = []; // Speichert die empfangenen Bilder

  constructor(private http: HttpClient) {}

  //ngOnInit() {
  //  this.loadProducts();
  //  this.loadUsers();
  //  this.loadReviews();
  //  this.loadImages();
  //}

  //loadProducts() {
  //  this.http.get<any[]>('https://localhost:7219/api/test/products/first10').subscribe(
  //    (data) => this.products = data,
  //    (error) => console.error('Fehler beim Laden der Produkte:', error)
  //  );
  //}

  //loadUsers() {
  //  this.http.get<any[]>('https://localhost:7219/api/test/users/first10').subscribe(
  //    (data) => this.users = data,
  //    (error) => console.error('Fehler beim Laden der Benutzer:', error)
  //  );
  //}

  //loadReviews() {
  //  this.http.get<any[]>('https://localhost:7219/api/test/reviews/first10').subscribe(
  //    (data) => this.reviews = data,
  //    (error) => console.error('Fehler beim Laden der Bewertungen:', error)
  //  );
  //}

  //loadImages() {
  //  this.http.get<any[]>('https://localhost:7219/api/test/images/first10').subscribe(
  //    (data) => this.images = data,
  //    (error) => console.error('Fehler beim Laden der Bilder:', error)
  //  );
  //}

/*  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }*/
}
