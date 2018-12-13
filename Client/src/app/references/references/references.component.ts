import {Component, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastsManager} from 'ng2-toastr';

@Component({
  selector: 'app-references',
  templateUrl: './references.component.html',
  styleUrls: ['./references.component.css']
})
export class ReferencesComponent implements OnInit {

  constructor(public rs: ReferenceManagerService,
              private toaster: ToastsManager) {
  }

  ngOnInit() {

  }

}
