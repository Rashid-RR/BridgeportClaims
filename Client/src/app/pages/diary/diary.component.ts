import { Component, OnInit } from '@angular/core';
// Services
import { DiaryService } from "../../services/diary.service";

@Component({
  selector: 'app-diary',
  templateUrl: './diary.component.html',
  styleUrls: ['./diary.component.css']
})
export class DiaryComponent implements OnInit {

  constructor(
    public ds: DiaryService
  ) { 
    
  }

  ngOnInit() {
    this.ds.search();
  }

}
