import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";

@Component({
  selector: 'app-claim-result',
  templateUrl: './claim-result.component.html',
  styleUrls: ['./claim-result.component.css']
})
export class ClaimResultComponent implements OnInit {

  constructor(public claimManager:ClaimManager,private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService) {
    
  }

  ngOnInit() {
    
  }

}
