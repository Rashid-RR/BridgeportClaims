import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {ClaimImage} from "../../models/claim-image";
import {DocumentItem} from "../../models/document";
import { SortColumnInfo } from '../../directives/table-sort.directive';
import { HttpService } from '../../services/http-service';
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";
import {UnindexedImageFileComponent} from "../../pages/unindexed-image-file/unindexed-image-file.component";
declare var  $:any; 

@Component({
  selector: 'app-claim-images',
  templateUrl: './claim-images.component.html',
  styleUrls: ['./claim-images.component.css']
})
export class ClaimImagesComponent implements OnInit {


  sortColumn: SortColumnInfo;
  constructor(
    public claimManager:ClaimManager,
    private myInjector: WindowsInjetor,
    private http: HttpService) { }

  ngOnInit() {

  }
  openAttachment(image:ClaimImage){
    localStorage.setItem('file-' + image.documentId, JSON.stringify(image));
    window.open('#/main/indexed-image/' + image.documentId, '_blank');    
  }
  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  fetchData() {
  this.claimManager.loadingImage = true;
    var data = {
      claimId:this.claimManager.selectedClaim.claimId,
      date: null,
      isIndexed:true,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      pageSize: 500
    };
    let sort = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc';
    if (this.sortColumn) {
      data.sort = this.sortColumn.column;
      data.sortDirection = this.sortColumn.dir;
    }
    
    this.http.getDocuments(data).map(p => p.json())
      .subscribe(results => {
        this.claimManager.selectedClaim.setImages(results.documentResults);
        this.claimManager.loadingImage = false;
      },err=>{
        this.claimManager.loadingImage = false;
      });
  }

}
