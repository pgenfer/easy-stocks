import { Component, OnInit, Inject } from '@angular/core';
import {Router} from '@angular/router';
import { StockService } from '../providers/stock.service';

@Component({
  selector: 'app-stocklist',
  templateUrl: './stocklist.component.html',
  styleUrls: ['./stocklist.component.css']
})
export class StockListComponent {
  constructor(
    public readonly router:Router,
    public readonly stockService: StockService) {
  }
}
