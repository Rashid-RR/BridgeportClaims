import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ClaimManager} from '../../services/claim-manager';

@Component({
  selector: 'app-carrier-modal',
  templateUrl: './carrier-modal.component.html',
  styleUrls: ['./carrier-modal.component.css']
})
export class CarrierModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CarrierModalComponent>,
              public claimManager: ClaimManager) {
  }

  ngOnInit() {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}
