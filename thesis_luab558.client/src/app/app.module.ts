import { provideHttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Importiere FormsModule für den Header
import { LOCALE_ID } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainpageComponent } from './components/mainpage/mainpage.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { ConditionsComponent } from './components/footer-hrefs/conditions/conditions.component';
import { DataProtectionComponent } from './components/footer-hrefs/data-protection/data-protection.component';
import { ImprintComponent } from './components/footer-hrefs/imprint/imprint.component';
import { ProductPageComponent } from './components/productpage/productpage.component';
import { CartComponent } from './components/cart/cart.component';

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
    BrowserModule,
    AppRoutingModule,
    FormsModule // Formsmodule für den Header
  ],
  providers: [
    provideHttpClient(),
    { provide: LOCALE_ID, useValue: 'de-DE' } // Setzt 'de-DE' als Standard-Lokalisierung
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
