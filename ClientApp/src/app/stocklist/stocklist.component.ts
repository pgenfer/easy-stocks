import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-stocklist',
  templateUrl: './stocklist.component.html',
  styleUrls: ['./stocklist.component.css']
})
export class StockListComponent {
  public stocks: Stock[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Stock[]>(baseUrl + 'api/Stocks/CurrentAccountItems').subscribe(
      result => {
        this.stocks = result;
        this.stocks.forEach(x => x.isPositive = x.dailyChangeInPercent >= 0);
    }, error => console.error(error));
  }
}

interface Stock {
  name: string;
  symbol: string;
  currentRate: number;
  currentChange: number;
  dailyChangeInPercent: number;
  lastTradingDate: Date;
  isPositive: boolean;
  // TODO: add account items here
}
