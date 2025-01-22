import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConditionsComponent } from './services/footer-hrefs/conditions/conditions.component';
import { DataProtectionComponent } from './services/footer-hrefs/data-protection/data-protection.component';
import { ImprintComponent } from './services/footer-hrefs/imprint/imprint.component';
import { MainpageComponent } from './services/mainpage/mainpage.component';
import { ProductPageComponent } from './services/productpage/productpage.component';
import { CartComponent } from './services/cart/cart.component';

const routes: Routes = [
  { path: '', component: MainpageComponent }, // Standardroute
  { path: 'mainpage', component: MainpageComponent },
  { path: 'conditions', component: ConditionsComponent },
  { path: 'data-protection', component: DataProtectionComponent },
  { path: 'imprint', component: ImprintComponent },
  { path: 'product/:productName/:color/:ram/:memory', component: ProductPageComponent },
  { path: 'cart', component: CartComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
