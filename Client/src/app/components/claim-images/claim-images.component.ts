import { Component, NgZone, OnInit } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import { ClaimImage } from "../../models/claim-image";
import { DocumentItem } from "../../models/document";
import { DocumentType } from "../../models/document-type";
import { SortColumnInfo } from '../../directives/table-sort.directive';
import { HttpService } from '../../services/http-service';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from "../ng-window";
import { UnindexedImageFileComponent } from "../../pages/unindexed-image-file/unindexed-image-file.component";
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from "ng2-bootstrap-modal";
import { DatePipe,DecimalPipe } from '@angular/common';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
declare var $: any;

@Component({
  selector: 'app-claim-images',
  templateUrl: './claim-images.component.html',
  styleUrls: ['./claim-images.component.css']
})
export class ClaimImagesComponent implements OnInit {


  sortColumn: SortColumnInfo;
  form: FormGroup;
  editing: boolean = false;
  editingDocumentId: number;
  constructor(
    public claimManager: ClaimManager,
    private myInjector: WindowsInjetor,
    private dialogService: DialogService,
    private formBuilder: FormBuilder,
    private ngZone: NgZone,
    private dp:DatePipe,
    private toast: ToastsManager,
    private http: HttpService) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      documentId: [null, Validators.compose([Validators.required])],
      claimId: [null, Validators.compose([Validators.required])],
      documentTypeId: [null, Validators.compose([Validators.required])],
      rxDate: [null],
      rxNumber: [null],
    });

  }
  openAttachment(image: ClaimImage) {
    localStorage.setItem('file-' + image.documentId, JSON.stringify(image));
    window.open('#/main/indexed-image/' + image.documentId, '_blank');
  }
  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  saveImage(image: any) {
    if(this.form.get('documentTypeId').valid ){
      this.claimManager.loading = true
      let rxDate  = this.dp.transform($("#datepicker").val(),"MM/dd/yyyy");
      let data = Object.assign(this.form.value);
      data.rxDate = rxDate;
      this.http.updateDocumentIndex(data).map(r=>{return r.json()}).single().subscribe(res=>{              
          this.toast.success(res.message); 
          this.claimManager.loading = false; 
          image.rxDate = $("#datepicker").val();         
          image.rxNumber = data.rxNumber;
          let type = this.claimManager.selectedClaim.documentTypes.find(t=>t.documentTypeId==this.form.get('documentTypeId').value);
          image.type = type.typeName;
          this.cancel();
      },error=>{                          
        this.toast.error(error.message);
        this.claimManager.loading = false;
      });
    }else{
      this.toast.warning("You must fill the type field");
    }
  }
  reindex(image: any) {
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Reindex Image",
      message: "Are you sure you wish to reindex this image?"
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true
          this.http.unindexImage(image.documentId).map(r => { return r.json() }).single().subscribe(res => {
            this.toast.success(res.message);
            this.removeImage(image);
            this.claimManager.loading = false;
          }, error => {
            this.toast.error(error.message);
            this.claimManager.loading = false;
          });
        }
        else { }
      });
  }
  removeImage(image:ClaimImage) {
    for (var i = 0; i < this.claimManager.selectedClaim.images.length; i++) {
      if (image.documentId == this.claimManager.selectedClaim.images[i].documentId) {
        this.claimManager.selectedClaim.images.splice(i, 1);
      }
    }
  }
  update(image: any) {
    this.editing = true;
    this.editingDocumentId = image.documentId; 
    this.form.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      documentId:image.documentId,
      rxDate: image.rxDate,
      rxNumber: image.rxNumber
    });
    let type = this.claimManager.selectedClaim.documentTypes.find(t=>t.typeName==image.type)
    try{
      this.form.get('documentTypeId').setValue(type.documentTypeId);
    }catch(e){}    
    setTimeout(()=>{
      $('#datepicker').datepicker({
        autoclose: true
      });
      $("#datepicker").inputmask("mm/dd/yyyy", {"placeholder": "mm/dd/yyyy"});
      $("[inputs-mask]").inputmask();
      $("[data-mask]").inputmask();
    },500)
  }
  cancel() {
    this.editing = false;
    this.editingDocumentId = undefined;  
    this.form.reset();
  }

  fetchData() {
    this.claimManager.loadingImage = true;
    var data = {
      claimId: this.claimManager.selectedClaim.claimId,
      date: null,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };
    let sort = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc';
    if (this.sortColumn) {
      data.sort = this.sortColumn.column;
      data.sortDirection = this.sortColumn.dir.toUpperCase();
    }

    this.http.getSortedImages(data).map(p => p.json())
      .subscribe(results => {
        this.claimManager.selectedClaim.setImages(results.claimImages);
        this.claimManager.loadingImage = false;
      }, err => {
        this.claimManager.loadingImage = false;
      });
  }

}
