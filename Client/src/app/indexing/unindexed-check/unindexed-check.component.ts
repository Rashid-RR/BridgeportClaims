import { Component, Input,Output,EventEmitter, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, Validators,FormGroup } from '@angular/forms';
import { EventsService } from "../../services/events-service"
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from '../../models/document';
import { HttpService } from "../../services/http-service";
import { ToastsManager } from 'ng2-toastr';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../../components/confirm.component';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
declare var $: any;


@Component({
  selector: 'indexing-unindexed-check',
  templateUrl: './unindexed-check.component.html',
  styleUrls: ['./unindexed-check.component.css']
})
export class UnindexedCheckComponent implements OnInit , AfterViewInit {

  file: DocumentItem;
  form: FormGroup;
  @Input() checkType;
  @Input() checkNumber;
  @Input() index;
  @Output() getSearchStatusChange = new EventEmitter<string>();

  @ViewChild('checkSwal') private checkSwal: SwalComponent;
  constructor(
    private dialogService: DialogService,
    public readonly swalTargets: SwalPartialTargets,
    private http: HttpService, private route: ActivatedRoute,
    private toast: ToastsManager, private formBuilder: FormBuilder, public ds: DocumentManagerService, private events: EventsService) {
    this.form = this.formBuilder.group({
      checkNumber: [null, Validators.compose([Validators.min(3),Validators.required])]
    });

  }
  ngOnInit() {
    this.form.patchValue({ checkNumber: this.checkNumber });
    if (!this.checkNumber) {
      this.events.broadcast("reset-indexing-form", true);
      this.ds.cancel('checks');
    }
    this.events.on("archived-image", (id: any) => {
      this.ds.cancel('checks');
    });
  }
  ngAfterViewInit() {

  }
  setCurrent(current:string){
    this.getSearchStatusChange.emit(current);

  }

  saveCheck() {
    this.form.markAsDirty();
    this.form.controls.checkNumber.markAsTouched();
    if (this.form.valid) {
      this.ds.loading = true;
      try {
            let data = this.form.value;
            data.documentId = this.ds.checksFile.documentId;
            this.ds.loading = true;
            this.http.saveCheckIndex(data).subscribe(res => {
              this.toast.success(res.message);
              this.ds.loading = false;
              this.form.reset();
              this.ds.checks = this.ds.checks.delete(this.ds.checksFile.documentId);
              this.ds.totalCheckRowCount--;
              this.ds.newCheck = false;
              this.ds.checksFile = undefined;
              this.ds.loading = false;
              this.ds.closeModal();
            }, requestError => {
              let err = requestError.error;
              this.toast.error(err.Message);
              this.ds.loading = false;
            })
      } catch (e) {
        this.ds.loading = false;
      } finally {

      }
    } else {
      let er = '';
      this.ds.loading = false;
      this.toast.warning(er, 'Please provide a Check # with a least 3 Characters....', { enableHTML: true });
    }
  }
  archive() {
    this.dialogService.addDialog(ConfirmComponent, {
      title: "Archive Image",
      message: "Are you sure you wish to archive " + (this.ds.checksFile.fileName || '') + "?"
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.ds.archive(this.ds.checksFile.documentId,false,'searchCheckes');
        }
      });
  }
  cancel() {
    this.form.reset();
    this.ds.cancel('checks');
  }
}

