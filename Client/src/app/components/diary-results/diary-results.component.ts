import { Component, OnInit } from '@angular/core';
import {DiaryService} from "../../services/services.barrel";

@Component({
  selector: 'app-diary-results',
  templateUrl: './diary-results.component.html',
  styleUrls: ['./diary-results.component.css']
})
export class DiaryResultsComponent implements OnInit {

  constructor(public diaryService:DiaryService) {
    
   }

  ngOnInit() {
  }

}
