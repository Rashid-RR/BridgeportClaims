import {Component, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';

@Component({
  selector: 'app-references-container',
  templateUrl: './references-container.component.html',
  styleUrls: ['./references-container.component.css']
})
export class ReferencesContainerComponent implements OnInit {

  constructor(public rs: ReferenceManagerService) {
  }

  ngOnInit() {

  }
}
