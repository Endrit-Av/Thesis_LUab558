import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Importiere FormsModule für den Header

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainpageComponent } from './services/mainpage/mainpage.component';
import { HeaderComponent } from './services/header/header.component';
import { FooterComponent } from './services/footer/footer.component';
import { ConditionsComponent } from './services/footer-hrefs/conditions/conditions.component';
import { DataProtectionComponent } from './services/footer-hrefs/data-protection/data-protection.component';
import { ImprintComponent } from './services/footer-hrefs/imprint/imprint.component';
import { ProductPageComponent } from './services/productpage/productpage.component';
import { CartComponent } from './services/cart/cart.component';

@NgModule({
  declarations: [
    AppComponent,
    MainpageComponent,
    HeaderComponent,
    FooterComponent,
    ConditionsComponent,
    DataProtectionComponent,
    ImprintComponent,
    ProductPageComponent,
    CartComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule // Formsmodule für den Header
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
