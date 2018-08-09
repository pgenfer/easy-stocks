import { Component, OnInit, Input } from '@angular/core';
import { StockService, Stock } from '../providers/stock.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-stock-item',
  templateUrl: './stock-item.component.html',
  styleUrls: ['./stock-item.component.css']
})
export class StockItemComponent implements OnInit {

  constructor(
    private readonly _stockService: StockService,
    private _route: ActivatedRoute) {
      this._route.params.subscribe(
        params => {
           this.symbol = params['symbol'];
        }
     );
    }

  public symbol: string;
  public stockItem: Stock;

  ngOnInit() {
    this._stockService.getStockForSymbol(this.symbol).subscribe(
      stock => {
        this.stockItem = stock;
      });
  }
}
