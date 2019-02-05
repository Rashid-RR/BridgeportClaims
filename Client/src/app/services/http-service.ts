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
import {  tap } from 'rxjs/operators';

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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  deleteFileById(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/fileupload/delete/?importFileId=' + id)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  importFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/ServerEvents/ImportPaymentFile/?fileName=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  importLakerFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/laker/process?', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  updateProfile(data): Observable<any> {
    const s = this.http.patch(this.baseUrl + '/users', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  passwordreset(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/passwordreset', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  resetpassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/resetpassword', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  forgotpassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/forgotpassword', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  editClaim(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/claims/edit-claim', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  addHistory(id: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/history/addclaim?claimId=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getHistory(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/history/claims')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getPaymentClaim(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/claims-script-counts', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  postPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/post-payments', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  deletePayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/delete-posting/?sessionId=' + data.sessionId + '&prescriptionId=' + data.prescriptionId + '&id=' + data.id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  paymentPosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/payment-posting', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  paymentToSuspense(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/to-suspense/', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  finalizePosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/finalize-posting/?sessionId=' + data.sessionId, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  deletePrescriptionPayment(prescriptionPaymentId: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/delete/?prescriptionPaymentId=' + prescriptionPaymentId, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  updatePrescriptionPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/update', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  changepassword(data): Observable<any> {
    const s = this.http.put(this.baseUrl + '/account/changepassword', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  changeUserNameAndPassword(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/change-user-name-and-password', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  changeusername(firstName, lastName, id, extension: string): Observable<any> {
    const s = this.http.post(this.baseUrl + '/users/updatename/' + id + '?firstName=' + firstName + '&lastName=' +
      lastName + '&extension=' + extension,
      '')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  // register user
  register(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/create', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getClaimsData(data: any) {
    const s = this.http.post(this.baseUrl + '/Claims/GetClaimsData', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  // get user using id
  userFromId(id: UUID): Observable<any> {
    const s = this.http.get(this.baseUrl + '/Account/UserInfo?t=' + (new Date().getTime()))
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );

    return s;
  }

  confirmEmail(id: any, code: any): Observable<any> {
    return this.http.get(this.baseUrl + '/Account/ConfirmEmail?userId=' + id + '&code=' + code, { responseType: 'text' })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  profile(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/Account/UserInfo?t=' + (new Date().getTime()))
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getPayors(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.get(this.baseUrl + '/payor/getpayors/?pageNumber=' + pageNumber + '&pageSize=' + pageSize)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getActiveUsers(): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptions/get-active-users/', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getUsersListPerActiveUser(userid: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/collection/get-collection-assignment-data/?userId=' + userid, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }


  getPayorList(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/get-payors/?pageNumber=' + pageNumber + '&pageSize=' + pageSize, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }


  getPayorList_newapi(): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/get-payors-for-unpaid-scripts/', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getUsers(pageNumber: Number, pageSize: Number): Observable<any> {
    const s = this.http.get(this.baseUrl + '/account/users/?pageNumber=' + pageNumber + '&pageSize=' + pageSize)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getRoles(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/roles/', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  assignUserRole(data: any) {
    const s = this.http.post(this.baseUrl + '/roles/ManageUsersInRole', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  smartAsignRole(data: any) {
    const s = this.http.post(this.baseUrl + '/users/assign/' + data.EnrolledUsers + '?roleName=' + data.role, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  activateUser(userID) {
    const s = this.http.post(this.baseUrl + '/users/activate/' + userID, '')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  deactivateUser(userID) {
    const s = this.http.post(this.baseUrl + '/users/deactivate/' + userID, '')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getNotetypes(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/claimnotes/notetypes')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getPrescriptionNotetypes(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/prescriptionnotes/notetypes')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getEpisodesNoteTypes(): Observable<any> {
    return this.http.get(this.baseUrl + '/episodes/getepisodetypes')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getEpisodesOwners(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getDiaryOwners(): Observable<any> {
    return this.http.get(this.baseUrl + '/diary/owners')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getPrescriptionNotes(id: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/getprescriptionnotes/?prescriptionId=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  saveClaimNote(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/claimnotes/savenote', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  deleteClaimNote(data): Observable<any> {
    const s = this.http.delete(this.baseUrl + '/claimnotes/delete?claimId=' + data.claimId)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  savePrescriptionNote(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/savenote', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  saveEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  saveEpisodeNote(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save-note', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  addEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/saveepisode', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  handleResponseError(res: any) {
    if (res.status === 401) {
      const toast = this.toast.toasts.find(t => t.toastId === this.activeToastId);
      if (toast) {
        toast.message = 'An invalid login was detected. Please log in again.';
      } else {
        this.activeToastId = this.toast.info('An invalid login was detected. Please log in again.', null,
          { timeOut: 10000 }).toastId;
      }
      this.router.navigate(['/login']);
      this.events.broadcast('logout', true);
    } else if (res.status === 406) {
      const err = res.error;
      const toast = this.toast.toasts.find(t => t.toastId === this.errorToastId);
      if (toast) {
        toast.message = err.message;
      } else {
        this.errorToastId = this.toast.error(err.message, null, { timeOut: 100000 }).toastId;
      }
    } else if (res.status === 500) {
      const toast = this.toast.toasts.find(t => t.toastId === this.errorToastId);
      if (toast) {
        toast.message = 'A server error was detected. Please contact your system administrator.';
      } else {
        this.errorToastId = this.toast.error('A server error was detected. Please contact your system administrator.', null,
          { timeOut: 10000 }).toastId;
      }
      this.events.broadcast('loading-error', true);
    } else if (res.status === 0 || res.status === 504) {
      const toast = this.toast.toasts.find(t => t.toastId === this.errorToastId);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getPayments(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  cancelPayment(sessionId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/cancel-posting/?sessionId=' + sessionId, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getIndexedChecks(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/get-indexed-checks', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getIndexedCheckDetail(documentId: any): Observable<any> {
    return this.http.post(this.baseUrl + `/payment/get-indexed-check-details/?documentId=${documentId}`, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  reIndexedCheck(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/re-index-check/?documentId=' + data.documentId + '&skipPayments='
      + data.skipPayments + '&prescriptionPaymentId', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  saveFlex2(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/claims/set-flex2/?claimId=' + data.claimId + '&claimFlex2Id=' + data.claimFlex2Id, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  updatePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/set-status/?prescriptionId=' + data.prescriptionId
      + '&prescriptionStatusId=' + data.prescriptionStatusId, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  archivePrescription(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/archive-unpaid-script', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  diaryList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/get', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  episodeList(data: any): Observable<any> {
    // return this.http.get('assets/json/episodes.json')
    return this.http.post(this.baseUrl + '/episodes/get', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  markEpisodeAsSolved(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/resolve/?episodeId=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  acquireEpisode(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/acquire/?episodeId=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  associateEpisodeClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/associate-to-claim', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  assignEpisode(id: any, userId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/assign/?episodeId=' + id + '&userId=' + userId, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  userToAssignEpisode(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users-to-assign', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getEpisodeNotes(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/note-modal/?episodeId=' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  archiveEpisode(id: any): Observable<any> {
    const params = new HttpParams()
      .set('episodeId', id);

    return this.http.post(this.baseUrl + '/episodes/archive', {}, { params: params })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  uploadFile(form: FormData): Observable<any> {
    return this.http.post(this.baseUrl + '/fileupload/upload', form)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getUploadFiles(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  deleteUploadedFile(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  removeFromDiary(prescriptionNoteId: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/remove/?prescriptionNoteId=' + prescriptionNoteId, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  updateDiaryFollowUpDate(prescriptionNoteId: any, data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/update-follow-up-date?prescriptionNoteId=' + prescriptionNoteId, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  unpaidScriptsList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/unpaid', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  revenueByMonth(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/kpi/revenue', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  accountReceivable(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/accounts-receivable', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  archiveDuplicateClaim(id: any): Observable<any> {
    const params = new HttpParams()
      .set('claimId', id);
    return this.http.post(this.baseUrl + '/reports/archive-duplicate-claim', {}, { params: params })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  duplicateClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/duplicate-claims', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  skippedPaymentList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/skippedpayment', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  shortPayList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/shortpay', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  collectionBonus(data?: any): Observable<any> {
    const params = new HttpParams()
      .set('month', data.month)
      .set('year', data.year);
    return this.http.post(this.baseUrl + '/reports/collections-bonus', {}, { params: params })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  removeShortPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-shortpay/?prescriptionId=${data.prescriptionId}`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  removeSkippedPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-skipped-payment/?prescriptionId=${data.prescriptionId}`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getComparisonClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/get-left-right-claims/?leftClaimId=${data.leftClaimId}&rightClaimId=${data.rightClaimId}`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getMergeClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/save-claim-merge`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getExport(data: any): Observable<HttpResponse<Blob>> {
    const headers = new HttpHeaders();
    headers.append('accept', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');

    return this.http.post(this.baseUrl + '/reports/excel-export', data, { observe: 'response', responseType: 'blob', headers: headers })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  groupNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/group-name/?groupName=' + name, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  pharmacyNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/pharmacy-name/?pharmacyName=' + name, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getDocuments(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getInvalidChecks(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get-invalid', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  archiveDocument(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/archive/' + id, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getSortedImages(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/image/get', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  saveDocumentIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/save', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  saveInvoiceIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-invoice/?documentId=' + data.documentId + '&invoiceNumber=' + data.invoiceNumber, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  saveCheckIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-check/?documentId=' + data.documentId + '&checkNumber=' + data.checkNumber, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  checkInvoiceNumber(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/inv-num-exists/?invoiceNumber=' + data.invoiceNumber, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  modifyDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/edit', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  deleteDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/delete/?documentId=' + data.documentId, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  searchDocumentClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/claim-search/?searchText=' + data.searchText, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  updateDocumentIndex(data: any): Observable<any> {
    return this.http.put(this.baseUrl + '/image/edit', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  unindexImage(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/image/reindex/?documentId=' + id)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getKPIs(): Observable<any> {
    return this.http.post(this.baseUrl + '/dashboard/kpi', {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getFirewallSettings(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/firewall', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  createFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/create-firewall-setting', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  deleteFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/delete-firewall-setting/?ruleName=' + data.ruleName, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  invoiceAmounts(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/get-invoice-amounts', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getReferencesPayorsList(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/get-references-payors-list/', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  updateBilledAmount(data: any): Observable<any> {
    const params = new HttpParams()
      .set('prescriptionId', data.prescriptionId)
      .set('billedAmount', data.billedAmount);
    return this.http.post(this.baseUrl + '/admin/update-billed-amount/', {}, { params: params })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  exportPrescriptions(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/scripts-pdf/?claimId=' + data.claimId, data, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  exportBillingStatement(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/billing-statement-excel/?claimId=' + data.claimId, data, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  updateMultiplePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/set-multiple-prescription-statuses', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  setMaxBalance(claimId: any, isMaxBalance: boolean): Observable<any> {
    return this.http.post(this.baseUrl + `/claims/set-max-balance/?claimId=${claimId}&isMaxBalance=${isMaxBalance}`, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  downloadDrLetter(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/letters/download-dr-letter', data, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
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
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getNotifications(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/get', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  saveLetterNotifications(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/save-payor-letter-name', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  multipageInvoices(data: any): Observable<HttpResponse<Blob>> {
    return this.http.post(this.baseUrl + '/prescriptions/multi-page-invoices', data, { observe: 'response', responseType: 'blob' })
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
  }

  getReferencesAdjustorsList(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/get-adjustors', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  getTree(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/get-tree/?parentTreeId=${data.parentTreeId}`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  saveTreeNode(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/insert-node`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  selectTree(treeId: string,claimId: string): Observable<any> {
    const s = this.http.post(this.baseUrl + `/trees/select-tree?treeRootId=${treeId}&claimId=${claimId}`,{})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  /**
   * TO DO - update with the API call for node selection by the user
   * @param treeId
   * @param claimId
   */
  selectTreeNode(treeId: string,claimId: string): Observable<any> {
    const s = this.http.post(this.baseUrl + `/trees/select-tree?treeRootId=${treeId}&claimId=${claimId}`,{})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  deleteTreeNode(treeId: string): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/delete-node/?treeId=${treeId}`, {})
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }
  getTreeList(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + `/tree-config/get-decision-tree-list`, data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getReferencesAttorneysList(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/get-attorneys', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  updateAdjustor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/update-adjustor', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  insertAdjustor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/adjustors/insert-adjustor', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  insertAttorney(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/insert-attorney', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  updateAttorney(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/attorney/update-attorney', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  updatePayor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/references-payor-update', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  insertPayor(data: any): Observable<any> {
    const s = this.http.post(this.baseUrl + '/payors/references-payor-insert', data)
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

  getStates(data: any): Observable<any> {
    const s = this.http.get(this.baseUrl + '/attorney/get-states')
      .pipe(
        tap(_ => { }, error => {
          this.handleResponseError(error);
        })
      );
    return s;
  }

}
