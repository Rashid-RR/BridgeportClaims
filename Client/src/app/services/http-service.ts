// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Response } from '@angular/http/src/static_response';
import { Http, RequestOptions, ResponseContentType, Headers, URLSearchParams } from '@angular/http';
import { UUID } from 'angular2-uuid';
import { Router } from '@angular/router';
import { EventsService } from './events-service';
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class HttpService {
  baseUrl = '/api';
  activeToast: Toast;
  errorToast: Toast;
  token: String;
  customObject = {
    groupNameAutoSuggest: (name) => {
      return this.groupNameAutoSuggest(name)
    }
  };
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
    return this.http.post(this.baseUrl + '/cache/clear', {}, { headers: this.headers });
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
    return this.http.post(this.baseUrl + '/ServerEvents/ImportPaymentFile/?fileName=' + id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  importLakerFile(id: String): Observable<Response> {
    return this.http.post(this.baseUrl + '/laker/process?', {}, { headers: this.headers })
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  forgotpassword(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/account/forgotpassword", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  editClaim(data: any): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/claims/edit-claim", data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  addHistory(id: Number): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/history/addclaim?claimId=" + id, {}, { headers: this.headers })
      .catch(err => {
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  deletePayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/delete-posting/?sessionId=' + data.sessionId + '&prescriptionId=' + data.prescriptionId + '&id=' + data.id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  paymentPosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/payment-posting', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  paymentToSuspense(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/to-suspense/', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  finalizePosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/finalize-posting/?sessionId=' + data.sessionId, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  deletePrescriptionPayment(prescriptionPaymentId: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/delete/?prescriptionPaymentId=' + prescriptionPaymentId, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  updatePrescriptionPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/update', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getDetailedPaymentClaim(data: any, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/payment/claims-script-details?sort=RxDate&sortDirection=DESC&page=1&pageSize=30
    let params = new URLSearchParams();
    if (sort) {
      params.append('sort', sort.toString());
      params.append('sortDirection', sortDir.toUpperCase());
    }
    if (page >= 1) {
      params.append('page', page.toString());
    }
    params.append('pageSize', pageSize.toString());
    let options = new RequestOptions({ headers: this.headers });
    const s = this.http.post(this.baseUrl + '/payment/claims-script-details', data, options)
      .catch(err => {
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
  changeusername(firstName, lastName, id,extension:string): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/users/updatename/' + id + '?firstName=' + firstName + '&lastName=' + lastName+"&extension="+extension,
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getClaimsData(data: any) {
    const s = this.http.post(this.baseUrl + '/Claims/GetClaimsData', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  //get user using id
  userFromId(id: UUID): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });

    return s;
  }
  confirmEmail(id: any, code: any): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/ConfirmEmail?userId=" + id + "&code=" + code, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  profile(): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPayours(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getUsers(pageNumber: Number, pageSize: Number): Observable<Response> {
    let s = this.http.get(this.baseUrl + "/account/users/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getRoles(data: any): Observable<Response> {
    let s = this.http.post(this.baseUrl + "/roles/", data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  assignUserRole(data: any) {
    let s = this.http.post(this.baseUrl + "/roles/ManageUsersInRole", data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  smartAsignRole(data: any) {
    let s = this.http.post(this.baseUrl + "/users/assign/" + data.EnrolledUsers + "?roleName=" + data.role, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  activateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/activate/" + userID, '', { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      })
    return s;
  }
  deactivateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/deactivate/" + userID, '', { headers: this.headers })
      .catch(err => {
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPrescriptionNotetypes(): Observable<Response> {
    let s = this.http.get(this.baseUrl + '/prescriptionnotes/notetypes', { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getEpisodesNoteTypes(): Observable<Response> {
    return this.http.get(this.baseUrl + '/episodes/getepisodetypes', { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getEpisodesOwners(): Observable<Response> {
    return this.http.post(this.baseUrl + '/users/get-users',{}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getPrescriptionNotes(id: Number): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/getprescriptionnotes/?prescriptionId=' + id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  saveClaimNote(data): Observable<Response> {
    const s = this.http.post(this.baseUrl + '/claimnotes/savenote', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  deleteClaimNote(data): Observable<Response> {
    const s = this.http.delete(this.baseUrl + '/claimnotes/delete?claimId=' + data.claimId, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  savePrescriptionNote(data): Observable<Response> {
    let s = this.http.post(this.baseUrl + '/prescriptionnotes/savenote', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  saveEpisode(data): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/save', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  sortEpisodes(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/prescriptions/sort/?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
    let params = new URLSearchParams();
    params.append('claimId', claimId.toString());
    if (sort) {
      params.append('sortColumn', sort.toString());
      params.append('sortDirection', sortDir.toUpperCase());
    }
    if (page >= 1) {
      params.append('page', page.toString());
    }
    params.append('pageSize', pageSize.toString());
    let options = new RequestOptions({ body: params, headers: this.headers });
    const s = this.http.post(this.baseUrl + '/claims/sort-episodes', '', options)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  saveEpisodeNote(data): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/save-note', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  addEpisode(data): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/saveepisode', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  handleResponseError(res: Response) {
    if (res.status == 401) {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'An invalid login was detected. Please log in again.';
      } else {
        this.toast.info('An invalid login was detected. Please log in again.', null,
          { toastLife: 10000 }).then((toast: Toast) => {
            this.activeToast = toast;
          })
      }
      this.router.navigate(['/login']);
      this.events.broadcast('logout', true);
    } else if (res.status == 406) {
      let err = res.json();
      this.toast.error(err.message);
    } else if (res.status == 500) {
      if (this.errorToast && this.errorToast.timeoutId) {
        this.errorToast.message = 'A server error was detected. Please contact your system administrator.';
      } else {
        this.toast.error('A server error was detected. Please contact your system administrator.', null,
          { toastLife: 10000 }).then((toast: Toast) => {
            this.errorToast = toast;
          });
      }
      this.events.broadcast("loading-error", true);
    } else if (res.status == 0 || res.status == 504) {
      if (this.errorToast) {
        //this.toast.dismissToast(this.errorToast);
        this.errorToast.message = 'Cannot reach the server. Please check your network connection.';
      } else {
        this.toast.error('Cannot reach the server. Please check your network connection.', null,
          { toastLife: 10000 }).then((toast: Toast) => {
            this.errorToast = toast;
          });
      }
      this.events.broadcast("loading-error", true);
    }
  }
  getPrescriptions(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/prescriptions/sort/?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPayments(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    //api/payment/payments-blade?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
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
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  cancelPayment(sessionId: UUID): Observable<Response> {
    return this.http.post(this.baseUrl + '/payment/cancel-posting/?sessionId=' + sessionId, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveFlex2(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/claims/set-flex2/?claimId=' + data.claimId + '&claimFlex2Id=' + data.claimFlex2Id, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updatePrescriptionStatus(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/prescriptions/set-status/?prescriptionId=' + data.prescriptionId + '&prescriptionStatusId=' + data.prescriptionStatusId, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  diaryList(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/diary/get', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  episodeList(data: any): Observable<Response> {
    //return this.http.get('assets/json/episodes.json', { headers: this.headers })
    return this.http.post(this.baseUrl + '/episodes/get', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  markEpisodeAsSolved(id: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/resolve/?episodeId='+id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  acquireEpisode(id: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/acquire/?episodeId='+id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  assignEpisode(id: any,userId:UUID): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/assign/?episodeId='+id+'&userId='+userId, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  userToAssignEpisode(): Observable<Response> {
    return this.http.post(this.baseUrl + '/users/get-users-to-assign', {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getEpisodeNotes(id: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/episodes/note-modal/?episodeId='+id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  uploadFile(form: FormData): Observable<Response> {
    return this.http.post(this.baseUrl + '/fileupload/upload', form, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getUploadFiles(form: FormData): Observable<Response> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles', { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteUploadedFile(form: FormData): Observable<Response> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles', { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  removeFromDiary(prescriptionNoteId: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/diary/remove/?prescriptionNoteId=' + prescriptionNoteId, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  unpaidScriptsList(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/prescriptions/unpaid', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  revenueByMonth(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/kpi/revenue', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  accountReceivable(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/reports/accounts-receivable', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getExport(data: any): Observable<Response> {
    let headers = this.headers;
    headers.append('accept', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    return this.http.post(this.baseUrl + '/reports/excel-export', data, { responseType: ResponseContentType.Blob, headers: headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  groupNameAutoSuggest(name: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/reports/group-name/?groupName=' + name, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  pharmacyNameAutoSuggest(name: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/reports/pharmacy-name/?pharmacyName=' + name, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getDocuments(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/document/get', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  archiveDocument(id: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/document/archive/' + id, {}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getSortedImages(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/image/get', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveDocumentIndex(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/index-document/save', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveInvoiceIndex(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/index-document/index-invoice/?documentId='+data.documentId+"&invoiceNumber="+data.invoiceNumber, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  modifyDocument(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/document/edit', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteDocument(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/document/delete/?documentId=' + data.documentId, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  searchDocumentClaim(data: any): Observable<Response> {
    return this.http.post(this.baseUrl + '/document/claim-search/?searchText=' + data.searchText, data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updateDocumentIndex(data: any): Observable<Response> {
    return this.http.put(this.baseUrl + '/image/edit', data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  unindexImage(id: any): Observable<Response> {
    return this.http.delete(this.baseUrl + '/image/reindex/?documentId=' + id, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getKPIs(): Observable<Response> {
    return this.http.post(this.baseUrl + '/dashboard/kpi',{}, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getFirewallSettings(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/admin/firewall',data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  createFirewallSetting(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/admin/create-firewall-setting',data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteFirewallSetting(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/admin/delete-firewall-setting/?ruleName='+data.ruleName,data, { headers: this.headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportPrescriptions(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + "/prescriptions/scripts-pdf/?claimId="+data.claimId,data, {responseType:ResponseContentType.Blob,headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportBillingStatement(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + "/prescriptions/billing-statement-excel/?claimId="+data.claimId,data, {responseType:ResponseContentType.Blob,headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportLetter(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + "/letters/download/?claimId="+data.claimId+"&letterType="+data.type+"&prescriptionId="+data.prescriptionId,data, {responseType:ResponseContentType.Blob,headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getNotifications(data?:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/notifications/get',data, {headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveLetterNotifications(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/notifications/save-payor-letter-name',data, {headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  multipageInvoices(data:any): Observable<Response> {
    return this.http.post(this.baseUrl + '/prescriptions/multi-page-invoices',data, {responseType: ResponseContentType.Blob,headers:this.headers})    
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

}
