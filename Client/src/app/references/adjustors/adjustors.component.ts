import { Component, OnInit } from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';

@Component({
  selector: 'app-adjustors',
  templateUrl: './adjustors.component.html',
  styleUrls: ['./adjustors.component.css']
})
export class AdjustorsComponent implements OnInit {

  constructor(public rs: ReferenceManagerService) { }

  ngOnInit() {
  }

}
