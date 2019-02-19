import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {Router} from '@angular/router';
import {HttpService} from '../../services/http-service';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ClaimManager} from '../../services/claim-manager';

@Component({
  selector: 'app-attorney-modal',
  templateUrl: './attorney-modal.component.html',
  styleUrls: ['./attorney-modal.component.css']
})
export class AttorneyModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AttorneyModalComponent>,
              private router: Router,
              private http: HttpService,
              public rs: ReferenceManagerService,
              public claimManager: ClaimManager) {
    this.http.getStates({}).subscribe(data => {
      this.rs.states = data;
    }, error => {
    });
  }

  ngOnInit() { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  showReference(attorneyId: number) {
    this.dialogRef.close();
    this.router.navigate(['main/refrences'], { queryParams: { attorneyId: attorneyId } });
  }
}
