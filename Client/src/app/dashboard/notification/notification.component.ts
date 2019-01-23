import { SortColumnInfo } from '../../directives/table-sort.directive';
import { Component, OnInit} from '@angular/core';
import { HttpService } from '../../services/http-service';
import { FormBuilder,  FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProfileManager } from '../../services/profile-manager';
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
    private formBuilder: FormBuilder,
    private profileManager: ProfileManager,
    private toast: ToastrService,
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
    this.editingNotificationId = payment.notificationId;
    this.form = this.formBuilder.group({
      letterName: [null, Validators.required],
      notificationId: [payment.notificationId, Validators.required],
    });
  }
  saveButtonClick() {
    const btn = $('#saveNotificationButton');
    if (btn.length > 0) {
      $('#saveNotificationButton').click();
    }
  }
  saveNotification(n: any) {
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
      }, () => {
        this.toast.error('Could not update letter name');
        this.loadingNotification = false;
      });
    } else {
      this.toast.warning('You must fill amount paid, check Number and date posted to continue');
    }
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
      .subscribe((result: any) => {
        this.loadingNotification = false;
        this.notifications = result;
      }, () => {
          this.loadingNotification = false;
      });
  }

}

