import { Component, OnInit } from "@angular/core";
import { EventsService } from "../../services/events-service";
import { ProfileManager } from "../../services/profile-manager";
import { HttpService } from "../../services/http-service";
import { Router } from "@angular/router";
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  date: string;
  private getDateSub: Subscription;
  constructor(
    private http: HttpService,
    private router: Router,
    public eventservice: EventsService,
    public profileManager: ProfileManager
  ) { }


  private fetchDate(){
    this.getDateSub = this.http.getDate().subscribe(result => {
      this.date = result.json().message;
    });
  }
  
  ngOnInit() {
    // this.date = this.http.date;
    if (this.profileManager.profile) {
      this.fetchDate();
    }
    this.profileManager.profileChanged.subscribe((profile) => {
      if (profile) {
        if (!this.getDateSub) {
          this.fetchDate();
        }
      } else if (this.getDateSub) {
        this.getDateSub.unsubscribe();
        this.getDateSub = null;
      }
    });
  }

  logout() {
    this.eventservice.broadcast("logout", true);
    this.profileManager.profile = undefined;
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
    /* this.http.logout().subscribe(res=>{
         console.log(res);
     });*/
  }
}
