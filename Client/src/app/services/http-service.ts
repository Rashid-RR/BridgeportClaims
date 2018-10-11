// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { UUID } from 'angular2-uuid';
import { Router } from '@angular/router';
import { EventsService } from './events-service';
import { ToastsManager, Toast } from 'ng2-toastr';

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
  constructor(private router: Router, private http: HttpClient, private events: EventsService, private toast: ToastsManager) {
  }

  setAuth(auth: String) {
    this.token = auth;
  }
  login(data, headers): Observable<any> {
    return this.http.post('/oauth/token', data, { headers: headers })
  }

  logout(): Observable<any> {
    return this.http.get(this.baseUrl + '/users/logout');
  }
  clearCache(): Observable<any> {
    return this.http.post(this.baseUrl + '/cache/clear', {});
  }
  getFiles(): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteFileById(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/fileupload/delete/?importFileId=' + id)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  importFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/ServerEvents/ImportPaymentFile/?fileName=' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  importLakerFile(id: String): Observable<any> {
    return this.http.post(this.baseUrl + '/laker/process?', {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  updateProfile(data): Observable<any> {
    const s = this.http.patch(this.baseUrl + '/users', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  passwordreset(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/passwordreset', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  resetpassword(data): Observable<any> {
    let s = this.http.post(this.baseUrl + "/account/resetpassword", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  forgotpassword(data): Observable<any> {
    let s = this.http.post(this.baseUrl + "/account/forgotpassword", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  editClaim(data: any): Observable<any> {
    let s = this.http.post(this.baseUrl + "/claims/edit-claim", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  addHistory(id: Number): Observable<any> {
    let s = this.http.post(this.baseUrl + "/history/addclaim?claimId=" + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  getHistory(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/history/claims')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  getPaymentClaim(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/claims-script-counts', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  postPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/post-payments', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  deletePayment(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/delete-posting/?sessionId=' + data.sessionId + '&prescriptionId=' + data.prescriptionId + '&id=' + data.id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  paymentPosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/post-payments', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  paymentToSuspense(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/to-suspense/', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  finalizePosting(data: any) {
    const s = this.http.post(this.baseUrl + '/payment/finalize-posting/?sessionId=' + data.sessionId, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  deletePrescriptionPayment(prescriptionPaymentId: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/delete/?prescriptionPaymentId=' + prescriptionPaymentId, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  updatePrescriptionPayment(data: any) {
    const s = this.http.post(this.baseUrl + '/prescription-payments/update', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
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
      params =params.set('page', page.toString());
    }
    params = params.set('pageSize', pageSize.toString());
    const s = this.http.post(this.baseUrl + '/payment/claims-script-details', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  changepassword(data): Observable<any> {
    const s = this.http.put(this.baseUrl + '/account/changepassword', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  changeusername(firstName, lastName, id, extension: string): Observable<any> {
    const s = this.http.post(this.baseUrl + '/users/updatename/' + id + '?firstName=' + firstName + '&lastName=' + lastName + "&extension=" + extension,
      '')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  // register user
  register(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/account/create', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getClaimsData(data: any) {
    const s = this.http.post(this.baseUrl + '/Claims/GetClaimsData', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  //get user using id
  userFromId(id: UUID): Observable<any> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo?t=" + (new Date().getTime()))
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });

    return s;
  }
  confirmEmail(id: any, code: any): Observable<any> {
    let s = this.http.get(this.baseUrl + "/Account/ConfirmEmail?userId=" + id + "&code=" + code)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  profile(): Observable<any> {
    let s = this.http.get(this.baseUrl + "/Account/UserInfo?t=" + (new Date().getTime()))
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPayours(pageNumber: Number, pageSize: Number): Observable<any> {
    let s = this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPayorList(pageNumber: Number, pageSize: Number): Observable<any> {
    let s = this.http.post(this.baseUrl + "/payors/get-payors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getUsers(pageNumber: Number, pageSize: Number): Observable<any> {
    let s = this.http.get(this.baseUrl + "/account/users/?pageNumber=" + pageNumber + "&pageSize=" + pageSize)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getRoles(data: any): Observable<any> {
    let s = this.http.post(this.baseUrl + "/roles/", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  assignUserRole(data: any) {
    let s = this.http.post(this.baseUrl + "/roles/ManageUsersInRole", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  smartAsignRole(data: any) {
    let s = this.http.post(this.baseUrl + "/users/assign/" + data.EnrolledUsers + "?roleName=" + data.role, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  activateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/activate/" + userID, '')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      })
    return s;
  }
  deactivateUser(userID) {
    let s = this.http.post(this.baseUrl + "/users/deactivate/" + userID, '')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  get headers(): HttpHeaders {
    const header = new HttpHeaders();
    var user = localStorage.getItem('user');
    try {
      let us = JSON.parse(user);
      header.append('Authorization', 'Bearer ' + us.access_token);
    } catch (error) {

    }
    return header;
  }

  getNotetypes(): Observable<any> {
    const s = this.http.get(this.baseUrl + '/claimnotes/notetypes')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPrescriptionNotetypes(): Observable<any> {
    let s = this.http.get(this.baseUrl + '/prescriptionnotes/notetypes')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getEpisodesNoteTypes(): Observable<any> {
    return this.http.get(this.baseUrl + '/episodes/getepisodetypes')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getEpisodesOwners(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users', {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getDiaryOwners(): Observable<any> {
    return this.http.get(this.baseUrl + '/diary/owners')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getPrescriptionNotes(id: Number): Observable<any> {
    const s = this.http.post(this.baseUrl + '/prescriptionnotes/getprescriptionnotes/?prescriptionId=' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }

  saveClaimNote(data): Observable<any> {
    const s = this.http.post(this.baseUrl + '/claimnotes/savenote', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  deleteClaimNote(data): Observable<any> {
    const s = this.http.delete(this.baseUrl + '/claimnotes/delete?claimId=' + data.claimId)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  savePrescriptionNote(data): Observable<any> {
    let s = this.http.post(this.baseUrl + '/prescriptionnotes/savenote', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  saveEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  sortEpisodes(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    let data = {
      claimId: claimId.toString(),
      sortColumn: sort.toString(),
      sortDirection: sortDir.toUpperCase(),
      page: page.toString(),
      pageSize: pageSize.toString()
    }
    const s = this.http.post(this.baseUrl + '/claims/sort-episodes', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  saveEpisodeNote(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/save-note', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  addEpisode(data): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/saveepisode', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  handleResponseError(res: any) {
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
      let err = res.error;
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

    const s = this.http.post(this.baseUrl + '/prescriptions/sort/',{}, { params: params })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  getPayments(claimId: Number, sort: String = null, sortDir: 'asc' | 'desc' = 'asc',
    page: Number = 1, pageSize: Number = 30) {
    //api/payment/payments-blade?claimId=776&sort=RxDate&sortDirection=DESC&page=1&pageSize=30
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
    let options = { params: params };
    const s = this.http.post(this.baseUrl + '/payment/payments-blade/',{}, options)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
    return s;
  }
  cancelPayment(sessionId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/payment/cancel-posting/?sessionId=' + sessionId, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveFlex2(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/claims/set-flex2/?claimId=' + data.claimId + '&claimFlex2Id=' + data.claimFlex2Id, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updatePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/set-status/?prescriptionId=' + data.prescriptionId + '&prescriptionStatusId=' + data.prescriptionStatusId, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  archivePrescription(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/archive-unpaid-script', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  diaryList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/get', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  episodeList(data: any): Observable<any> {
    //return this.http.get('assets/json/episodes.json')
    return this.http.post(this.baseUrl + '/episodes/get', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  markEpisodeAsSolved(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/resolve/?episodeId=' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  acquireEpisode(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/acquire/?episodeId=' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  associateEpisodeClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/associate-to-claim', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  assignEpisode(id: any, userId: UUID): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/assign/?episodeId=' + id + '&userId=' + userId, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  userToAssignEpisode(): Observable<any> {
    return this.http.post(this.baseUrl + '/users/get-users-to-assign', {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getEpisodeNotes(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/episodes/note-modal/?episodeId=' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  archiveEpisode(id: any): Observable<any> {
    let params = new HttpParams()
    .set('episodeId',id);
    
    return this.http.post(this.baseUrl + '/episodes/archive', {},{params:params})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  uploadFile(form: FormData): Observable<any> {
    return this.http.post(this.baseUrl + '/fileupload/upload', form)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getUploadFiles(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteUploadedFile(form: FormData): Observable<any> {
    return this.http.get(this.baseUrl + '/fileupload/getfiles')
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  removeFromDiary(prescriptionNoteId: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/remove/?prescriptionNoteId=' + prescriptionNoteId, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updateDiaryFollowUpDate(prescriptionNoteId: any, data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/diary/update-follow-up-date?prescriptionNoteId=' + prescriptionNoteId, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  unpaidScriptsList(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/unpaid', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  revenueByMonth(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/kpi/revenue', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  accountReceivable(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/accounts-receivable', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  archiveDuplicateClaim(id: any): Observable<any> {
    const params = new HttpParams()
    .set('claimId', id);
    return this.http.post(this.baseUrl + '/reports/archive-duplicate-claim', {},{params:params})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  duplicateClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/duplicate-claims', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  skippedPaymentList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/skippedpayment', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  shortPayList(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/shortpay', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  removeShortPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-shortpay/?prescriptionId=${data.prescriptionId}`, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  removeSkippedPay(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/reports/remove-skipped-payment/?prescriptionId=${data.prescriptionId}`, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getComparisonClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/get-left-right-claims/?leftClaimId=${data.leftClaimId}&rightClaimId=${data.rightClaimId}`, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getMergeClaims(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + `/kpi/save-claim-merge`, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getExport(data: any): Observable<any> {
    let headers = new HttpHeaders();
    headers.append('accept', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');

    return this.http.post(this.baseUrl + '/reports/excel-export', data, { responseType: "blob", headers: headers })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

  groupNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/group-name/?groupName=' + name, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  pharmacyNameAutoSuggest(name: any): Observable<any> {
    return this.http.post(this.baseUrl + '/reports/pharmacy-name/?pharmacyName=' + name, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getDocuments(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getInvalidChecks(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/get-invalid', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  archiveDocument(id: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/archive/' + id, {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getSortedImages(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/image/get', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveDocumentIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/save', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveInvoiceIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-invoice/?documentId=' + data.documentId + "&invoiceNumber=" + data.invoiceNumber, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveCheckIndex(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/index-check/?documentId=' + data.documentId + "&checkNumber=" + data.checkNumber, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  checkInvoiceNumber(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/index-document/inv-num-exists/?invoiceNumber=' + data.invoiceNumber, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  modifyDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/edit', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteDocument(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/delete/?documentId=' + data.documentId, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  searchDocumentClaim(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/document/claim-search/?searchText=' + data.searchText, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updateDocumentIndex(data: any): Observable<any> {
    return this.http.put(this.baseUrl + '/image/edit', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  unindexImage(id: any): Observable<any> {
    return this.http.delete(this.baseUrl + '/image/reindex/?documentId=' + id)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getKPIs(): Observable<any> {
    return this.http.post(this.baseUrl + '/dashboard/kpi', {})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getFirewallSettings(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/firewall', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  createFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/create-firewall-setting', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  deleteFirewallSetting(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/delete-firewall-setting/?ruleName=' + data.ruleName, data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  invoiceAmounts(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/admin/get-invoice-amounts', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updateBilledAmount(data: any): Observable<any> {
    const params = new HttpParams()
    .set('prescriptionId', data.prescriptionId)
    .set('billedAmount', data.billedAmount);
    return this.http.post(this.baseUrl + '/admin/update-billed-amount/', {}, {params: params})
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportPrescriptions(data: any): Observable<any> {
    return this.http.post(this.baseUrl + "/prescriptions/scripts-pdf/?claimId=" + data.claimId, data, { observe: 'response',responseType: 'blob' })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportBillingStatement(data: any): Observable<any> {
    return this.http.post(this.baseUrl + "/prescriptions/billing-statement-excel/?claimId=" + data.claimId, data, { observe: 'response',responseType: 'blob' })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  updateMultiplePrescriptionStatus(data: any): Observable<any> {
    return this.http.post(this.baseUrl + "/prescriptions/set-multiple-prescription-statuses", data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  exportLetter(data: any): Observable<any> {
    return this.http.post(this.baseUrl + "/letters/download/?claimId=" + data.claimId + "&letterType=" + data.type + "&prescriptionId=" + data.prescriptionId, data, { observe: 'response',responseType: 'blob' })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  getNotifications(data?: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/get', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  saveLetterNotifications(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/notifications/save-payor-letter-name', data)
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }
  multipageInvoices(data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/prescriptions/multi-page-invoices', data, { observe: 'response',responseType: 'blob' })
      .catch(err => {
        this.handleResponseError(err);
        return Observable.throw(err);
      });
  }

}
