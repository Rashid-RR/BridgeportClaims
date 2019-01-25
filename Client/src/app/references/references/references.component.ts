import {Component, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-references',
  templateUrl: './references.component.html',
  styleUrls: ['./references.component.css']
})
export class ReferencesComponent implements OnInit {

  constructor(public rs: ReferenceManagerService,
              private toaster: ToastrService) {
  }

  ngOnInit() {

  }
}
