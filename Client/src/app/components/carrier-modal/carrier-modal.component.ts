import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { ReferenceManagerService } from '../../services/reference-manager.service';

@Component({
  selector: 'app-carrier-modal',
  templateUrl: './carrier-modal.component.html',
  styleUrls: ['./carrier-modal.component.css']
})
export class CarrierModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CarrierModalComponent>,
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
    window.open('#/main/references/?payorId=' + payorId, '_blank');
  }
}
