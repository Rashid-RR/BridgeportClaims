import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd, NavigationStart } from '@angular/router';
import { Http, Headers } from "@angular/http";
import { HttpService } from "../../services/http-service"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';


@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  code: any;
  user: any;
  confirmed: Number = 0;
  loading = true;
  private hashChange: any;
  constructor(
    private route: ActivatedRoute,
    private http: HttpService,
    private req: Http,
    private router: Router,
    private toast: ToastsManager
  ) {
    router.routerState.root.queryParams.subscribe(
      data => {
        this.code = encodeURIComponent(data['code']); this.user = data['userId']
      });

  }

  ngOnInit() {
    try {
      this.http.confirmEmail(this.user, this.code).subscribe(res => {
        this.toast.success('Thank you for confirming your email. Please proceed to login');
        this.loading = false;
        this.confirmed = 1;
        this.router.navigate(['/login']);
      }, error => {
        let err = error.json() || ({ "Message": "Server error!" });
        this.toast.error(err.Message);
        this.loading = false;
      })
    } catch (e) {
      this.toast.error('Error in fields. Please correct to proceed!');
    }
  }
}
