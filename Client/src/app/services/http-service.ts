// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { UUID } from 'angular2-uuid';
import { Router } from '@angular/router';
import { EventsService } from './events-service';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpService {
  baseUrl = '/api';
  activeToastId: number;
  errorToastId: number;
  token: String;
  customObject = {
    groupNameAutoSuggest: (name) => {
      return this.groupNameAutoSuggest(name);
    }
  };

  constructor(private router: Router, private http: HttpClient, private events: EventsService, private toast: ToastrService) {
  }

  get headers(): HttpHeaders {
    const header = new HttpHeaders();
    const user = localStorage.getItem('user');
    try {
      const us = JSON.parse(user);
      header.append('Authorization', 'Bearer ' + us.access_token);
    } catch (error) {

    }
    return header;
  }

  setAuth(auth: String) {
    this.token = auth;
  }

  login(data: any, headers: any): Observable<any> {
    return this.http.post('/oauth/token', data, { headers: headers });
  }

  referralTypes(data): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/get-referral-types', data);
  }

  setReferralType(data): Observable<any> {
    return this.http.post(this.baseUrl + `/admin/set-client-user-type/?userId=${data.userId}&referralTypeId=${data.referralTypeId}`, data);
  }

  insertReferral(data): Observable<any> {
    return this.http.post(this.baseUrl + '/client/insert-referral', data);
  }

  states(data): Observable<any> {
    return this.http.post(this.baseUrl + '/client/get-states', data);
  }

  logout(): Observable<any> {
    return this.http.get(this.baseUrl + '/users/logout');
  }

  clearCache(): Observable<any> {
    return this.http.post(this.baseUrl + '/cache/clear', {});
  }

  getFiles(): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  deleteFileById(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/fileupload/delete/?importFileId=' + id)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  importFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/ServerEvents/ImportPaymentFile/?fileName=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  importLakerFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/laker/process?', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updateProfile(data): Observable<any> {
    const s = this.http.patch(this.baseUrl + '/users', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  passwordreset(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/passwordreset', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  resetpassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/resetpassword', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  forgotpassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/forgotpassword', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  editClaim(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/claims/edit-claim', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  addHistory(id: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/history/addclaim?claimId=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getHistory(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/history/claims')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getPaymentClaim(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/claims-script-counts', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  postPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/post-payments', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  deletePayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/delete-posting/?sessionId=' + data.sessionId + '&prescriptionId=' + data.prescriptionId + '&id=' + data.id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  paymentPosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/payment-posting', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  paymentToSuspense(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/to-suspense/', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  finalizePosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/finalize-posting/?sessionId=' + data.sessionId, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  deletePrescriptionPayment(prescriptionPaymentId: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/delete/?prescriptionPaymentId=' + prescriptionPaymentId, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  updatePrescriptionPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/update', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getDetailedPaymentClaim(data: any, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/payment/claims-script-details?sort=RxDate&sortDirection=DESC&page=1&pageSize=30
    let params = new HttpParams();
    if (sort) {
      params = params.set('sort', sort.toString());
      params = params.set('sortDirection', sortDir.toUpperCase());
    }
    if (page >= 1) {
      params = params.set('page', page.toString());
    }
    params = params.set('pageSize', pageSize.toString());
    const s = this.http.post(this.baseUrl + '/payment/claims-script-details', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  changepassword(data): Observable<any> {
    const s = this.http.put(this.baseUrl + '/account/changepassword', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  changeUserNameAndPassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/change-user-name-and-password', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  changeusername(firstName, lastName, id, extension: string): Observable<any> {
    const s = this.http.post(this.baseUrl + '/users/updatename/' + id + '?firstName=' + firstName + '&lastName=' +
      lastName + '&extension=' + extension,
      '')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  // register user
  register(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/create', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getClaimsData(data: any) {
    const s = this.http.post(this.baseUrl + '/Claims/GetClaimsData', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  // get user using id
  userFromId(id: UUID): Observable<any> {
    const s = this.http.get(this.baseUrl + '/Account/UserInfo?t=' + (new Date().getTime()))
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );

    return s;
  }

  confirmEmail(id: any, code: any): Observable<any> {
    const s = this.http.get(this.baseUrl + '/Account/ConfirmEmail?userId=' + id + '&code=' + code, { responseType: 'text' })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  profile(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/Account/UserInfo?t=' + (new Date().getTime()))
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getPayours(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.get(this.baseUrl + '/payor/getpayors/?pageNumber=' + pageNumber + '&pageSize=' + pageSize)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getActiveUsers(): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptions/get-active-users/', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getUsersListPerActiveUser(userid: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/collection/get-collection-assignment-data/?userId=' + userid, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  assignUsertoPayors(userid: any, payorIds: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/collection/assign-users-to-payors/', {
      'userId': userid,
      'payorIds': payorIds
    })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }


  getPayorList(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/get-payors/?pageNumber=' + pageNumber + '&pageSize=' + pageSize, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }


  getPayorList_newapi(): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/get-payors-for-unpaid-scripts/', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getUsers(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.get(this.baseUrl + '/account/users/?pageNumber=' + pageNumber + '&pageSize=' + pageSize)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getRoles(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/roles/', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  assignUserRole(data: any) {
    const s = this.http.post(this.baseUrl + '/roles/ManageUsersInRole', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  smartAsignRole(data: any) {
    const s = this.http.post(this.baseUrl + '/users/assign/' + data.EnrolledUsers + '?roleName=' + data.role, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  activateUser(userID) {
    const s = this.http.post(this.baseUrl + '/users/activate/' + userID, '')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  deactivateUser(userID) {
    const s = this.http.post(this.baseUrl + '/users/deactivate/' + userID, '')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getNotetypes(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/claimnotes/notetypes')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getPrescriptionNotetypes(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/prescriptionnotes/notetypes')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getEpisodesNoteTypes(): Observable<any> {
    return this.http.get(this.baseUrl + '/episodes/getepisodetypes')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getEpisodesOwners(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getDiaryOwners(): Observable<any> {
    return this.http.get(this.baseUrl + '/diary/owners')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getPrescriptionNotes(id: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/getprescriptionnotes/?prescriptionId=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  saveClaimNote(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/claimnotes/savenote', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  deleteClaimNote(data): Observable<any> {
    const s = this.http.delete(this.baseUrl + '/claimnotes/delete?claimId=' + data.claimId)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  savePrescriptionNote(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/savenote', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  saveEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  sortEpisodes(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    const data = {
      claimId: claimId.toString(),
      sortColumn: sort.toString(),
      sortDirection: sortDir.toUpperCase(),
      page: page.toString(),
      pageSize: pageSize.toString()
    };
    const s = this.http.post(this.baseUrl + '/claims/sort-episodes', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  saveEpisodeNote(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save-note', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  addEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/saveepisode', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  handleResponseError(res: any) {
    if (res.status === 401) {
      let toast = this.toast.toasts.find(t => t.toastId == this.activeToastId)
      if (toast) {
        toast.message = 'An invalid login was detected. Please log in again.';
      } else {
        this.activeToastId = this.toast.info('An invalid login was detected. Please log in again.', null,
          { timeOut: 10000 }).toastId
      }
      this.router.navigate(['/login']);
      this.events.broadcast('logout', true);
    } else if (res.status === 406) {
      const err = res.error;
      this.toast.error(err.message);
    } else if (res.status === 500) {
      let toast = this.toast.toasts.find(t => t.toastId == this.errorToastId)
      if (toast) {
        toast.message = 'A server error was detected. Please contact your system administrator.';
      } else {
        this.errorToastId = this.toast.error('A server error was detected. Please contact your system administrator.', null,
          { timeOut: 10000 }).toastId;
      }
      this.events.broadcast('loading-error', true);
    } else if (res.status === 0 || res.status === 504) {
      let toast = this.toast.toasts.find(t => t.toastId == this.errorToastId)
      if (toast) {
        toast.message = 'Cannot reach the server. Please check your network connection.';
      } else {
        this.errorToastId = this.toast.error('Cannot reach the server. Please check your network connection.', null,
          { timeOut: 10000 }).toastId;
      }
      this.events.broadcast('loading-error', true);
    }
  }

  getPrescriptions(claimId: Number, sort: string = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/prescriptions/sort/?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
    let params = new HttpParams()
      .set('claimId', claimId.toString())
      .set('pageSize', pageSize.toString());
    if (sort) {
      params = params.set('sort', sort);
      params = params.set('sortDirection', sortDir.toUpperCase());
    }
    if (page >= 1) {
      params = params.set('page', page.toString());
    }

    const s = this.http.post(this.baseUrl + '/prescriptions/sort/', {}, { params: params })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getOutstandingPrescriptions(claimId: Number, sort: string = 'rxDate', sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {

    const data: any = {
      claimId: claimId,
      sort: sort,
      sortDirection: sortDir.toUpperCase(),
      page: page,
      pageSize: pageSize
    };

    const s = this.http.post(this.baseUrl + '/Claims/outstanding', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getPayments(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    // api/payment/payments-blade?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
    let params = new HttpParams().set('claimId', claimId.toString());
    if (sort) {
      params = params.set('sort', sort.toString());
      params = params.set('sortDirection', sortDir.toUpperCase());
    }

    params = params.set('secondSort', 'null');
    params = params.set('secondSortDirection', 'null');
    if (page >= 1) {
      params = params.set('page', page.toString());
    }
    params = params.set('pageSize', pageSize.toString());
    const options = { params: params };
    const s = this.http.post(this.baseUrl + '/payment/payments-blade/', {}, options)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  cancelPayment(sessionId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/cancel-posting/?sessionId=' + sessionId, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getIndexedChecks(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/get-indexed-checks', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getIndexedCheckDetail(documentId: any): Observable<any> {
    return this.http.post(this.baseUrl + `/payment/get-indexed-check-details/?documentId=${documentId}`, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  reIndexedCheck(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/re-index-check/?documentId=' + data.documentId + '&skipPayments=' + data.skipPayments + '&prescriptionPaymentId', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  /* cancelIndexedCheck(documentId: any): Observable<any> {
    return this.http.post(this.baseUrl + `/payment/cancel-posting/?sessionId=9a2012bd-9a7c-4c75-ad17-3b5f099e462b?documentId=${documentId}`,{})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }
  deleteIndexedCheck(documentId: any): Observable<any> {
    return this.http.post(this.baseUrl + `/payment/get-indexed-check-details/?documentId=${documentId}`,{})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  } */
  saveFlex2(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/claims/set-flex2/?claimId=' + data.claimId + '&claimFlex2Id=' + data.claimFlex2Id, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updatePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/set-status/?prescriptionId=' + data.prescriptionId + '&prescriptionStatusId=' + data.prescriptionStatusId, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  archivePrescription(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/archive-unpaid-script', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  diaryList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/get', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  episodeList(data: any): Observable<any> {
    // return this.http.get('assets/json/episodes.json')
    return this.http.post(this.baseUrl + '/episodes/get', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  markEpisodeAsSolved(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/resolve/?episodeId=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  acquireEpisode(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/acquire/?episodeId=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  associateEpisodeClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/associate-to-claim', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  assignEpisode(id: any, userId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/assign/?episodeId=' + id + '&userId=' + userId, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  userToAssignEpisode(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users-to-assign', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getEpisodeNotes(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/note-modal/?episodeId=' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  archiveEpisode(id: any): Observable<any> {
    const params = new HttpParams()
      .set('episodeId', id);

    return this.http.post(this.baseUrl + '/episodes/archive', {}, { params: params })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  uploadFile(form: FormData): Observable<any> {
    return this.http.post(this.baseUrl + '/fileupload/upload', form)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getUploadFiles(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  deleteUploadedFile(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  removeFromDiary(prescriptionNoteId: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/remove/?prescriptionNoteId=' + prescriptionNoteId, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updateDiaryFollowUpDate(prescriptionNoteId: any, data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/update-follow-up-date?prescriptionNoteId=' + prescriptionNoteId, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  unpaidScriptsList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/unpaid', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  revenueByMonth(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/kpi/revenue', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  accountReceivable(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/accounts-receivable', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  archiveDuplicateClaim(id: any): Observable<any> {
    const params = new HttpParams()
      .set('claimId', id);
    return this.http.post(this.baseUrl + '/reports/archive-duplicate-claim', {}, { params: params })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  duplicateClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/duplicate-claims', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  skippedPaymentList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/skippedpayment', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  shortPayList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/shortpay', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  collectionBonus(data?: any): Observable<any> {
    const params = new HttpParams()
      .set('month', data.month)
      .set('year', data.year);
    return this.http.post(this.baseUrl + '/reports/collections-bonus', {}, { params: params })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  removeShortPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-shortpay/?prescriptionId=${data.prescriptionId}`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  removeSkippedPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-skipped-payment/?prescriptionId=${data.prescriptionId}`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getComparisonClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/get-left-right-claims/?leftClaimId=${data.leftClaimId}&rightClaimId=${data.rightClaimId}`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getMergeClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/save-claim-merge`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getExport(data: any): Observable<HttpResponse<Blob>> {
    const headers = new HttpHeaders();
    headers.append('accept', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');

    return this.http.post(this.baseUrl + '/reports/excel-export', data, { observe: 'response',responseType: 'blob', headers: headers })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  groupNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/group-name/?groupName=' + name, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  pharmacyNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/pharmacy-name/?pharmacyName=' + name, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getDocuments(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getInvalidChecks(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get-invalid', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  archiveDocument(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/archive/' + id, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getSortedImages(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/image/get', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  saveDocumentIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/save', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  saveInvoiceIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-invoice/?documentId=' + data.documentId + '&invoiceNumber=' + data.invoiceNumber, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  saveCheckIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-check/?documentId=' + data.documentId + '&checkNumber=' + data.checkNumber, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  checkInvoiceNumber(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/inv-num-exists/?invoiceNumber=' + data.invoiceNumber, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  modifyDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/edit', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  deleteDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/delete/?documentId=' + data.documentId, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  searchDocumentClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/claim-search/?searchText=' + data.searchText, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updateDocumentIndex(data: any): Observable<any> {
    return this.http.put(this.baseUrl + '/image/edit', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  unindexImage(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/image/reindex/?documentId=' + id)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getKPIs(): Observable<any> {
    return this.http.post(this.baseUrl + '/dashboard/kpi', {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getFirewallSettings(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/firewall', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  createFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/create-firewall-setting', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  deleteFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/delete-firewall-setting/?ruleName=' + data.ruleName, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  invoiceAmounts(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/get-invoice-amounts', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updateBilledAmount(data: any): Observable<any> {
    const params = new HttpParams()
      .set('prescriptionId', data.prescriptionId)
      .set('billedAmount', data.billedAmount);
    return this.http.post(this.baseUrl + '/admin/update-billed-amount/', {}, { params: params })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  exportPrescriptions(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/scripts-pdf/?claimId=' + data.claimId, data, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  exportBillingStatement(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/billing-statement-excel/?claimId=' + data.claimId, data, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  updateMultiplePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/set-multiple-prescription-statuses', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  setMaxBalance(claimId: any, isMaxBalance: boolean): Observable<any> {
    return this.http.post(this.baseUrl + `/claims/set-max-balance/?claimId=${claimId}&isMaxBalance=${isMaxBalance}`, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  downloadDrLetter(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/letters/download-dr-letter', data, {
      observe: 'response',
      responseType: 'blob'
    })
    .pipe(
      catchError(err => {
        this.handleResponseError(err);
        return Observable.throw(err)
      })
    );
  }

  exportLetter(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/letters/download/?claimId=' + data.claimId + '&letterType='
      + data.type + '&prescriptionId=' + data.prescriptionId, data, {
        observe: 'response',
        responseType: 'blob'
      })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  getNotifications(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/get', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  saveLetterNotifications(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/save-payor-letter-name', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }

  multipageInvoices(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/multi-page-invoices', data, { observe: 'response', responseType: 'blob' })
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
  }


  getAdjustorName(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/get-adjustors', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }
  getTree(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/get-tree/?parentTreeId=${data.parentTreeId}`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }
  saveTreeNode(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/insert-node`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }
  deleteTreeNode(treeId: string): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/delete-node/?treeId=${treeId}`, {})
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }
  getTreeList(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/get-decision-tree-list`, data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getAttorneyName(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/get-attorneys', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  updateAdjustor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/update-adjustor', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  insertAdjustor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/insert-adjustor', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  insertAttorney(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/insert-attorney', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  updateAttorney(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/update-attorney', data)
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

  getStates(data: any): Observable<any> {
    const s = this.http.get(this.baseUrl + '/attorney/get-states')
      .pipe(
        catchError(err => {
          this.handleResponseError(err);
          return Observable.throw(err)
        })
      );
    return s;
  }

}
