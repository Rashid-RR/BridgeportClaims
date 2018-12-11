import { Component, OnInit } from '@angular/core';
import {HttpService} from '../../services/http-service';
import {ReferenceManagerService} from '../../services/reference-manager.service';

@Component({
  selector: 'app-references',
  templateUrl: './references.component.html',
  styleUrls: ['./references.component.css']
})
export class ReferencesComponent implements OnInit {

  constructor(public rs: ReferenceManagerService) { }

  ngOnInit() {
   ;

  }

}
