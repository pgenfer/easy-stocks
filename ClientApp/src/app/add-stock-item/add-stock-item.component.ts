import { Component, OnInit } from '@angular/core';
import { NewAccountItem, StockService } from '../providers/stock.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-stock-item',
  templateUrl: './add-stock-item.component.html',
  styleUrls: ['./add-stock-item.component.css']
})
export class AddStockItemComponent implements OnInit {

  public newAccountItem: NewAccountItem = new NewAccountItem();
  public message = '';
  public isErrorMessage = false;

  constructor(
    private readonly _stockService: StockService,
    private readonly _router: Router) { }

  ngOnInit() {
  }

  public searchStockSymbol(): void {
    if (this.newAccountItem.stockSymbol === undefined ||
        this.newAccountItem.stockSymbol === '') {
      this.isErrorMessage = true;
      this.message = 'no stock symbol';
      this.newAccountItem.isStockFound = false;
    } else {
      this.isErrorMessage = false;
      this.message = 'searching';
      this._stockService.findNewStockBySymbol(this.newAccountItem.stockSymbol).subscribe(
        stock => {
          if (stock === undefined || stock === null) {
            this.message = `stock with symbol ${this.newAccountItem.stockSymbol} not found.`;
            this.isErrorMessage = true;
            this.newAccountItem.isStockFound = false;
          } else {
            this.newAccountItem.isStockFound = true;
            this.isErrorMessage = false;
            this.message = 'found';
            this.newAccountItem.buyingDate = stock.buyingDate;
            this.newAccountItem.buyingRate = stock.buyingRate;
            this.newAccountItem.stopRate = stock.stopRate;
          }
        }
      );
    }
  }

  public cancel(): void {
    this._router.navigateByUrl('/stocks');
  }

  public changeStockItemWatchListState(event: any){
    this.newAccountItem.isOnWatchList = !this.newAccountItem.isOnWatchList;
  }

  public saveStock(): void {
    this._stockService.createNewAccountItem(this.newAccountItem).subscribe(
      x => {
        if (x) {
          this._router.navigateByUrl('/stocks');
        } else {
          this.isErrorMessage = true;
          this.message = 'Account item could not be saved';
        }
      }
    );
  }
}
