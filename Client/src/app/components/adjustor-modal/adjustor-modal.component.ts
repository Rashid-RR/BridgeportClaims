import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { ReferenceManagerService } from '../../services/reference-manager.service';

@Component({
    selector: 'app-adjustor-modal',
    templateUrl: './adjustor-modal.component.html',
    styleUrls: ['./adjustor-modal.component.css']
})
export class AdjustorModalComponent implements OnInit {

    constructor(public dialogRef: MatDialogRef<AdjustorModalComponent>,
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

    showReference(adjustorId: number) {
        this.dialogRef.close();
        window.open('#/main/references/?adjustorId=' + adjustorId, '_blank');
    }
}
