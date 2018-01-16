import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { EventsService } from '../../services/events-service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { User } from '../../models/user';
import { Role } from '../../models/role';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: Array<User> = [];
  pageNumber: number;
  pageSize = 5;
  loading = false;
  userRole = 'User';
  indexerRole = 'Indexer';
  adminRole = 'Admin';
  isAdmin = undefined;
  userName: String = undefined;
  roles: Array<Role> = [];
  form: FormGroup;
  submitted = false;
  constructor(
    private events: EventsService,
    private http: HttpService,
    private formBuilder: FormBuilder,
    private dialogService: DialogService,
    private toast: ToastsManager
  ) {
    this.form = this.formBuilder.group({
      userName: [null],
      isAdmin: [null]
    });
    this.loading = false;
    this.getRoles();
    this.getUsers(1);
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
    this.events.on('loading-error', (v) => {
        this.loading = false;
    });
  }

  getUsers(pageNumber: number) {
    this.loading = true;
    this.http.getUsers(pageNumber, this.pageSize).map(res => { this.loading = false; return res.json(); }).subscribe(result => {
      result.forEach(element => {

        if (element.roles.includes(this.userRole)) {
          element.user = true;
        } else {
          element.user = false;
        }
        if (element.roles.includes(this.indexerRole)) {
          element.indexer = true;
        } else {
          element.indexer = false;
        }
        if (element.roles.includes(this.adminRole)) {
          element.admin = true;
        } else {
          element.admin = false;
        }
        this.users.push(element);
      });
      this.pageNumber = pageNumber;
      this.loading = false;
    }, err => {
      this.loading = false;
    }, () => {
      this.loading = false;
    });
  }


  getRoles() {
    const data = '';
    this.http.getRoles(data).map(res => { return res.json(); }).subscribe(result => {
      this.roles = result.reduce(function (result, role) {
        result[role.name] = { name: role.name, id: role.id, users: role.users };
        return result;
      }, {});

    }, err => {
    });
  }

  changeStatus(index, event) {
    let title = '';
    let msg = '';
    if (!event) {
      title = 'Deactivate User';
      msg = 'Are you sure you want to  deactivate ' + this.users[index].fullName + ' from the entire site?';
    } else {
      title = 'Activate User';
      msg = 'Are you sure you want to  activate ' + this.users[index].fullName + ' to use the entire site?';
    }
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.processStatusChange(index, event);
        } else {
          this.users[index].deactivated = !event;
        }
      });
  }
  processStatusChange(index, event) {
    if (!event) {
      try {
        this.http.deactivateUser(this.users[index].id).subscribe(res => {
          this.users[index].admin = false;
          this.users[index].user = false;
          this.users[index].indexer = false;
          this.users[index].deactivated = true;
          this.toast.success('The user was deactivated successfully.');
        }, error => {
          const err = error.json();
          this.toast.error('A server error has occurred. Please contact your system administrator.');
        });
      } catch (e) {
        this.toast.error('A server error has occurred. Please contact your system administrator.');
      }
    } else {
      try {
        this.http.activateUser(this.users[index].id).subscribe(res => {
          this.users[index].user = true;
          this.toast.success('The user was activated sucessfully.');
        }, error => {
          const err = error.json();
          this.toast.error('A server error has occurred. Please contact your system administrator.');
        });
      } catch (e) {
        this.toast.error('A server error has occurred. Please contact your system administrator.');
      }
    }
  }
  processRoleChange(index, role, event) {
    let data;

    if (event) {
      data = { Id: this.roles[role].id, EnrolledUsers: this.users[index].id };
    } else {
      data = { Id: this.roles[role].id, RemovedUsers: this.users[index].id };
    }
    this.processRoleChangeRequest(data, role, index, event);
  }

  showRoleConfirm(index, role, event) {
    const title = 'Update Role';
    let msg = '';
    const action = (event) ? 'Assign ' + role + ' role to ' : 'Revoke ' + role + ' role from ';
    // console.log(this.users[index].admin , this.users[index].user , role , this.userRole ,event);
    if (this.users[index].admin && role == this.userRole && !event) {
      msg = 'Warning, revoking ' + this.users[index].fullName + ' from the "User" role will also revoke them from the "Admin" role.';
    } else {
      msg = '' + action + this.users[index].fullName + '?';
    }



    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    })
      .subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.processRoleChange(index, role, event);
        } else {
          if (role == this.adminRole) {
            this.users[index].admin = !event;
          }
          if (role == this.userRole) {
            this.users[index].user = !event;
          }
          if (role == this.indexerRole) {
            this.users[index].indexer = !event;
          }
        }
      });
    // We can close dialog calling disposable.unsubscribe();
    // If dialog was not closed manually close it by timeout
    // setTimeout(() => {
    //   disposable.unsubscribe();
    // }, 10000);
  }

  processRoleChangeRequest(data, role, index, event) {
    let msg = '';
    if (event) {
      msg = 'Assigned ' + this.users[index].firstName + ' ' + this.users[index].lastName + ' to the ' + role + ' role Successfully';
    } else {
      msg = 'Removed ' + this.users[index].firstName + ' ' + this.users[index].lastName + ' from the ' + role + ' role Successfully';
    }
    try {
      data.role = role;
      const request = event ? this.http.smartAsignRole(data) : this.http.assignUserRole(data);
      request.subscribe(res => {
        if (this.users[index].admin && role == this.userRole && !event) {
          this.users[index].admin = false;
        } else if (role == this.adminRole && this.users[index].admin) {
          this.users[index].user = true;
          this.users[index].indexer = true;
        } else if (role == this.indexerRole && this.users[index].indexer) {
          this.users[index].user = true;
        }
        this.toast.success(msg);

      }, error => {
        const err = error.json();
        this.toast.error('A server error has occurred. Please contact your system administrator.');
        console.log(err.message);
      });
    } catch (e) {
        console.log(e);
    } finally {

    }
  }

  search() {
    console.log(this.form.value);
  }
}