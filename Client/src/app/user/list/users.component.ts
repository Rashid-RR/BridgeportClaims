import {Component, NgZone, OnInit, } from '@angular/core';
import {HttpService} from '../../services/http-service';
import {EventsService} from '../../services/events-service';
import {FormBuilder, FormGroup} from '@angular/forms';

import {User} from '../../models/user';
import {Role} from '../../models/role';
import {ToastrService} from 'ngx-toastr';
import { map } from 'rxjs/operators';
import {ConfirmComponent} from '../../components/confirm.component';
import {DialogService} from 'ng2-bootstrap-modal';
declare var $: any;

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: Array<User> = [];
  referralTypes: any[] = [];
  pageNumber: number;
  pageSize = 5;
  user_pic_hover = false;
  loading = false;
  userRole = 'User';
  indexerRole = 'Indexer';
  clientRole = 'Client';
  adminRole = 'Admin';
  isAdmin = undefined;
  userName: String = undefined;
  roles: Array<Role> = [];
  form: FormGroup;
  submitted = false;

  allUsers: any = [];
  tempAllUsers: any = [];
  selectedUsers: any = [];


  userSearchQuery = '';
  selectedUserId = '';
  activeUsers: any = [];

  constructor(
    private events: EventsService,
    private http: HttpService,
    private formBuilder: FormBuilder,
    private dialogService: DialogService,
    private toast: ToastrService,
    private zone: NgZone
  ) {
    this.form = this.formBuilder.group({
      userName: [null],
      isAdmin: [null]
    });
    this.loading = false;
    this.getRoles();
    this.loading = true;
    this.http.referralTypes({}).subscribe(res => {
      this.referralTypes = res;
      this.getUsers(1);
    }, () => {
      this.loading = false;
    });


    this.tempAllUsers = this.allUsers;

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
    this.http.getActiveUsers()
    .subscribe(result => {
      this.activeUsers = result;
    }, error1 => {

    });
  }


  getUsers(pageNumber: number) {
    this.loading = true;
    this.http.getUsers(pageNumber, this.pageSize).
    pipe(map(res => {
      this.loading = false;
      return res;
    })).subscribe((result:any[]) => {
      result.forEach(element => {
        if (element.referralTypeId) {
          element.referralType = this.referralTypes.find(r => r.referralTypeId === element.referralTypeId);
        }
        if (element.roles.includes(this.userRole)) {
          element.user = true;
        } else {
          element.user = false;
        }
        if (element.roles.includes(this.clientRole)) {
          element.client = true;
        } else {
          element.client = false;
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
    }, () => {
      this.loading = false;
    });
  }

  getReferalType(id: any) {
    const ref = this.referralTypes.find(r => r.referralTypeId === id);
    if (!ref) {
      return '';
    }
    return ref.typeName;
  }

  changeReferalType(user, ref) {
    this.loading = true;
    if (user.roles.length === 0) {
      this.toast.warning('Please assign this user to the client role, before you are able to assign a referral type to them.');
      this.loading = false;
      return;
    } else if (user.roles.indexOf(this.clientRole) === -1) {
      this.toast.warning('Unable to assign a referral type to someone who is not a member of the client role.');
      this.loading = false;
      return;
    }
    this.http.setReferralType({userId: user.id, referralTypeId: ref.referralTypeId}).subscribe(res => {
      user.referralTypeId = ref.referralTypeId;
      this.toast.success(res.message || `${user.firstName} ${user.lastName}'s referral type was updated successfully.`);
      this.loading = false;
    }, (error) => {
      this.loading = false;
      const err = error.error;
      this.toast.error(err.message);
    });
  }

  getRoles() {
    const data = '';
    this.http.getRoles(data).subscribe(result => {
      this.roles = result.reduce(function (result, role) {
        result[role.name] = {name: role.name, id: role.id, users: role.users};
        return result;
      }, {});

    }, () => {
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
    this.dialogService.addDialog(ConfirmComponent, {
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
          this.users[index].client = false;
          this.users[index].user = false;
          this.users[index].indexer = false;
          this.users[index].deactivated = true;
          this.toast.success('The user was deactivated successfully.');
        }, () => {
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
        }, () => {
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
      data = {Id: this.roles[role].id, EnrolledUsers: this.users[index].id};
    } else {
      data = {Id: this.roles[role].id, RemovedUsers: this.users[index].id};
    }
    this.processRoleChangeRequest(data, role, index, event);
  }

  showRoleConfirm(index, role, event) {
    const title = 'Update Role';
    let msg = '';
    const action = (event) ? 'Assign ' + role + ' role to ' : 'Revoke ' + role + ' role from ';

    if (event && role === this.clientRole && (this.users[index].admin || this.users[index].indexer || this.users[index].user)) {
      this.toast.warning('A user of the Bridgeport system cannot be added to the \'Client\' role.',null,{closeButton:true});
      return this.undoRole(role, index, event);
    }
    if (this.users[index].client && event && [this.indexerRole, this.userRole, this.adminRole].indexOf(role) > -1) {
      this.toast.warning('A member of the \'Client\' role cannot be an \'Admin\' or an \'Indexer\'.',null,{closeButton:true});
      return this.undoRole(role, index, event);
    }
    if (this.users[index].admin && role === this.userRole && !event) {
      msg = 'Warning, revoking ' + this.users[index].fullName + ' from the "User" role will also revoke them from the "Admin" role.';
    } else {
      msg = '' + action + this.users[index].fullName + '?';
    }
    this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    }).subscribe((isConfirmed) => {
      // We get dialog result
      if (isConfirmed) {
        this.processRoleChange(index, role, event);
      } else {
        this.undoRole(role, index, event);
      }
    });
    // We can close dialog calling disposable.unsubscribe();
    // If dialog was not closed manually close it by timeout
    // setTimeout(() => {
    //   disposable.unsubscribe();
    // }, 10000);
  }

  undoRole(role, index, event) {

    setTimeout(() => {
      if (role === this.adminRole) {
        this.users[index].admin = !event;
      }
      if (role === this.clientRole) {
        this.users[index].client = !event;
      }
      if (role === this.userRole) {
        this.users[index].user = !event;
      }
      if (role === this.indexerRole) {
        this.users[index].indexer = !event;
      }

    }, 200);
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
        if (this.users[index].admin && role === this.userRole && !event) {
          this.users[index].admin = false;
        } else if (role === this.adminRole && this.users[index].admin) {
          this.users[index].user = true;
          this.users[index].indexer = true;
        } else if (role === this.indexerRole && this.users[index].indexer) {
          this.users[index].user = true;
        }
        this.toast.success(msg);
      }, error => {
        const err = error.error;
        this.toast.error('A server error has occurred. Please contact your system administrator.');

      });
    } catch (e) {

    } finally {

    }
  }

  search() {

  }

  addUser(id) {

    this.allUsers = this.tempAllUsers;
    if (id === -1) {
      for (const user in this.allUsers) {
        this.selectedUsers.push(this.allUsers[user]);
      }
      this.allUsers = [];

    } else {
      const index = this.allUsers.findIndex(data => data.payorId === id);

      if (index > -1) {


        this.selectedUsers.push(this.allUsers[index]);

        this.allUsers.splice(index, 1);
      }

    }
    this.userSearchQuery = '';
    this.tempAllUsers = this.allUsers;

  }

  remoevUser(id) {
    if (id === -1) {
      for (const user in this.selectedUsers) {
        this.allUsers.push(this.selectedUsers[user]);
      }
      this.selectedUsers = [];
    } else {
      const index = this.selectedUsers.findIndex(data => data.payorId === id);
      this.allUsers.push(this.selectedUsers[index]);

      if (index > -1) {
        this.selectedUsers.splice(index, 1);
      }
    }
  }

  filterUsers(query) {

    if (query !== '') {
      this.allUsers = this.tempAllUsers.filter(x => x.carrier.toLocaleLowerCase().includes(query.toLocaleLowerCase()));
    } else {
      this.allUsers = this.tempAllUsers;
    }

  }

  getListofusers(id) {

    this.selectedUserId = id;
    this.allUsers = [];
    this.selectedUsers = [];

    this.http.getUsersListPerActiveUser(id)
    .subscribe(result => {
      this.allUsers = result['rightCarriers'];
      this.selectedUsers = result['leftCarriers'];
      this.tempAllUsers = this.allUsers;




    }, error1 => {

    });
  }

  saveAssignment() {
    const payorId = [];
    for (const payor in this.selectedUsers) {
      payorId.push(this.selectedUsers[payor].payorId);
    }

    this.loading = true;
    this.http.assignUsertoPayors(this.selectedUserId, payorId)
    .subscribe(result => {
      this.loading = false;
      this.toast.success(result['message']);
      $('#usersModal').modal('hide');

    }, error1 => {
      const err = error1.error;
      this.toast.error(err.message);
      this.loading = false;
    });
  }

  initilizeModal() {
    this.userSearchQuery = '';
    $('#userSelection').val(0);
    this.allUsers = [];
    this.selectedUsers = [];
  }
}
