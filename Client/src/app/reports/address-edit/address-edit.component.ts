import { HttpService } from './../../services/http-service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-address-edit',
  templateUrl: './address-edit.component.html',
  styleUrls: ['./address-edit.component.css']
})
export class AddressEditComponent implements OnInit {

  constructor(private http: HttpService) { }

  ngOnInit() {
    this.http.getNotifications().subscribe(res => console.log(res));
  }

}
