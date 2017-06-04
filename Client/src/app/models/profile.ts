// profile.ts
import {UUID} from "angular2-uuid";

export class Module{
  id:UUID;
  name:string;
  constructor(id:UUID,name:string){
      this.id=id;
      this.name=name;
  }
}
export class Role{
  id:UUID;
  canView:boolean;
  canEdit:boolean;
  canAdd:boolean;
  canDelete:boolean;
  module:Module;
  constructor(id:UUID,canView:boolean,canEdit:boolean,canAdd:boolean,canDelete:boolean,module:Module){
    this.id=id;
    this.canAdd=canAdd;
    this.canEdit=canEdit;
    this.canView=canView;
    this.canDelete=canDelete;
    this.module=module;
  }
}
export class UserProfile {
  id: UUID;
  login: string;
  userName: string;
  displayName: string;
  avatarUrl?: URL;
  email: string;
  createdOn: Date;
  roles?:Array<Role>;
constructor(id:UUID,login:string,displayName:string,email:string,userName?:string,avatarUrl?:URL,createdOn?:Date,roles?:Array<Role>){
    this.id=id;
    this.login=login;
    this.userName=userName;
    this.displayName=displayName;
    this.avatarUrl=avatarUrl;
    this.email=email;
    this.roles=roles;
  }
}
