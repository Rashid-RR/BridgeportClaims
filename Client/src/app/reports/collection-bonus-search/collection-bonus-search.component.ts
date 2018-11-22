import { Component, OnInit } from '@angular/core';
import { CollectionBonusService, ReportLoaderService, HttpService} from '../../services/services.barrel';
declare var $: any;

@Component({
  selector: 'app-collection-bonus-search',
  templateUrl: './collection-bonus-search.component.html',
  styleUrls: ['./collection-bonus-search.component.css']
})
export class CollectionBonusSearchComponent implements OnInit {

  constructor(public http: HttpService, public cb: CollectionBonusService, public reportloader: ReportLoaderService) {
     
   }

  ngOnInit() {
    
  }

}

