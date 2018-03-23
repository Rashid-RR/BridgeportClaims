import { SortColumnInfo } from "../../directives/table-sort.directive";
import { Component, OnInit, Renderer2, AfterViewInit, NgZone, HostListener, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import { HttpService } from "../../services/http-service";
import { Payment } from "../../models/payment";
import { EventsService } from "../../services/events-service";
import { DatePipe, DecimalPipe } from '@angular/common';
import { ConfirmComponent } from '../../components/confirm.component';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { DialogService } from "ng2-bootstrap-modal";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { ProfileManager } from "../../services/profile-manager";
declare var $: any;

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  sortColumn: SortColumnInfo;
  editing: Boolean = false;
  loadingNotification: Boolean = false;
  editingNotificationId: any;
  form: FormGroup;
  notifications: Array<any> = [];
  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    private formBuilder: FormBuilder,
    private events: EventsService,
    private dialogService: DialogService,
    private profileManager: ProfileManager,
    private toast: ToastsManager,
    private decimalPipe: DecimalPipe,
    private http: HttpService
  ) {
    this.form = this.formBuilder.group({
      letterName: [null],
      notificationId: [null],
    });

  }

  ngOnInit() {
    this.fetchData();
  }

  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  update(payment: any) {
    this.editing = true;
    this.editingNotificationId = payment.notificationId
    let checkAmt = Number(payment.checkAmt).toFixed(2);
    let postedDate = this.dp.transform(payment.postedDate, "shortDate");
    let rxDate = this.dp.transform(payment.rxDate, "shortDate");
    this.form = this.formBuilder.group({
      letterName: [null,Validators.required],
      notificationId: [payment.notificationId,Validators.required],
    });
  }
  saveButtonClick() {
    var btn = $("#saveNotificationButton");
    if (btn.length > 0) {
      $("#saveNotificationButton").click();
    }
  }
  saveNotification(n: any) {
    if(this.form.get('letterName').value){
      this.loadingNotification = true
      this.http.saveLetterNotifications(this.form.value).single().subscribe(res=>{              
          this.toast.success('Letter name successfully updated'); 
          this.loadingNotification = false;
          for (var i=0;i<this.notifications.length;i++){
            if(this.notifications[i].notificationId==n.notificationId){ 
              this.notifications.splice(i,1);
            }
          }
          this.cancel();
      },error=>{                          
        this.toast.error('Could not update letter name');
        this.loadingNotification = false;
      });
    }else{
      this.toast.warning("You must fill amount paid, check Number and date posted to continue");
    }
  }


  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1)
  }
  cancel() {
    this.editing = false;
    this.editingNotificationId = undefined;
    this.form.patchValue({
      amountPaid: [null],
      checkNumber: [null],
      notificationId: [null],
      prescriptionId: [null],
      datePosted: [null],
    });
  }

  fetchData() {
    this.loadingNotification = true;
    this.http.getNotifications().map(res => { return res.json(); })
      .subscribe((result: any) => {
        this.loadingNotification = false;
        this.notifications = result;
      }, err => {        
          this.loadingNotification = false;
      });
  }

}

