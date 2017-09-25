// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Response } from '@angular/http/src/static_response';
import { Http, RequestOptions, Headers, URLSearchParams } from '@angular/http';
import { UUID } from 'angular2-uuid';
import {Router} from '@angular/router';
import {EventsService} from './events-service';
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class HttpService {
  baseUrl = '/api';
  activeToast: Toast;
  token: String;
  constructor(private router: Router, private http: Http, private events: EventsService, private toast: ToastsManager) {
  }
  setAuth(auth: String) {
    this.token = auth;
  }
  login(data, headers): Observable<Response> {
    return this.http.post('/oauth/token', data, { headers: headers })
  }

  logout(): Observable<Response> {
    return this.http.get(this.baseUrl + '/users/logout');
  }
  clearCache(): Observable<Response> {
    return this.http.post(this.baseUrl + '/cache/clear',{}, { headers: this.headers });
  }
  getFiles(): Observable<Response> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles', { headers: this.headers })
    .catch(err => {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteFileById(id: any): Observable<Response> {
    return this.http.delete(this.baseUrl + '/fileupload/delete/?importFileId=' + id, { headers: this.headers })
    .catch(err => {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  importFile(id: String): Observable<Response> {
    return this.http.post(this.baseUrl + '/ServerEvents/ImportPaymentFile/?fileName='+id, {},{ headers: this.headers })
    .catch(err => {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  importLakerFile(id: String): Observable<Response> {
    return this.http.post(this.baseUrl + '/laker/process?', {},{ headers: this.headers })
    .catch(err => {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  updateProfile(data): Observable<Response> {
    const s = this.http.patch(this.baseUrl + '/users', data)
    .catch(err => {
     this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  passwordreset(data): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/passwordreset', data)
    .catch(err => {
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
  addHistory(id:Number): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/history/addclaim?claimId="+id, {}, { headers: this.headers })    
    .catch(err =>  { 
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  getHistory(): Observable<Response> {
    const s = this.http.get(this.baseUrl + '/history/claims', { headers: this.headers })
    .catch(err => {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  getPaymentClaim(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/claims-script-counts', data, { headers: this.headers })
    .catch(err => {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  postPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/post-payments', data, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  deletePrescriptionPayment(prescriptionPaymentId: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/delete/?prescriptionPaymentId='+prescriptionPaymentId, {}, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  updatePrescriptionPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/update', data, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  getDetailedPaymentClaim(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/claims-script-details', data, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  changepassword(data): Observable<Response> {
    const s = this.http.put(this.baseUrl + '/account/changepassword', data, { headers: this.headers })
    .catch(err => {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  changeusername(firstName, lastName, id): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/users/updatename/' + id + '?firstName=' + firstName + '&lastName=' + lastName,
     '', { headers: this.headers })
    .catch(err => {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  // register user
  register(data): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/account/create', data)
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  getClaimsData(data: any) {
    const s = this.http.post(this.baseUrl + '/Claims/GetClaimsData', data, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  //get user using id
  userFromId(id: UUID): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });

    return s;
  }
  confirmEmail(id: any, code: any): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/ConfirmEmail?userId="+id + "&code=" + code, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  profile(): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  getPayours(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  getUsers(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/account/users/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }
  getRoles(data: any): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/roles/", data, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  assignUserRole(data: any) {
    let s =  this.http.post(this.baseUrl + "/roles/ManageUsersInRole", data, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  activateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/activate/" + userID, '', { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      })
    return s;
  }
  deactivateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/deactivate/" + userID, '', { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  get headers(): Headers {
    const header = new Headers();
    var user = localStorage.getItem('user');
     try {
          let us = JSON.parse(user);
          header.append('Authorization', 'Bearer ' + us.access_token);
     } catch (error) {

      }
    return header;
  }

  getNotetypes(): Observable<Response> {
    const s = this.http.get(this.baseUrl + '/claimnotes/notetypes', { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPrescriptionNotetypes(): Observable<Response> {
    let s =  this.http.get(this.baseUrl + '/prescriptionnotes/notetypes', { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getEpisodesNoteTypes(): Observable<Response> {
    return this.http.get(this.baseUrl + '/episodes/getepisodetypes', {headers: this.headers})
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getPrescriptionNotes(id: Number): Observable<Response> {
    const s =  this.http.post(this.baseUrl + '/prescriptionnotes/getprescriptionnotes/?prescriptionId=' + id, {}, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  saveClaimNote(data): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/claimnotes/savenote?claimId=' + data.claimId + '&noteText=' + data.noteText + 
    '&noteTypeId=' + data.noteTypeId, {}, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  savePrescriptionNote(data): Observable<Response> {
    let s= this.http.post(this.baseUrl + '/prescriptionnotes/savenote', data, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  saveEpisode(data): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/saveepisode', data, { headers: this.headers })
    .catch(err =>  {
      this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  handleResponseError(res: Response) {
      if(res.status == 401) {
        if(this.activeToast && this.activeToast.timeoutId){
          this.toast.dismissToast(this.activeToast);
        }
        setTimeout(() => {
          this.toast.info('An invalid login was detected. Please log in again.', null,
           {toastLife: 10000}).then((toast: Toast) => {
              this.activeToast = toast;
          })
        , 1500; });
        this.router.navigate(['/login']);
        this.events.broadcast('logout', true);
        } else if (res.status == 406) {
          let err = res.json();
        this.toast.error(err.message);
        } else if (res.status == 500) {
          this.toast.error('A server error was detected. Please contact your system administrator.');
       }
  }
  getPrescriptions(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number= 1, pageSize: Number = 500) {
    // api/prescriptions/sort/?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=500
    let params = new URLSearchParams();
    params.append('claimId', claimId.toString());
    if (sort) {
      params.append('sort', sort.toString());
      params.append('sortDirection', sortDir.toUpperCase());
    }
    if (page >= 1) {
      params.append('page', page.toString());
    }
    params.append('pageSize', pageSize.toString());
    let options = new RequestOptions({ params: params, headers: this.headers });
    const s = this.http.post(this.baseUrl + '/prescriptions/sort/', '', options)
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

  getPayments(claimId: Number, sort: String = 'RxDate', sortDir: 'asc' | 'desc' = 'desc',
    page: Number= 1, pageSize: Number = 500) {
    //api/payment/payments-blade?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=500
    let params = new URLSearchParams();
    params.append('claimId', claimId.toString());
    if (sort) {
      params.append('sort', sort.toString());
      params.append('sortDirection', sortDir.toUpperCase());
    }
    params.append('secondSort', 'null');
    params.append('secondSortDirection', 'null');
    if (page >= 1) {
      params.append('page', page.toString());
    }
    params.append('pageSize', pageSize.toString());
    let options = new RequestOptions({ params: params, headers: this.headers });
    const s = this.http.post(this.baseUrl + '/payment/payments-blade/', '', options)
    .catch(err =>  {
    this.handleResponseError(err);
      return Observable.throw(err);
    });
    return s;
  }

}
