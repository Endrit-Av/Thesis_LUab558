import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Importiere FormsModule

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainpageComponent } from './services/mainpage/mainpage.component';
import { HeaderComponent } from './services/header/header.component';

@NgModule({
  declarations: [
    AppComponent,
    MainpageComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule // Füge FormsModule hier hinzu
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
