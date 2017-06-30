import { Component, OnInit } from '@angular/core';
import { HttpService } from "../../services/http-service";
import { User } from "../../models/user";
import { Role } from "../../models/role"

import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from "ng2-bootstrap-modal";

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
  userRole = 'User';
  adminRole = 'Admin';
  roles: Array<Role> = [];

  constructor(private http: HttpService, private dialogService: DialogService) {
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
        if (element.roles.includes(this.userRole)) {
          element.user = true;
        } else {
          element.user = false;
        }
        if (element.roles.includes(this.adminRole)) {
          element.admin = true;
        } else {
          element.admin = false;
        }
        this.users.push(element);
      });  
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


  processRoleChange(index, role, event) {
    let data;    

    if (event) {
      data = { Id: this.roles[role].id, EnrolledUsers: this.users[index].id };
    } else {
      data = { Id: this.roles[role].id, RemovedUsers: this.users[index].id };
    }
    this.processRoleChangeRequest(data,role,index);
  }

  showRoleConfirm(index, role, event) {
    let title = 'Update Role';
    let action = (event)?'Assgin '+role+' role to ':'Revoke '+role+' role from ';
    let msg = 'Please confirm to '+action+this.users[index].fullName;

    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {
          this.processRoleChange(index, role, event);
        }
        else {          
          if(role == this.adminRole){
            this.users[index].admin = !event;
          }
          if(role == this.userRole){
            this.users[index].user = !event;
          }          
        }
      });
    //We can close dialog calling disposable.unsubscribe();
    //If dialog was not closed manually close it by timeout
    // setTimeout(() => {
    //   disposable.unsubscribe();
    // }, 10000);
  }

  processRoleChangeRequest(data,role,index) {
    // this.http.assignUserRole(data);
    try {
      this.http.assignUserRole(data).subscribe(res => {
        console.log("Successful updated role");
        if (this.users[index].admin) {
          this.users[index].user = true;
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