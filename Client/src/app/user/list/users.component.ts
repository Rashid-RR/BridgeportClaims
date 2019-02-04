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
    }, error => {
    });
  }

  getUsers(pageNumber: number) {
    this.loading = true;
    this.http.getUsers(pageNumber, this.pageSize).
    pipe(map(res => {
      this.loading = false;
      return res;
    })).subscribe((result: any[]) => {
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

  changeReferalType(user: { roles: { length: number; indexOf: (arg0: string) => number; }; id: any;
    referralTypeId: any; firstName: any; lastName: any; }, ref: { referralTypeId: any; }) {
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
      this.roles = result.reduce(function (result: { [x: string]: { name: any; id: any; users: any; }; },
        role: { name: string | number; id: any; users: any; }) {
        result[role.name] = {name: role.name, id: role.id, users: role.users};
        return result;
      }, {});
    }, () => {
    });
  }

  changeStatus(userName: string, event: any) {
    let title = '';
    let msg = '';
    const user = this.users.find(x => x.userName === userName);
    if (!event) {
      title = 'Deactivate User';
      msg = 'Are you sure you want to  deactivate ' + user.fullName + ' from the entire site?';
    } else {
      title = 'Activate User';
      msg = 'Are you sure you want to  activate ' + user.fullName + ' to use the entire site?';
    }
    this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.processStatusChange(userName, event);
        } else {
          user.deactivated = !event;
        }
      });
  }

  processStatusChange(userName: string, event: any) {
    if (!event) {
      try {
        const user = this.users.find(x => x.userName === userName);
        this.http.deactivateUser(user.id).subscribe(res => {
          user.admin = false;
          user.client = false;
          user.user = false;
          user.indexer = false;
          user.deactivated = true;
          this.toast.success('The user was deactivated successfully.');
        }, () => {
          this.toast.error('A server error has occurred. Please contact your system administrator.');
        });
      } catch (e) {
        this.toast.error('A server error has occurred. Please contact your system administrator.');
      }
    } else {
      try {
        const user = this.users.find(x => x.userName === userName);
        this.http.activateUser(user.id).subscribe(res => {
          user.user = true;
          this.toast.success('The user was activated sucessfully.');
        }, () => {
          this.toast.error('A server error has occurred. Please contact your system administrator.');
        });
      } catch (e) {
        this.toast.error('A server error has occurred. Please contact your system administrator.');
      }
    }
  }

  processRoleChange(userName: string, role: string, event: any) {
    let data: { Id?: String; EnrolledUsers?: String; RemovedUsers?: String; role?: any; };
    const user = this.users.find(x => x.userName === userName);
    if (event) {
      data = {Id: this.roles[role].id, EnrolledUsers: user.id};
    } else {
      data = {Id: this.roles[role].id, RemovedUsers: user.id};
    }
    this.processRoleChangeRequest(data, role, userName, event);
  }

  showRoleConfirm(userName: string, role: string, event: any): void {
    const title = 'Update Role';
    let msg = '';
    const action = (event) ? 'Assign ' + role + ' role to ' : 'Revoke ' + role + ' role from ';
    const user = this.users.find(u => u.userName === userName);
    if (user) {
      if (user.deactivated) {
        this.toast.warning('This user has been deactivated, and therefore cannot be assigned to any roles.');
        return this.undoRole(role, userName, event);
      }
      if (event && role === this.clientRole && (user.admin || user.indexer || user.user)) {
        this.toast.warning('A user of the Bridgeport system cannot be added to the \'Client\' role.', null, {closeButton: true});
        return this.undoRole(role, userName, event);
      }
      if (user.client && event && [this.indexerRole, this.userRole, this.adminRole].indexOf(role) > -1) {
        this.toast.warning('A member of the \'Client\' role cannot be an \'Admin\' or an \'Indexer\'.', null, {closeButton: true});
        return this.undoRole(role, userName, event);
      }
      if (user.admin && role === this.userRole && !event) {
        msg = 'Warning, revoking ' + user.fullName + ' from the "User" role will also revoke them from the "Admin" role.';
      } else {
        msg = '' + action + user.fullName + '?';
      }
    } else {
      this.toast.error('Error, could not find user.');
      return;
    }
    this.dialogService.addDialog(ConfirmComponent, {
      title: title,
      message: msg
    }).subscribe((isConfirmed) => {
      // We get dialog result
      if (isConfirmed) {
        this.processRoleChange(userName, role, event);
      } else {
        this.undoRole(role, userName, event);
      }
    });
  }

  undoRole(role: string, userName: string, event: any) {
    setTimeout(() => {
      const user = this.users.find(u => u.userName === userName);
      if (role === this.adminRole) {
        user.admin = !event;
      }
      if (role === this.clientRole) {
        user.client = !event;
      }
      if (role === this.userRole) {
        user.user = !event;
      }
      if (role === this.indexerRole) {
        user.indexer = !event;
      }
    }, 200);
  }

  processRoleChangeRequest(data: { Id?: String; EnrolledUsers?: String; RemovedUsers?: String; role?: any; },
     role: string, userName: string, event: any) {
    let msg = '';
    const user = this.users.find(u => u.userName === userName);
    if (event) {
      msg = 'Assigned ' + user.firstName + ' ' + user.lastName + ' to the ' + role + ' role Successfully';
    } else {
      msg = 'Removed ' + user.firstName + ' ' + user.lastName + ' from the ' + role + ' role Successfully';
    }
    try {
      data.role = role;
      const request = event ? this.http.smartAsignRole(data) : this.http.assignUserRole(data);
      request.subscribe(res => {
        if (user.admin && role === this.userRole && !event) {
          user.admin = false;
        } else if (role === this.adminRole && user.admin) {
          user.user = true;
          user.indexer = true;
        } else if (role === this.indexerRole && user.indexer) {
          user.user = true;
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

  addUser(id: number) {
    this.allUsers = this.tempAllUsers;
    if (id === -1) {
      for (const user of this.allUsers) {
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

  remoevUser(id: number) {
    if (id === -1) {
      for (const user of this.selectedUsers) {
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

  getListofusers(id: string) {
    this.selectedUserId = id;
    this.allUsers = [];
    this.selectedUsers = [];
    this.http.getUsersListPerActiveUser(id)
    .subscribe(result => {
      this.allUsers = result['rightCarriers'];
      this.selectedUsers = result['leftCarriers'];
      this.tempAllUsers = this.allUsers;
    }, error => {
    });
  }

  saveAssignment() {
    const payorId = [];
    for (const payor of this.selectedUsers) {
      payorId.push(this.selectedUsers[payor].payorId);
    }
    this.loading = true;
    this.http.assignUsertoPayors(this.selectedUserId, payorId)
    .subscribe(result => {
      this.loading = false;
      this.toast.success(result['message']);
      $('#usersModal').modal('hide');
    }, error => {
      const err = error.error;
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
