import { Component, OnInit } from '@angular/core';
import { HttpService } from "../../services/http-service";
import { User } from "../../models/user";
import { Role } from "../../models/role"


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: Array<User> = [];
  pageNumber: number;
  pageSize: number = 5;
  loading: boolean;
  roles: Array<Role> = [];

  constructor(private http: HttpService) {
    this.loading = false;
    this.getRoles();
    this.getUsers(1)
  }

  next() {
    this.getUsers(this.pageNumber + 1);
  }
  prev() {
    if (this.pageNumber > 1) {
      this.getUsers(this.pageNumber - 1);
    }
  }

  ngOnInit() {
  }

  getUsers(pageNumber: number) {
    this.loading = true;
    this.http.getUsers(pageNumber, this.pageSize).map(res => { this.loading = false; return res.json() }).subscribe(result => {
      result.forEach(element => {
        if (element.roles.includes('User')) {
          element.user = true;
        } else {
          element.user = false;
        }
        if (element.roles.includes('Admin')) {
          element.admin = true;
        } else {
          element.admin = false;
        }
        this.users.push(element);
      });
      console.log(this.users);
      this.pageNumber = pageNumber;
    }, err => {
      console.log(err);
    })
  }

  getRoles() {
    let data = '';
    this.http.getRoles(data).map(res => { return res.json() }).subscribe(result => {
      this.roles = result.reduce(function (result, role) {
        result[role.name] = { name: role.name, id: role.id, users: role.users };
        return result;
      }, {});

    }, err => {
      console.log(err);
    })
  }

  processRoleChange(user, role, event) {
    let data;
    console.log(this.roles[role]);

    if (event) {
      data = { Id: this.roles[role].id, EnrolledUsers: [user.id] };
    } else {
      data = { Id: this.roles[role].id, RemovedUsers: [user.id] };
    }
    console.log(data);
    this.http.assignUserRole(data);
    try {
      this.http.assignUserRole(data).subscribe(res => {
        console.log("Successful updated role");
        if(role=='admin'){
          user.admin=true;
          user.user=true;
        }
      }, error => {
        let err = error.json();
        console.log(err.message);
      })
    } catch (e) {

    } finally {

    }
  }

}
