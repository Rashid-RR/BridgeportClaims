import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";



@Injectable()
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor() { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var user = localStorage.getItem('user');
        let token = '';
        if (user && req.url.indexOf('images.bridgeportclaims.com')==-1 && req.url.indexOf('invoices.bridgeportclaims.com')==-1) {
            try {
                let us = JSON.parse(user);
                token = us.access_token;
                const dupReq = req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) });
                return next.handle(dupReq);
            } catch (error) {
                console.log(error);
            }
        }
        let headers = new HttpHeaders();
        return next.handle(req.clone({ headers: headers }));
    }
}