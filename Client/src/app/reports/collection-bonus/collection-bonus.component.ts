import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, CollectionBonusService } from '../../services/services.barrel';

@Component({
  selector: 'app-collection-bonus',
  templateUrl: './collection-bonus.component.html',
  styleUrls: ['./collection-bonus.component.css']
})
export class CollectionBonusComponent implements OnInit {

  constructor(public reportloader: ReportLoaderService, public bonusService: CollectionBonusService) { }

  ngOnInit() {
    this.reportloader.current = 'Collection Bonus Report';
    this.reportloader.currentURL = 'collection-bonus';
  }

}
