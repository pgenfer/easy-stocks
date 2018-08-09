import { Injectable, Inject } from '../../../node_modules/@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class StockService {

    public stocks: Stock[];

    constructor(
        private readonly _http: HttpClient,
        @Inject('BASE_URL') private readonly _baseUrl: string) {
            _http.get<Stock[]>(`${_baseUrl}api/Stocks/CurrentAccountItems`).subscribe(
                result => {
                  this.stocks = result;
              }, error => console.error(error));
    }

    /**
     * returns all relevant stock information for a single stock symbol.
     * This contains the daily data of the stock and all relevant account information
     * for this stock.
     * @param symbol the yahoo financial symbol used to search for this stock
     */
    public getStockForSymbol(symbol: string): Observable<Stock> {
      const stockSource = new Subject<Stock>();
      this._http.get<Stock>(`${this._baseUrl}api/Stocks/AccountItemBySymbol/`, {
        params: {
          'symbol': symbol
        }}).subscribe(
          result => {
            stockSource.next(result);
          });
      return stockSource.asObservable();
    }
}

/**
 * represents the information about a specific stock.
 * This contains the daily information and the stock items
 * which are in the users account.
 */
export interface Stock {
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
  /**
   * an account item represents a single stock position
   * within the user's account
   */
export interface AccountItem {
    stopRate: number;
    buyingRate: number;
    buyingDate: Date;
    absoluteOverallChangeInPercent: number;
    overallChangeIsPositive: boolean;
  }
