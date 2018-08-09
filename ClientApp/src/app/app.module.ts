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

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    StockListComponent,
    StockItemComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StockListComponent, pathMatch: 'full' },
      { path: 'stocks', component: StockListComponent },
      { path: 'stock-item/:symbol', component: StockItemComponent },
    ])
  ],
  providers: [
    StockService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
