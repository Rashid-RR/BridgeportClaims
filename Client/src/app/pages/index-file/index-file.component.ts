import { Component, OnInit } from '@angular/core';
import { DocumentItem } from 'app/models/document';
import {Router} from "@angular/router";

@Component({
  selector: 'app-index-file',
  templateUrl: './index-file.component.html',
  styleUrls: ['./index-file.component.css']
})
export class IndexFileComponent implements OnInit {

  file:DocumentItem
  constructor(
    private router:Router
  ) { }

  ngOnInit() {
      this.router.routerState.root.queryParams.subscribe(params => {
        if(params['id']){          
            let file = localStorage.getItem("file-"+params['id']);
            if(file){
              this.file = JSON.parse(file) as DocumentItem;
            }
            console.log(file);
        }
    });
  }

}
