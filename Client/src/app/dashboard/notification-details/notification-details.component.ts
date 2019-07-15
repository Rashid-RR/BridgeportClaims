import { SortColumnInfo } from '../../directives/table-sort.directive';
import { Component, OnInit } from '@angular/core';
import { HttpService, NotificationResult, PayorSearchResult } from '../../services/http-service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ProfileManager } from '../../services/profile-manager';
import { NotificationService } from '../../services/notification.service';
import { MessageResponse } from 'src/app/models/message-response';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../../components/confirm.component';

declare var $: any;

@Component({
  selector: 'app-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.css']
})
export class NotificationDetailsComponent implements OnInit {
  sortColumn: SortColumnInfo;
  editing: Boolean = false;
  loadingNotification: Boolean = false;
  editingNotificationId: any;
  form: FormGroup;
  carrierName = '';
  notifications: Array<NotificationResult> = [];
  constructor(
    private formBuilder: FormBuilder,
    private profileManager: ProfileManager,
    private dialogService: DialogService,
    private toast: ToastrService,
    private http: HttpService,
    private router: Router,
    public notificationservice: NotificationService,
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

  update(payment: NotificationResult) {
    this.editing = true;
    this.editingNotificationId = payment.notificationId;

    this.form = payment.notificationType === 'Payor Letter Name' ?
      this.formBuilder.group({
        letterName: [null, Validators.required],
        notificationId: [payment.notificationId, Validators.required],
      }) : this.formBuilder.group({
        prescriptionId: [payment.prescriptionId, Validators.required],
        billedAmount: [null, [Validators.required, Validators.min(1)]],
        payorId: [null, payment.needsCarrier ? [Validators.required] : []]
      });
  }

  onPayorSelected(payor: PayorSearchResult): void {
    this.form.controls['payorId'].patchValue(payor.payorId);
      this.carrierName = payor.carrier;
    $('#saveNotificationButton').focus();
  }
  saveButtonClick() {
    const btn = $('#saveNotificationButton');
    if (btn.length > 0 && this.form.valid) {
      $('#saveNotificationButton').click();
    }
  }
  saveLetterNotification(n: any) {
    if (this.form.get('letterName').value) {
      this.loadingNotification = true;
      this.http.saveLetterNotifications(this.form.value).subscribe(res => {
        this.toast.success('Letter name successfully updated');
        this.loadingNotification = false;
        for (let i = 0; i < this.notifications.length; i++) {
          if (this.notifications[i].notificationId === n.notificationId) {
            this.notifications.splice(i, 1);
          }
        }
        this.cancel();
        this.fetchData();
      }, () => {
        this.toast.error('Could not update letter name');
        this.loadingNotification = false;
      });
    } else {
      this.toast.warning('You must fill in the letter name to continue');
    }
  }
  saveEnvisionNotification(n: any) {
    if (this.form.valid) {
      const form = this.form.value;
      form.billedAmount = Number($('#billedAmount').val());
      this.dialogService.addDialog(ConfirmComponent, {
        title: 'Envision Information',
        message: `Are you sure you wish to save Billed Amount
        <b>$${form.billedAmount}</b>
        ${form.payorId ? 'and Carrier <b>' + this.carrierName + '</b>' : ''}
        to this prescription?`
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            // Use jquery to get value since the input mask is losing the decimal place
            this.loadingNotification = true;
            this.http.dismissEnvisionNotification(form).subscribe((res: MessageResponse) => {
              this.toast.success(res.message);
              this.loadingNotification = false;
              for (let i = 0; i < this.notifications.length; i++) {
                if (this.notifications[i].notificationId === n.notificationId) {
                  this.notifications.splice(i, 1);
                }
              }
              this.cancel();
              this.fetchData();
            }, () => {
              this.toast.error('Could not update prescription');
              this.loadingNotification = false;
            });
          }
        });
    } else {
      const warning = `Please adjust the following to save:\<br>
        ${this.form.controls['payorId'].errors ? 'Provide a Carrier<br>' : ''}
        ${this.form.controls['billedAmount'].errors && this.form.controls['billedAmount'].errors.required ? 'Billed amount is required<br>' : ''}
        ${this.form.controls['billedAmount'].errors && this.form.controls['billedAmount'].errors.pattern ? 'Provide a valid  billed amount<br>' : ''}
        ${this.form.controls['billedAmount'].errors && this.form.controls['billedAmount'].errors.min ? 'Billed amount entered is too low<br>' : ''}
        <br><b>Please correct the above to save</b>
      `;
      this.toast.warning(warning, 'You got error in your input!', { enableHtml: true });
    }
  }
  dismissNotification(n: NotificationResult) {
    this.loadingNotification = true;
    this.http.dismissNotification(n.notificationId).subscribe(res => {
      this.toast.success(res.message || 'Notification dismissed');
      this.loadingNotification = false;
      for (let i = 0; i < this.notifications.length; i++) {
        if (this.notifications[i].notificationId === n.notificationId) {
          this.notifications.splice(i, 1);
        }
      }
      this.cancel();
      this.fetchData();
    }, (error) => {
      const err = error.error || ({ 'Message': 'Could not dismiss notification!' });
      this.toast.error(err.Message || 'Could not dismiss notification!');
      this.loadingNotification = false;
    });
  }
  openClaim(n: NotificationResult) {
    this.router.navigate(['/main/claims'], { queryParams: { claimId: n.claimId } });
  }

  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles
      instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
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
    this.http.getNotifications()
      .subscribe((result: NotificationResult[]) => {
        this.loadingNotification = false;
        this.notifications = result;
        this.notificationservice.updateNotifications(result);
      }, () => {
        this.loadingNotification = false;
      });
  }

}
