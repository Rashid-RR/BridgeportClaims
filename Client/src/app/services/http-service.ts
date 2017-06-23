// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import "rxjs/add/operator/map";
 import {Response} from "@angular/http/src/static_response";
import {Http,RequestOptions,Headers} from "@angular/http";
import {UUID} from "angular2-uuid";

@Injectable()
export class HttpService {
  baseUrl: string = "/api";
 
  token:String
  constructor(private http:Http) {
     
  }
 setAuth(auth:String){
   this.token = auth;
 }
  login(data,headers): Observable<Response> {
      return this.http.post("/oauth/token", data,{headers:headers})
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
    return this.http.post(this.baseUrl + "/account/create", data)
  }
  getClaimsData(data:any){
    return this.http.post(this.baseUrl + "/Claims/GetClaimsData",data,{headers:this.headers});
  }
  //get user using id
  userFromId(id:UUID): Observable<Response> {  
     return this.http.get(this.baseUrl + "/Account/UserInfo",{headers:this.headers})
  }
  profile(): Observable<Response> {  
     return this.http.get(this.baseUrl + "/Account/UserInfo",{headers:this.headers})
  }
  getPayours(pageNumber:Number,pageSize:Number): Observable<Response> {  
     return this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber="+pageNumber+"&pageSize="+pageSize,{headers:this.headers})
  }
  getUsers(pageNumber:Number,pageSize:Number): Observable<Response> {  
     return this.http.get(this.baseUrl + "/account/users/?pageNumber="+pageNumber+"&pageSize="+pageSize,{headers:this.headers})
  }
  get headers(){
    let header = new Headers();
    header.append('Authorization',"Bearer "+this.token);
    return header;
  }
 

}
