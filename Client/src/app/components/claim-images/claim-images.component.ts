import { DatePipe } from '@angular/common';
import { Component, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from 'ng2-bootstrap-modal';
import { ToastrService } from 'ngx-toastr';
import { SortColumnInfo } from '../../directives/table-sort.directive';
import { ClaimImage } from '../../models/claim-image';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { EpisodeNoteModalComponent } from '../components-barrel';
import { ConfirmComponent } from '../confirm.component';
import { CustomPosition, Size, WindowConfig, WindowsInjetor } from '../ng-window';
declare var $: any;

@Component({
  selector: 'app-claim-images',
  templateUrl: './claim-images.component.html',
  styleUrls: ['./claim-images.component.css']
})
export class ClaimImagesComponent implements OnInit {


  sortColumn: SortColumnInfo;
  form: FormGroup;
  editing = false;
  editingDocumentId: number;
  constructor(
    public claimManager: ClaimManager,
    private myInjector: WindowsInjetor,
    private dialogService: DialogService,
    private formBuilder: FormBuilder,
    private ngZone: NgZone,
    private dp: DatePipe,
    private toast: ToastrService,
    private http: HttpService) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      documentId: [null, Validators.compose([Validators.required])],
      claimId: [null, Validators.compose([Validators.required])],
      documentTypeId: [null, Validators.compose([Validators.required])],
      rxDate: [null],
      rxNumber: [null],
      injuryDate: [null],
      attorneyName: [null],
      invoiceNumber: [null],
    });

  }
  openAttachment(image: ClaimImage) {
    localStorage.setItem('file-' + image.documentId, JSON.stringify(image));
    window.open('#/main/indexing/indexed-image/' + image.documentId, '_blank');
  }
  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }
  showNoteWindow(episode: any) {
    if (!episode.episodeId && episode['episodeId']) {
      episode.episodeId = episode['episodeId'];
    }
    const config = new WindowConfig('Episode Note(s)', new Size(400, 700));  // height, width
    config.position = new CustomPosition((window.innerWidth - 700) / 2 + 50, 60); // left,top
    config.minusTop = 0;
    config.minusHeight = 0;
    config.minusLeft = 0;
    config.minusWidth = 0;
    config.centerInsideParent = false;
    const temp = {};
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(EpisodeNoteModalComponent, config)
      .then((win: EpisodeNoteModalComponent) => {
        win.showNote(episode);
      });
  }

  saveImage(image: any) {
    if (this.form.get('documentTypeId').valid) {
      this.claimManager.loading = true;
      const rxDate = this.dp.transform($('#datepicker').val(), 'MM/dd/yyyy');
      const injuryDate = this.dp.transform($('#injuryDate').val(), 'MM/dd/yyyy');
      const data = Object.assign(this.form.value);
      data.rxDate = rxDate;
      data.injuryDate = injuryDate;
      this.http.updateDocumentIndex(data).subscribe(res => {
        this.toast.success(res.message);
        this.claimManager.loading = false;
        image.rxDate = $('#datepicker').val();
        image.injuryDate = $('#injuryDate').val();
        image.rxNumber = data.rxNumber;
        image.invoiceNumber = data.invoiceNumber;
        image.attorneyName = data.attorneyName;
        const type = this.claimManager.selectedClaim.documentTypes.find(t => t.documentTypeId === this.form.get('documentTypeId').value);
        image.type = type.typeName;
        const shit = this.claimManager.selectedClaim.images;
        this.cancel();
      }, error => {
        this.toast.error(error.message);
        this.claimManager.loading = false;
      });
    } else {
      this.toast.warning('You must fill the type field');
    }
  }
  reindex(image: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Reindex Image',
      message: 'Are you sure you wish to reindex this image?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true;
          this.http.unindexImage(image.documentId).subscribe(res => {
            this.toast.success(res.message);
            this.removeImage(image);
            this.claimManager.loading = false;
          }, error => {
            this.toast.error(error.message);
            this.claimManager.loading = false;
          });
        } else { }
      });
  }
  removeImage(image: ClaimImage) {
    for (let i = 0; i < this.claimManager.selectedClaim.images.length; i++) {
      if (image.documentId === this.claimManager.selectedClaim.images[i].documentId) {
        this.claimManager.selectedClaim.images.splice(i, 1);
      }
    }
  }
  update(image: any) {
    this.editing = true;
    this.editingDocumentId = image.documentId;
    this.form.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      documentId: image.documentId,
      rxDate: image.rxDate,
      rxNumber: image.rxNumber,
      injuryDate: image.injuryDate,
      attorneyName: image.attorneyName,
      invoiceNumber: image.invoiceNumber,
    });
    const type = this.claimManager.selectedClaim.documentTypes.find(t => t.typeName === image.type);
    try {
      this.form.get('documentTypeId').setValue(type.documentTypeId);
    } catch (e) { }
    setTimeout(() => {
      $('#datepicker').datepicker({
        autoclose: true
      });
      $('#datepicker').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
      $('#injuryDate').datepicker({
        autoclose: true
      });
      $('#injuryDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
      $('[inputs-mask]').inputmask();
      $('[data-mask]').inputmask();
    }, 500);
  }
  cancel() {
    this.editing = false;
    this.editingDocumentId = undefined;
    this.form.reset();
  }

  fetchData() {
    this.claimManager.loadingImage = true;
    const data = {
      claimId: this.claimManager.selectedClaim.claimId,
      date: null,
      sort: 'DocumentID',
      sortDirection: 'ASC',
      page: 1,
      pageSize: 30
    };
    const sort = 'RxDate';
    const sort_dir: 'asc' | 'desc' = 'desc';
    if (this.sortColumn) {
      data.sort = this.sortColumn.column;
      data.sortDirection = this.sortColumn.dir.toUpperCase();
    }

    this.http.getSortedImages(data)
      .subscribe(results => {
        this.claimManager.selectedClaim.setImages(results.claimImages);
        this.claimManager.loadingImage = false;
      }, err => {
        this.claimManager.loadingImage = false;
      });
  }

}
