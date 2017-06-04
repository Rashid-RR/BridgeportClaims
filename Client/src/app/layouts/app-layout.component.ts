import {Component, OnInit} from "@angular/core";
import {Router} from "@angular/router";
import {ProfileManager} from "../services/profile-manager"
@Component({
  selector: 'app-layout',
  templateUrl: "./app-layout.component.html",
  styleUrls: ["./app-layout.component.css"]
})
export class AppLayoutComponent implements OnInit {
  buildSha: '';
  buildDate: '';
  constructor(private router: Router,private profileManager:ProfileManager) { 
    
  }

  ngOnInit() {
 
  }

   get isLoggedIn():boolean{
    if(this.profileManager.profile){
        window['jQuery']('body').addClass('sidebar-mini');
        return true;
    }else{
        window['jQuery']('body').removeClass('sidebar-mini');
        window['jQuery']('body').addClass('sidebar-collapse');
        return false;
    }
  }

}
