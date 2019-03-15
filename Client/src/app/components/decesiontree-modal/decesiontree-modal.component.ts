import {Component, Inject, OnInit} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import {HttpService} from '../../services/http-service';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ClaimManager} from '../../services/claim-manager';

@Component({
  selector: 'app-decesiontree-modal',
  templateUrl: './decesiontree-modal.component.html',
  styleUrls: ['./decesiontree-modal.component.css']
})
export class DecesiontreeModalComponent implements OnInit {



  constructor(public dialogRef: MatDialogRef<DecesiontreeModalComponent>,
              private http: HttpService,
              public rs: ReferenceManagerService,
              public claimManager: ClaimManager,
              @Inject(MAT_DIALOG_DATA) data) {
    console.log(data.description);
    console.log(data);

  }

  ngOnInit() {

  }

  onNoClick(): void {
    this.dialogRef.close();
  }


}
