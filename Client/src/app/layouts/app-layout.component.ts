import {Component, OnInit} from "@angular/core";
import {Router} from "@angular/router";
@Component({
  selector: 'chat-layout',
  templateUrl: "./chat-layout.component.html",
  styleUrls: ["./chat-layout.component.css"]
})
export class AppLayoutComponent implements OnInit {
   constructor(private router: Router) { }

  ngOnInit() {
 
  }

   

}
