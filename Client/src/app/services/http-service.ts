// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import { Response } from "@angular/http/src/static_response";
import { Http, RequestOptions, Headers } from "@angular/http";
import { UUID } from "angular2-uuid";
import {Router} from "@angular/router";
import {EventsService} from "./events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class HttpService {
  baseUrl: string = "/api";

  token: String
  constructor(private router:Router,private http: Http,private events:EventsService,private toast: ToastsManager) {

  }
  setAuth(auth: String) {
    this.token = auth;
  }
  login(data, headers): Observable<Response> {
    return this.http.post("/oauth/token", data, { headers: headers })
  }

  logout(): Observable<Response> {
    return this.http.get(this.baseUrl + "/users/logout");
  }

  updateProfile(data): Observable<Response> {
    let s = this.http.patch(this.baseUrl + "/users", data)    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  passwordreset(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/passwordreset", data)    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  resetpassword(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/account/resetpassword", data)    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  forgotpassword(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/account/forgotpassword", data)    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  changepassword(data): Observable<Response> {
    let s = this.http.put(this.baseUrl + "/account/changepassword", data, { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  changeusername(firstName, lastName, id): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/users/updatename/" + id + "?firstName=" + firstName + "&lastName=" + lastName, '', { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    return s;
  }
  //register user
  register(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/account/create", data)    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    });
    
    return s;
  }
  getClaimsData(data: any) {
    let s = this.http.post(this.baseUrl + "/Claims/GetClaimsData", data, { headers: this.headers })   
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    return s;
  }
  //get user using id
  userFromId(id: UUID): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    
    return s;
  }
  confirmEmail(id: any, code: any): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/ConfirmEmail?userId="+id + "&code=" + code, { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    return s;
  }
  profile(): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    return s;
  }
  getPayours(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })    
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })
    return s;
  }
  getUsers(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/account/users/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })
    .catch(err =>  { 
      this.handleResponseError(err);
      return Observable.throw(err);
    })    
    return s;
  }
  getRoles(data: any): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/roles/", data, { headers: this.headers })    
    return s;
  }
  assignUserRole(data: any) {
    let s =  this.http.post(this.baseUrl + "/roles/ManageUsersInRole", data, { headers: this.headers });    
    return s;
  }
  activateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/activate/" + userID, '', { headers: this.headers }); 
    return s;
  }
  deactivateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/deactivate/" + userID, '', { headers: this.headers });    
    return s;
  }
  get headers():Headers {
    let header = new Headers();
    header.append('Authorization', "Bearer " + this.token);
    //header.append('Authorization',"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTM4NCJ9.eyJuYW1laWQiOiJjYTcwNjJkZC04ZDEyLTQ4ODItOWUwNy01N2QxNWFmOGQ0YzAiLCJ1bmlxdWVfbmFtZSI6ImpvZ3dheWlAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS9hY2Nlc3Njb250cm9sc2VydmljZS8yMDEwLzA3L2NsYWltcy9pZGVudGl0eXByb3ZpZGVyIjoiQVNQLk5FVCBJZGVudGl0eSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiYjBlM2I2NjUtNTBhZS00NDU1LTkzYWItNWI5NGQ0MjRhMWRiIiwicm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiaXNzIjoiTE9DQUwgQVVUSE9SSVRZIiwiYXVkIjoiOWYyYzBhYzlkMGRiMGE5ZDE4NDM4YzgyOTZmN2FhYzExYjMwZGJjMTc2OTY1YmJiMDlhMjIyZDcwNTViZDE2MCIsImV4cCI6MTQ5OTg1NDE4MSwibmJmIjoxNDk4NjQ0NTgxfQ.4COg0PkqIRM1biFb9md65BYVHbkq2mAa-LgvRNsiAek-YXK9fOz3vXtOoZATUj3-");

    return header;
  }

  getNotetypes(): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/claimnotes/notetypes", { headers: this.headers })    
    return s;
  }
  getPrescriptionNotetypes(): Observable<Response> {
    let s =  this.http.get(this.baseUrl + "/prescriptionnotes/notetypes", { headers: this.headers })    
    return s;
  }
  getPrescriptionNotes(id:Number): Observable<Response> {
    let s =  this.http.post(this.baseUrl + "/prescriptionnotes/getprescriptionnotes/?prescriptionId="+id,{}, { headers: this.headers })
    return s;
  }

  saveClaimNote(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/claimnotes/savenote?claimId=" + data.claimId + "&noteText=" + data.noteText + "&noteTypeId=" + data.noteTypeId, {}, { headers: this.headers })    
    return s;
  }
  savePrescriptionNote(data): Observable<Response> {
    let s= this.http.post(this.baseUrl + "/prescriptionnotes/savenote", data, { headers: this.headers })    
    return s;
  }
  saveEpisode(data): Observable<Response> {
    let s= this.http.post(this.baseUrl + "/episodes/saveepisode", data, { headers: this.headers })    
    return s;
  }

  handleResponseError(res:Response){
      if(res.status == 401) {     
        this.toast.info('The page the tried to reach discovered an invalid login for you. Please log in!');             
        this.router.navigate(['/login']);
        this.events.broadcast("logout", true);
      }
  }

}
