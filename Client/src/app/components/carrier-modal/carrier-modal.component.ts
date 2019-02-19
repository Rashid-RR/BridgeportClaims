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

  showReference(payorId: any) {
    this.dialogRef.close();
    this.router.navigate(['main/references'], {queryParams: {payorId: payorId}});
  }
}
