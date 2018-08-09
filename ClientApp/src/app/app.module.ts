import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { StockListComponent } from './stocklist/stocklist.component';
import { StockItemComponent } from './stock-item/stock-item.component';
import { StockService } from './providers/stock.service';
import { AddStockItemComponent } from './add-stock-item/add-stock-item.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    StockListComponent,
    StockItemComponent,
    AddStockItemComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StockListComponent, pathMatch: 'full' },
      { path: 'stocks', component: StockListComponent },
      { path: 'stock-item/:symbol', component: StockItemComponent },
      { path: 'add-stock-item', component: AddStockItemComponent },
    ])
  ],
  providers: [
    StockService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
