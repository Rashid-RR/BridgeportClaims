import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ClaimManager} from '../../services/claim-manager';
import {Router} from '@angular/router';
import {HttpService} from '../../services/http-service';
import {ReferenceManagerService} from '../../services/reference-manager.service';

@Component({
  selector: 'app-carrier-modal',
  templateUrl: './carrier-modal.component.html',
  styleUrls: ['./carrier-modal.component.css']
})
export class CarrierModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CarrierModalComponent>,
              private router: Router,
              private http: HttpService,
              public rs: ReferenceManagerService,
              public claimManager: ClaimManager) {
    this.http.getStates({}).subscribe(data => {
      this.rs.states = data;
    }, error => {
    });
  }

  ngOnInit() {
    // console.log(this.claimManager.claimsData[0]);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  convertPhoneNumber(num: any) {
    let convertedNumber;
    if (num.length === 10) {
      convertedNumber = '(' + num.substring(0, 3) + ') ' + num.substring(3, 6) + '-' + num.substring(6, 10);
      return convertedNumber;

    } else if (num.length === 11) {
      convertedNumber = num.substring(0, 1) + '-(' + num.substring(1, 4) + ') ' + num.substring(4, 7) + '-' + num.substring(7, 11);
      return convertedNumber;

    }
  }

  showRefrence(payorId: any) {
    this.dialogRef.close();
    this.router.navigate(['main/refrences'], {queryParams: {payorId: payorId}});
  }
}
