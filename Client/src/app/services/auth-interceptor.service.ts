import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventsService } from './events-service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    activeToastId: number;
    errorToastId: number;
    constructor(private router: Router, private events: EventsService, private toast: ToastrService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const user = localStorage.getItem('user');
        let token = '';
        if (user && req.url.indexOf('images.bridgeportclaims.com') === -1 && req.url.indexOf('invoices.bridgeportclaims.com') === -1) {
            try {
                const us = JSON.parse(user);
                token = us.access_token;
                const dupReq = req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) });
                /*if (dupReq && dupReq.url) {
                  console.log(`API called with ${dupReq.urlWithParams}`);
                }*/
                return next.handle(dupReq);
            } catch (error) {

            }
        }
        const headers = new HttpHeaders();
        return next.handle(req.clone({ headers: headers }))
    }

  }
