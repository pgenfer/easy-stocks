import { Injectable, Inject } from "../../../node_modules/@angular/core";
import { HttpClient } from '@angular/common/http';

@Injectable()
export class StockService{

    public stocks: Stock[];

    constructor(
        http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string){
            http.get<Stock[]>(baseUrl + 'api/Stocks/CurrentAccountItems').subscribe(
                result => {
                  this.stocks = result;
                  this.stocks.forEach(x => {
                    x.isPositive = x.dailyChangeInPercent >= 0;
                    x.dailyChangeInPercentAbsolute = Math.abs(x.dailyChangeInPercent);          
                    x.isStopRateReached = !x.accountItems.every(y => y.stopRate < x.currentRate);
                  });
              }, error => console.error(error));
            
        }
}

interface Stock {
    name: string;
    symbol: string;
    currentRate: number;
    currentChange: number;
    dailyChangeInPercent: number;
    dailyChangeInPercentAbsolute: number;
    lastTradingDate: Date;
    isPositive: boolean;
    isStopRateReached: boolean;
    accountItems: AccountItem[];
  }
  
  interface AccountItem{
    stopRate: number;
    buyingRate: number;
    buyingDate: Date;
  }