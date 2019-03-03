import { Component, OnInit, Input } from '@angular/core';
import { StockService, Stock, AccountItem } from '../providers/stock.service';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-stock-item',
  templateUrl: './stock-item.component.html',
  styleUrls: ['./stock-item.component.css']
})
export class StockItemComponent implements OnInit {

  public stockNews: StockNews[] = [];

  constructor(
    private readonly _http: HttpClient,
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
  public loadingNews: boolean = true;

  public changeAccountItemWatchListState(accountItem: AccountItem, event: any){
    accountItem.isOnWatchList = !accountItem.isOnWatchList;
    console.log(accountItem.isOnWatchList);
  }

  ngOnInit() {
    this.loadingNews = true;
    this._stockService.getStockForSymbol(this.symbol).subscribe(
      stock => {
        this.stockItem = stock;
        this.stockItem.accountItems.forEach(
          x => {
            const overallChange = 100 / x.buyingRate * ( this.stockItem.currentRate - x.buyingRate);
            x.absoluteOverallChangeInPercent = Math.abs(overallChange);
            x.overallChangeIsPositive = overallChange >= 0;
          });
        // get news for this stock
        this._http.get<any[]>(
          `https://api.newsriver.io/v2/search?query=text%3A((%22${this.stockItem.name}%22%20AND%20stocks)%20OR%20(%22${this.stockItem.name}%22%20AND%20aktie)%20OR%20%22${this.stockItem.name}%22)%20AND%20language%3A(en%20OR%20de)`,
          //`https://api.newsriver.io/v2/search?query=text%3A%22${this.stockItem.name}%22%20AND%20text%3A'stock'&sortBy=discoverDate&sortOrder=DESC&limit=15`,
          {
            headers:{
              'Authorization':'sBBqsGXiYgF0Db5OV5tAw4hBQbXooMFUzferbKVWkVouNmZbo0W7xpcqovxByq98n2pHZrSf1gT2PUujH1YaQA'
            }
          }).subscribe(result => {
            this.loadingNews = false;
            this.stockNews = [];
            result.forEach(entry => {
              const hostName = entry.hasOwnProperty('website') ? entry.website.hostName : '';
              const elements = <any[]>(entry.elements);
              let imageUrl = '';
              let hasImage = false;
              if(elements.length > 0){
                if(elements[0].type == 'Image'){
                  imageUrl = elements[0].url;
                  hasImage = true;
                }
              }

              this.stockNews.push(
                new StockNews(
                  entry.title,
                  entry.discoverDate,
                  entry.url,
                  imageUrl,
                  hostName,
                  hasImage
                )
              )
            });
          }
        );
      });
    }
  }

  class StockNews{
    constructor(
      public readonly title:string,
      public readonly date:Date,
      public readonly url:string,
      public readonly image:string,
      public readonly hostName:string,
      public readonly hasImage: boolean ){
        console.log('has image ' + hasImage);
      }
  }
