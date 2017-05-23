// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import "rxjs/add/operator/map";
 import {Response} from "@angular/http/src/static_response";
import {Http} from "@angular/http";
import {UUID} from "angular2-uuid";

@Injectable()
export class HttpService {
  baseUrl: string = "/api";
 
  constructor(private http:Http) {
     
  }
 
  login(data): Observable<Response> {
      return this.http.post(this.baseUrl + "/users", data)
  }
 
  logout(): Observable<Response> {
    return this.http.get(this.baseUrl + "/users/logout");
  }
 
  updateProfile(data): Observable<Response> {
    return this.http.patch(this.baseUrl + "/users", data)
  }

  passwordreset(data): Observable<Response> {
    return this.http.post(this.baseUrl + "/passwordreset", data)
  }

  changepassword(data): Observable<Response> {
    return this.http.post(this.baseUrl + "/changepassword", data)
  }
 //register user
  register(data): Observable<Response> {
    return this.http.post(this.baseUrl + "/users/register", data)
  }
  //get user using id
  userFromId(id:UUID): Observable<Response> {
    return this.http.get(this.baseUrl + "/users/"+id.toString())
  }
 

}
