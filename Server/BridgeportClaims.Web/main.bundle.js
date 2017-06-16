webpackJsonp([1,5],{

/***/ 100:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(14);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PayorsComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var PayorsComponent = (function () {
    function PayorsComponent(http) {
        this.http = http;
        this.payors = [];
        this.pageSize = 5;
        this.loading = false;
        this.getPayors(1);
    }
    PayorsComponent.prototype.next = function () {
        this.getPayors(this.pageNumber + 1);
    };
    PayorsComponent.prototype.prev = function () {
        if (this.pageNumber > 1) {
            this.getPayors(this.pageNumber - 1);
        }
    };
    PayorsComponent.prototype.ngOnInit = function () {
    };
    PayorsComponent.prototype.getPayors = function (pageNumber) {
        var _this = this;
        this.loading = true;
        this.http.getPayours(pageNumber, this.pageSize).map(function (res) { _this.loading = false; return res.json(); }).subscribe(function (result) {
            _this.payors = result;
            _this.pageNumber = pageNumber;
        }, function (err) {
            console.log(err);
        });
    };
    return PayorsComponent;
}());
PayorsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-payors',
        template: __webpack_require__(311),
        styles: [__webpack_require__(275)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object])
], PayorsComponent);

var _a;
//# sourceMappingURL=payors.component.js.map

/***/ }),

/***/ 101:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PrivateComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var PrivateComponent = (function () {
    function PrivateComponent(http, events, profileManager) {
        this.http = http;
        this.events = events;
        this.profileManager = profileManager;
    }
    PrivateComponent.prototype.ngOnInit = function () {
        this.http.profile().map(function (res) { return res.json(); }).subscribe(function (res) {
            //console.log(res)
        }, function (err) { return console.log(err); });
    };
    return PrivateComponent;
}());
PrivateComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-private',
        template: __webpack_require__(312),
        styles: [__webpack_require__(276)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], PrivateComponent);

var _a, _b, _c;
//# sourceMappingURL=private.component.js.map

/***/ }),

/***/ 102:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return RegisterComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var RegisterComponent = (function () {
    function RegisterComponent(formBuilder, http, router) {
        this.formBuilder = formBuilder;
        this.http = http;
        this.router = router;
        this.submitted = false;
        this.registered = false;
        this.emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        this.form = this.formBuilder.group({
            firstname: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            lastname: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            Email: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].pattern(this.emailRegex)])],
            Password: ["", __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            ConfirmPassword: ["", __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])]
        });
    }
    RegisterComponent.prototype.ngOnInit = function () {
    };
    RegisterComponent.prototype.login = function () {
        this.router.navigate(['/login']);
    };
    RegisterComponent.prototype.register = function () {
        var _this = this;
        this.submitted = true;
        console.log(this.form.value);
        if (this.form.valid && this.form.get('Password').value !== this.form.get('ConfirmPassword').value) {
            this.form.get('ConfirmPassword').setErrors({ "unmatched": "Repeat password does not match password" });
        }
        if (this.form.valid) {
            try {
                this.http.register(this.form.value).subscribe(function (res) {
                    console.log("Successful registration");
                    _this.router.navigate(['/main/private']);
                    //console.log(res.json());
                    _this.registered = true;
                }, function (err) {
                    console.log(err);
                });
            }
            catch (e) {
            }
            finally {
            }
        }
    };
    return RegisterComponent;
}());
RegisterComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-register',
        template: __webpack_require__(313),
        styles: [__webpack_require__(277)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _c || Object])
], RegisterComponent);

var _a, _b, _c;
//# sourceMappingURL=register.component.js.map

/***/ }),

/***/ 103:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__http_service__ = __webpack_require__(14);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__http_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__profile_manager__ = __webpack_require__(27);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_1__profile_manager__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__claim_manager__ = __webpack_require__(51);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "e", function() { return __WEBPACK_IMPORTED_MODULE_2__claim_manager__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__events_service__ = __webpack_require__(18);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "c", function() { return __WEBPACK_IMPORTED_MODULE_3__events_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__auth_guard__ = __webpack_require__(199);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "d", function() { return __WEBPACK_IMPORTED_MODULE_4__auth_guard__["a"]; });





//# sourceMappingURL=services.barrel.js.map

/***/ }),

/***/ 14:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map__ = __webpack_require__(119);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_http__ = __webpack_require__(93);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HttpService; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
// http-service.ts
/**
 * This service will serve to facilitate communication between app views and the web services
 */



var HttpService = (function () {
    function HttpService(http) {
        this.http = http;
        this.baseUrl = "/api";
    }
    HttpService.prototype.setAuth = function (auth) {
        this.token = auth;
    };
    HttpService.prototype.login = function (data, headers) {
        return this.http.post("/Token", data, { headers: headers });
    };
    HttpService.prototype.logout = function () {
        return this.http.get(this.baseUrl + "/users/logout");
    };
    HttpService.prototype.updateProfile = function (data) {
        return this.http.patch(this.baseUrl + "/users", data);
    };
    HttpService.prototype.passwordreset = function (data) {
        return this.http.post(this.baseUrl + "/passwordreset", data);
    };
    HttpService.prototype.changepassword = function (data) {
        return this.http.post(this.baseUrl + "/changepassword", data);
    };
    //register user
    HttpService.prototype.register = function (data) {
        return this.http.post(this.baseUrl + "/account/register", data);
    };
    HttpService.prototype.getClaimsData = function (data) {
        return this.http.post(this.baseUrl + "/Claims/GetClaimsData", data, { headers: this.headers });
    };
    //get user using id
    HttpService.prototype.userFromId = function (id) {
        return this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers });
    };
    HttpService.prototype.profile = function () {
        return this.http.get(this.baseUrl + "/Account/UserInfo", { headers: this.headers });
    };
    HttpService.prototype.getPayours = function (pageNumber, pageSize) {
        return this.http.get(this.baseUrl + "/payor/getpayors/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers });
    };
    Object.defineProperty(HttpService.prototype, "headers", {
        get: function () {
            var header = new __WEBPACK_IMPORTED_MODULE_2__angular_http__["b" /* Headers */]();
            header.append('Authorization', "Bearer " + this.token);
            return header;
        },
        enumerable: true,
        configurable: true
    });
    return HttpService;
}());
HttpService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["f" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */]) === "function" && _a || Object])
], HttpService);

var _a;
//# sourceMappingURL=http-service.js.map

/***/ }),

/***/ 157:
/***/ (function(module, exports) {

function webpackEmptyContext(req) {
	throw new Error("Cannot find module '" + req + "'.");
}
webpackEmptyContext.keys = function() { return []; };
webpackEmptyContext.resolve = webpackEmptyContext;
module.exports = webpackEmptyContext;
webpackEmptyContext.id = 157;


/***/ }),

/***/ 158:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__(191);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__(193);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__(200);




if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["a" /* enableProdMode */])();
}
__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 18:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__ = __webpack_require__(315);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EventsService; });
// event-service.ts
/**
 * Class to facilitate event based communication within the app by availing subjects/topics and subscribers
 */
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var EventsService = (function () {
    function EventsService() {
        var _this = this;
        this.listeners = {};
        this.eventsSubject = new __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__["Subject"]();
        this.events = __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__["Observable"].from(this.eventsSubject);
        this.events.subscribe(function (_a) {
            var name = _a.name, args = _a.args;
            if (_this.listeners[name]) {
                for (var _i = 0, _b = _this.listeners[name]; _i < _b.length; _i++) {
                    var listener = _b[_i];
                    listener.apply(void 0, args);
                }
            }
        });
    }
    EventsService.prototype.on = function (name, listener) {
        if (!this.listeners[name]) {
            this.listeners[name] = [];
        }
        this.listeners[name].push(listener);
    };
    EventsService.prototype.broadcast = function (name) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        this.eventsSubject.next({
            name: name,
            args: args
        });
    };
    return EventsService;
}());
EventsService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["f" /* Injectable */])(),
    __metadata("design:paramtypes", [])
], EventsService);

//# sourceMappingURL=events-service.js.map

/***/ }),

/***/ 192:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_profile__ = __webpack_require__(67);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var AppComponent = (function () {
    function AppComponent(http, events, profileManager) {
        this.http = http;
        this.events = events;
        this.profileManager = profileManager;
    }
    AppComponent.prototype.ngOnDestroy = function () {
    };
    AppComponent.prototype.ngOnInit = function () {
        var _this = this;
        var user = localStorage.getItem("user");
        if (user !== null && user.length > 0) {
            try {
                var us = JSON.parse(user);
                //this.events.broadcast('profile', us);
                this.http.setAuth(us.access_token);
                var profile = new __WEBPACK_IMPORTED_MODULE_3__models_profile__["a" /* UserProfile */](us.id || us.userName, us.login || us.userName, us.displayName || us.userName, us.email || us.userName, us.userName, us.avatarUrl, us.createdOn);
                this.profileManager.setProfile(profile);
                this.profileManager.profile = profile;
                /*this.profileManager.userInfo(us.userName).single().subscribe( res => {
                this.profileManager.profile= res;
                this.events.broadcast('profile', res);
               },(error)=>{
                 
               });*/
            }
            catch (error) {
                console.log(error);
            }
        }
        this.events.on("logout", function (v) {
            _this.profileManager.clearUsers();
            _this.profileManager.profile = undefined;
        });
    };
    return AppComponent;
}());
AppComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-root',
        template: "<router-outlet></router-outlet>"
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], AppComponent);

var _a, _b, _c;
//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 193:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__(37);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_http__ = __webpack_require__(93);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_common__ = __webpack_require__(36);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_component__ = __webpack_require__(192);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__layouts_header_header_component__ = __webpack_require__(197);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__layouts_app_layout_component__ = __webpack_require__(94);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__layouts_sidebar_sidebar_component__ = __webpack_require__(198);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__pages_private_private_component__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__pages_login_login_component__ = __webpack_require__(97);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__pages_register_register_component__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__pages_main_main_component__ = __webpack_require__(98);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__pages_password_reset_password_reset_component__ = __webpack_require__(99);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__pages_error404_error404_component__ = __webpack_require__(96);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__app_routing__ = __webpack_require__(194);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__ = __webpack_require__(103);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__pages_payors_payors_component__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__pages_claim_claim_component__ = __webpack_require__(95);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__components_claim_search_claim_search_component__ = __webpack_require__(196);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__components_claim_result_claim_result_component__ = __webpack_require__(195);
/* unused harmony export SafeStylePipe */
/* unused harmony export SafeUrlPipe */
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






//Layouts 



//end of layouts







//services





var SafeStylePipe = (function () {
    function SafeStylePipe(sanitized) {
        this.sanitized = sanitized;
    }
    SafeStylePipe.prototype.transform = function (value) {
        return this.sanitized.bypassSecurityTrustStyle(value);
    };
    return SafeStylePipe;
}());
SafeStylePipe = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["b" /* Pipe */])({ name: 'safeStyle' }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */]) === "function" && _a || Object])
], SafeStylePipe);

var SafeUrlPipe = (function () {
    function SafeUrlPipe(sanitized) {
        this.sanitized = sanitized;
    }
    SafeUrlPipe.prototype.transform = function (value) {
        return this.sanitized.bypassSecurityTrustUrl(value);
    };
    return SafeUrlPipe;
}());
SafeUrlPipe = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["b" /* Pipe */])({ name: 'safeURL' }),
    __metadata("design:paramtypes", [typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */]) === "function" && _b || Object])
], SafeUrlPipe);

var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["c" /* NgModule */])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */],
            __WEBPACK_IMPORTED_MODULE_7__layouts_app_layout_component__["a" /* AppLayoutComponent */],
            __WEBPACK_IMPORTED_MODULE_14__pages_error404_error404_component__["a" /* Error404Component */],
            __WEBPACK_IMPORTED_MODULE_6__layouts_header_header_component__["a" /* HeaderComponent */],
            __WEBPACK_IMPORTED_MODULE_10__pages_login_login_component__["a" /* LoginComponent */],
            __WEBPACK_IMPORTED_MODULE_10__pages_login_login_component__["a" /* LoginComponent */],
            __WEBPACK_IMPORTED_MODULE_12__pages_main_main_component__["a" /* MainComponent */],
            __WEBPACK_IMPORTED_MODULE_13__pages_password_reset_password_reset_component__["a" /* PasswordResetComponent */],
            __WEBPACK_IMPORTED_MODULE_11__pages_register_register_component__["a" /* RegisterComponent */],
            SafeStylePipe, SafeUrlPipe, __WEBPACK_IMPORTED_MODULE_18__pages_claim_claim_component__["a" /* ClaimsComponent */],
            __WEBPACK_IMPORTED_MODULE_8__layouts_sidebar_sidebar_component__["a" /* SidebarComponent */], __WEBPACK_IMPORTED_MODULE_9__pages_private_private_component__["a" /* PrivateComponent */], __WEBPACK_IMPORTED_MODULE_17__pages_payors_payors_component__["a" /* PayorsComponent */], __WEBPACK_IMPORTED_MODULE_19__components_claim_search_claim_search_component__["a" /* ClaimSearchComponent */], __WEBPACK_IMPORTED_MODULE_20__components_claim_result_claim_result_component__["a" /* ClaimResultComponent */],
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["b" /* BrowserModule */],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["a" /* FormsModule */],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["b" /* ReactiveFormsModule */],
            __WEBPACK_IMPORTED_MODULE_3__angular_http__["a" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_15__app_routing__["a" /* RoutingModule */],
        ],
        providers: [
            __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__["a" /* HttpService */], __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__["b" /* ProfileManager */], __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__["c" /* EventsService */], __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__["d" /* AuthGuard */], __WEBPACK_IMPORTED_MODULE_16__services_services_barrel__["e" /* ClaimManager */],
            {
                provide: __WEBPACK_IMPORTED_MODULE_4__angular_common__["a" /* LocationStrategy */],
                useClass: __WEBPACK_IMPORTED_MODULE_4__angular_common__["b" /* HashLocationStrategy */]
            }
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]]
    })
], AppModule);

var _a, _b;
//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 194:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__layouts_app_layout_component__ = __webpack_require__(94);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__pages_login_login_component__ = __webpack_require__(97);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__pages_private_private_component__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__pages_register_register_component__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__pages_main_main_component__ = __webpack_require__(98);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__pages_password_reset_password_reset_component__ = __webpack_require__(99);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__pages_error404_error404_component__ = __webpack_require__(96);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__pages_payors_payors_component__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__pages_claim_claim_component__ = __webpack_require__(95);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__services_services_barrel__ = __webpack_require__(103);
/* unused harmony export routes */
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return RoutingModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



//end of layouts









var routes = [
    {
        path: '',
        redirectTo: 'main/private',
        pathMatch: 'full'
    },
    {
        path: '',
        component: __WEBPACK_IMPORTED_MODULE_2__layouts_app_layout_component__["a" /* AppLayoutComponent */],
        children: [
            {
                path: 'home',
                component: __WEBPACK_IMPORTED_MODULE_6__pages_main_main_component__["a" /* MainComponent */]
            },
            {
                path: 'login',
                component: __WEBPACK_IMPORTED_MODULE_3__pages_login_login_component__["a" /* LoginComponent */]
            }, {
                path: 'recover-lost-password',
                component: __WEBPACK_IMPORTED_MODULE_7__pages_password_reset_password_reset_component__["a" /* PasswordResetComponent */]
            }, {
                path: 'register',
                component: __WEBPACK_IMPORTED_MODULE_5__pages_register_register_component__["a" /* RegisterComponent */]
            },
            {
                path: 'main',
                //resolve:AuthGuard,
                canActivate: [__WEBPACK_IMPORTED_MODULE_11__services_services_barrel__["d" /* AuthGuard */]],
                children: [
                    {
                        path: 'private',
                        component: __WEBPACK_IMPORTED_MODULE_4__pages_private_private_component__["a" /* PrivateComponent */]
                    },
                    {
                        path: 'payors',
                        component: __WEBPACK_IMPORTED_MODULE_9__pages_payors_payors_component__["a" /* PayorsComponent */]
                    },
                    {
                        path: 'claims',
                        component: __WEBPACK_IMPORTED_MODULE_10__pages_claim_claim_component__["a" /* ClaimsComponent */]
                    }
                ]
            }
        ]
    },
    { path: '404', component: __WEBPACK_IMPORTED_MODULE_8__pages_error404_error404_component__["a" /* Error404Component */] },
    { path: '**', redirectTo: '/404' }
];
var RoutingModule = (function () {
    function RoutingModule() {
    }
    return RoutingModule;
}());
RoutingModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["c" /* NgModule */])({
        imports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* RouterModule */].forRoot(routes)],
        exports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* RouterModule */]]
    })
], RoutingModule);

//# sourceMappingURL=app.routing.js.map

/***/ }),

/***/ 195:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimResultComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ClaimResultComponent = (function () {
    function ClaimResultComponent(claimManager, formBuilder, http, router, events) {
        this.claimManager = claimManager;
        this.formBuilder = formBuilder;
        this.http = http;
        this.router = router;
        this.events = events;
    }
    ClaimResultComponent.prototype.ngOnInit = function () {
    };
    return ClaimResultComponent;
}());
ClaimResultComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-claim-result',
        template: __webpack_require__(301),
        styles: [__webpack_require__(265)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */]) === "function" && _e || Object])
], ClaimResultComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=claim-result.component.js.map

/***/ }),

/***/ 196:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimSearchComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ClaimSearchComponent = (function () {
    function ClaimSearchComponent(claimManager, formBuilder, http, router, events) {
        this.claimManager = claimManager;
        this.formBuilder = formBuilder;
        this.http = http;
        this.router = router;
        this.events = events;
        this.submitted = false;
        this.form = this.formBuilder.group({
            claimNumber: [null],
            firstName: [null],
            lastName: [null],
            rxNumber: [null],
            invoiceNumber: [null]
        });
    }
    ClaimSearchComponent.prototype.ngOnInit = function () {
    };
    ClaimSearchComponent.prototype.textChange = function (controlName) {
        if (this.form.get(controlName).value === 'undefined' || this.form.get(controlName).value === '') {
            this.form.get(controlName).setValue(null);
        }
    };
    ClaimSearchComponent.prototype.search = function () {
        this.claimManager.search(this.form.value);
    };
    return ClaimSearchComponent;
}());
ClaimSearchComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-claim-search',
        template: __webpack_require__(302),
        styles: [__webpack_require__(266)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */]) === "function" && _e || Object])
], ClaimSearchComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=claim-search.component.js.map

/***/ }),

/***/ 197:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HeaderComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var HeaderComponent = (function () {
    function HeaderComponent(http, router, eventservice, profileManager) {
        this.http = http;
        this.router = router;
        this.eventservice = eventservice;
        this.profileManager = profileManager;
    }
    HeaderComponent.prototype.ngOnInit = function () {
    };
    HeaderComponent.prototype.logout = function () {
        this.eventservice.broadcast("logout", true);
        this.profileManager.profile = undefined;
        localStorage.removeItem('user');
        this.router.navigate(['/login']);
        /* this.http.logout().subscribe(res=>{
             console.log(res);
         });*/
    };
    return HeaderComponent;
}());
HeaderComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-header',
        template: __webpack_require__(304),
        styles: [__webpack_require__(268)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _d || Object])
], HeaderComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=header.component.js.map

/***/ }),

/***/ 198:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SidebarComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var SidebarComponent = (function () {
    function SidebarComponent(http, events, router, profileManager) {
        this.http = http;
        this.events = events;
        this.router = router;
        this.profileManager = profileManager;
    }
    SidebarComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(SidebarComponent.prototype, "userName", {
        get: function () {
            return this.profileManager.profile ? this.profileManager.profile.displayName : '';
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SidebarComponent.prototype, "avatar", {
        get: function () {
            return this.profileManager.profile ? this.profileManager.profile.avatarUrl : '';
        },
        enumerable: true,
        configurable: true
    });
    return SidebarComponent;
}());
SidebarComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-sidebar',
        template: __webpack_require__(305),
        styles: [__webpack_require__(269)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _d || Object])
], SidebarComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=sidebar.component.js.map

/***/ }),

/***/ 199:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_add_operator_first__ = __webpack_require__(118);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_add_operator_first___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_rxjs_add_operator_first__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthGuard; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
/*
 This class serves to guard routes and modules from unauthorized access
 CanActivate method of this class is used in the app routing module to determine if user has access before letting in
 */





 // in imports
var AuthGuard = (function () {
    function AuthGuard(events, router, profileManager) {
        var _this = this;
        this.events = events;
        this.router = router;
        this.profileManager = profileManager;
        this.events.on("logout", function (immediately) {
            _this.profileManager.profile = undefined;
            localStorage.removeItem("user");
            _this.router.navigate(['/login']);
        });
    }
    AuthGuard.prototype.canActivate = function () {
        var _this = this;
        return this.isLoggedIn.map(function (e) {
            if (e) {
                //console.log("User is logged in");
                return true;
            }
            else {
                //console.log(e,"User is not logged in");
                _this.router.navigate(['/login']);
            }
        }).catch(function (e) {
            //console.log(e);
            _this.router.navigate(['/login']);
            return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["Observable"].of(false);
        });
    };
    Object.defineProperty(AuthGuard.prototype, "isLoggedIn", {
        get: function () {
            var user = localStorage.getItem("user");
            if (user === null || user.length == 0) {
                return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["Observable"].of(false);
            }
            try {
                var us = JSON.parse(user);
                //console.log(this.profileManager.userProfile(us.userName));
                return this.profileManager.userInfo(us.userName).single().map(function (res) { return res.userName ? true : false; });
            }
            catch (error) {
                return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["Observable"].of(false);
            }
        },
        enumerable: true,
        configurable: true
    });
    AuthGuard.prototype.resolve = function (route, state) {
        var user = this.profileManager.User;
        user.subscribe(function (profile) {
            //console.log(profile);
        }, function (error) {
            console.log(error);
        });
        //return user.map(profile=>{console.log(profile);return profile});
        return this.profileManager.User;
    };
    AuthGuard.prototype.hasRights = function (module) {
    };
    return AuthGuard;
}());
AuthGuard = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["f" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], AuthGuard);

var _a, _b, _c;
//# sourceMappingURL=auth.guard.js.map

/***/ }),

/***/ 200:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.
// The file contents for the current environment will overwrite these during build.
var environment = {
    production: false
};
//# sourceMappingURL=environment.js.map

/***/ }),

/***/ 265:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 266:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 267:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, " ", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 268:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 269:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 27:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable__ = __webpack_require__(282);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_immutable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_profile__ = __webpack_require__(67);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ProfileManager; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ProfileManager = (function () {
    function ProfileManager(http, events) {
        var _this = this;
        this.http = http;
        this.events = events;
        this.userCache = __WEBPACK_IMPORTED_MODULE_0_immutable__["OrderedMap"]();
        this.profile = null;
        this.events.on("profile", function (profile) {
            _this.profile = profile;
        });
        this.events.on("logout", function (v) {
            _this.clearUsers();
        });
    }
    ProfileManager.prototype.userInfo = function (userId) {
        var _this = this;
        var v = this.userCache.get(userId);
        if (v) {
            return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].of(v);
        }
        else {
            var s = this.http.userFromId(userId);
            s.subscribe(function (res) {
                var u = res.json();
                _this.userCache = _this.userCache.set(u.userName, u);
            });
            return s.map(function (res) { return res.json(); });
        }
    };
    ProfileManager.prototype.setProfile = function (u) {
        var profile = new __WEBPACK_IMPORTED_MODULE_2__models_profile__["a" /* UserProfile */](u.id || u.userName, u.login || u.userName, u.displayName || u.userName, u.email || u.userName, u.userName, u.avatarUrl, u.createdOn);
        this.userCache = this.userCache.set(profile.userName, profile);
    };
    ProfileManager.prototype.userProfile = function (userId) {
        return this.userCache.get(userId);
    };
    ProfileManager.prototype.clearUsers = function () {
        this.userCache = __WEBPACK_IMPORTED_MODULE_0_immutable__["OrderedMap"]();
    };
    Object.defineProperty(ProfileManager.prototype, "User", {
        get: function () {
            var _this = this;
            var user = localStorage.getItem("user");
            return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].create(function (observer) {
                if (user !== null && user.length > 0) {
                    try {
                        var us = JSON.parse(user);
                        //this.eventservice.broadcast('profile', us);
                        _this.userInfo(us.id).single().subscribe(function (res) {
                            _this.profile = res;
                            _this.events.broadcast('profile', res);
                            observer.next(res);
                        }, function (error) {
                            observer.error();
                        });
                    }
                    catch (error) {
                        console.log(error);
                    }
                }
                else {
                    observer.error();
                }
            });
        },
        enumerable: true,
        configurable: true
    });
    return ProfileManager;
}());
ProfileManager = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__angular_core__["f" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */]) === "function" && _b || Object])
], ProfileManager);

var _a, _b;
//# sourceMappingURL=profile-manager.js.map

/***/ }),

/***/ 270:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 271:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 272:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 273:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 274:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 275:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 276:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 277:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 301:
/***/ (function(module, exports) {

module.exports = "<ng-container   *ngIf=\"claimManager.claims && claimManager.claims.length==1\">\r\n    <div class=\"row invoice-info\" *ngFor=\"let claim of claimManager.claims\">\r\n      <div class=\"col-sm-6 invoice-col\">\r\n        <address> \r\n          Name: {{claim.name}}<br>\r\n          DOB: {{claim.dateOfBirth}}<br>\r\n          Gender: {{claim.gender}}<br>\r\n          Carrier: {{claim.carrier}}<br/>\r\n          Adjustor : {{claim.adjustor}}<br>\r\n          Adjustor Ph : {{claim.adjustorPhoneNumber}}<br><br>\r\n          Eligibility Entered: {{claim.dateEntered}}<br><br>\r\n        </address>\r\n      </div>\r\n      <!-- /.col -->\r\n      <div class=\"col-sm-6 invoice-col\">\r\n        <address> \r\n          Claim #: {{claim.claimNumber}}<br>\r\n          Injury Date: {{claim.injuryDate}}<br>\r\n          Adjustor Fax : {{claim.adjustorFaxNumber}}<br>\r\n        </address>\r\n        <span class=\"label label-warning\" style=\"cursor:pointer;\"> View </span>\r\n      </div>\r\n  </div>\r\n</ng-container>\r\n<ng-container *ngIf=\"claimManager.claims && claimManager.claims.length>1\">\r\n  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <thead>\r\n              <tr>\r\n                <th>Claim #</th>\r\n                <th>Name</th>\r\n                <th>Carrier</th>\r\n                <th>Injury Date</th>\r\n                <th>&nbsp;</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody>\r\n              <tr *ngFor=\"let claim of claimManager.claims\">\r\n                <td>{{claim.claimNumber}}</td>\r\n                <td>{{claim.name}}</td>\r\n                <td>{{claim.carrier}}</td>\r\n                <td>{{claim.injuryDate}}</td>\r\n                <td><span class=\"label label-warning\" style=\"cursor:pointer;\"> View </span></td>\r\n              </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>\r\n</ng-container>\r\n"

/***/ }),

/***/ 302:
/***/ (function(module, exports) {

module.exports = "<form role=\"form\"  [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"search()\">\r\n    <div class=\"row\">\r\n        <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Claim #</label>\r\n              <input class=\"form-control\" name=\"claimNumber\" class=\"form-control\" formControlName=\"claimNumber\" (change)=\"textChange('claimNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>FirstName</label>\r\n              <input class=\"form-control\"  name=\"firstName\" class=\"form-control\" formControlName=\"firstName\"  (change)=\"textChange('firstName')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n       <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Last Name</label>\r\n              <input class=\"form-control\"  name=\"lastName\" class=\"form-control\" formControlName=\"lastName\"  (change)=\"textChange('lastName')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Rx Number</label>\r\n              <input class=\"form-control\"  name=\"rxNumber\" class=\"form-control\" formControlName=\"rxNumber\"  (change)=\"textChange('rxNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Invoice #</label>\r\n              <input class=\"form-control\"  name=\"invoiceNumber\" class=\"form-control\" formControlName=\"invoiceNumber\"  (change)=\"textChange('invoiceNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n\r\n      <div class=\"col-md-2\">\r\n           <label>&nbsp;</label>\r\n          <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"search()\">Search</button>\r\n      </div>\r\n    </div>\r\n</form>\r\n"

/***/ }),

/***/ 303:
/***/ (function(module, exports) {

module.exports = "<div class=\"wrapper\" style=\"height: auto;\">\r\n    <!--top header -->\r\n    <app-header></app-header>\r\n    <app-sidebar *ngIf=\"isLoggedIn\"></app-sidebar>\r\n    <div class=\"content-wrapper\">\r\n        <router-outlet></router-outlet>\r\n    </div>\r\n    <!-- /.content-wrapper -->\r\n</div>"

/***/ }),

/***/ 304:
/***/ (function(module, exports) {

module.exports = "<header class=\"main-header\">\r\n    <!-- Logo -->\r\n    <a [routerLink]=\"'/'\" class=\"logo\">\r\n      <!-- mini logo for sidebar mini 50x50 pixels -->\r\n      <span class=\"logo-mini\"><b>BR</b>-C</span>\r\n      <!-- logo for regular state and mobile devices -->\r\n      <span class=\"logo-lg\">Bridgeport Claims</span>\r\n    </a>\r\n    <nav class=\"navbar navbar-static-top\">\r\n            <!-- Sidebar toggle button, check if user is logged in-->\r\n            <a href=\"#\" class=\"sidebar-toggle\" data-toggle=\"offcanvas\" role=\"button\" *ngIf=\"profileManager.profile\">\r\n                <span class=\"sr-only\">Toggle navigation</span>\r\n                <span class=\"icon-bar\"></span>\r\n                <span class=\"icon-bar\"></span>\r\n                <span class=\"icon-bar\"></span>\r\n            </a>\r\n            <!-- Top right menu items, also check if user is logged in-->\r\n            <div class=\"navbar-custom-menu\">\r\n                <ul class=\"nav navbar-nav\" *ngIf=\"!profileManager.profile\">                        \r\n                    <!--<li><a [routerLink]=\"'/register'\">Register</a></li>-->\r\n                    <li><a [routerLink]=\"'/login'\">Login</a></li>\r\n                </ul>\r\n                <ul class=\"nav navbar-nav\" *ngIf=\"profileManager.profile\">                        \r\n                    <li routerLinkActive=\"active\" *ngIf=\"profileManager.profile\">\r\n                        <!--[routerLink]=\"'/profile'\"--> \r\n                        <a   class=\"navbar-link\">Logged in as {{profileManager.profile? profileManager.profile.email : ''}}</a>\r\n                    </li>\r\n                    <li routerLinkActive=\"active\" *ngIf=\"profileManager.profile\">\r\n                        <a  style=\"cursor:pointer;\" (click)=\"logout()\" class=\"navbar-link\">Logout</a>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n\r\n    </nav>\r\n    \r\n</header>"

/***/ }),

/***/ 305:
/***/ (function(module, exports) {

module.exports = "<aside class=\"main-sidebar\">\r\n    <!-- sidebar: style can be found in sidebar.less -->\r\n    <section class=\"sidebar\">\r\n      <!-- Sidebar user panel -->\r\n      <div class=\"user-panel\">\r\n        <div class=\"pull-left image\">\r\n          <img [src]=\"'assets/logo/Color Logo.jpg'\" class=\"img-square\" [alt]=\"userName\">\r\n          <br style=\"line-height:2em\" *ngIf=\"!avatar\">\r\n        </div>\r\n        <div class=\"pull-left info\">\r\n          <p>{{userName}}</p>\r\n        </div>\r\n      </div>\r\n      <!-- sidebar menu: : style can be found in sidebar.less -->\r\n      <ul class=\"sidebar-menu\">\r\n        <li>\r\n          <a [routerLink]=\"'/main/private'\">\r\n            <i class=\"fa fa-dashboard\"></i> <span>Dashboard</span>\r\n          </a>\r\n        </li>\r\n        <li>\r\n            <a  [routerLink]=\"'/main/payors'\">\r\n              <i class=\"fa fa-user fa-fw\"></i> \r\n              <span>Payors</span>\r\n            </a>\r\n        </li>       \r\n        <li>\r\n            <a  [routerLink]=\"'/main/claims'\">\r\n              <i class=\"fa fa-credit-card fa-fw\"></i> \r\n              <span>Claims</span>\r\n            </a>\r\n        </li>       \r\n      </ul>\r\n    </section>\r\n    <!-- /.sidebar -->\r\n  </aside>"

/***/ }),

/***/ 306:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">Bridgeport Claims</h3></div>\r\n            <div class=\"box-body\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-sm-12\"   id=\"accordion\">\r\n                            <app-claim-search></app-claim-search>\r\n                    </div>\r\n                </div>\r\n                <div class=\"row\">\r\n                    <div class=\"col-sm-6\">\r\n                        <div class=\"box box-info\" *ngIf=\"claimManager.claims\">\r\n                            <h5 class=\"box-title text-center\">Claims</h5>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-result></app-claim-result>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-sm-6\">\r\n                        \r\n                    </div>\r\n                </div>\r\n            </div>\r\n             <div class=\"overlay\" *ngIf=\"loading\">\r\n                <i class=\"fa fa-refresh fa-spin\"></i>\r\n            </div> \r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 307:
/***/ (function(module, exports) {

module.exports = "<div class=\"container\">\r\n    <div class=\"row\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-8 col-md-offset-2\">\r\n                 <h3>We can't seem to find the page you're looking for</h3>\r\n                <div class=\"row\">\r\n                    <div class=\"span5\">\r\n                        Please choose one of the locations below:\r\n                    </div>\r\n                </div>\r\n                <div class=\"row\"><br/></div>\r\n                <div class=\"row\">\r\n                    <div class=\"col-md-3\">\r\n                         <a [routerLink]=\"'/main/private'\" class=\"btn btn-primary btn-md btn-block\">Home</a>\r\n                    </div>\r\n                    <div class=\"col-md-3 col-md-offset-1\">\r\n                         <a (click)=\"backClicked()\" class=\"btn btn-default btn-md btn-block\">Go back</a>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>"

/***/ }),

/***/ 308:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3 \">\r\n        <div class=\"login-logo\">\r\n            <img [src]=\"'assets/logo/Color All.png'\" style=\"width:250px; padding-top: 50px;\" class=\"img-square\">\r\n        </div> \r\n    </div>\r\n</div>\r\n<div class=\"row\" style=\"padding-right:400px; padding-left:400px;\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3 \">\r\n        <form role=\"form\" [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"login()\">\r\n            <h3>Please sign in</h3>\r\n            <div class=\"form-group\">\r\n                <input type=\"text\" name=\"email\" class=\"form-control\" placeholder=\"Email address\" formControlName=\"email\" (focus)=\"submitted=false\" required focus-on>\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').value!='' && form.get('email').errors && submitted\">Incorrect email</p>\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').value =='' && submitted\">Email is required</p>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <input type=\"password\" name=\"password\" class=\"form-control bottom\" placeholder=\"Password\" formControlName=\"password\" required (focus)=\"submitted=false\">\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('password').errors && submitted\"> {{this.form.get('password').getError('required') ? 'Password is required': 'Incorrect email or password'}}</p>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <span class=\"help-block\"><a [routerLink]=\"'/recover-lost-password'\">Forgotten password?</a></span>\r\n            </div>\r\n            <div class=\"checkbox\">\r\n                <label><input type=\"checkbox\" value=\"true\" formControlName=\"rememberMe\"> Remember me</label>\r\n            </div>\r\n            <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"login()\">Sign in</button>\r\n        </form>\r\n    </div>\r\n</div>"

/***/ }),

/***/ 309:
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ 310:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-7 col-md-offset-3\">\r\n        <div class=\"box\">\r\n            <div class=\"box-body\"> \r\n                <form role=\"form\" [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\">\r\n                    <h4>Enter  your email to recover lost password</h4>\r\n                    <div class=\"form-group\">\r\n                        <input type=\"text\" formControlName=\"email\" class=\"form-control\" placeholder=\"Email address or login\"\r\n                            required>\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').errors && form.get('email').value\">Invalid Email address</p>\r\n                    </div>\r\n                    <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"submit()\">Reset password</button>\r\n                </form>\r\n            </div>\r\n            <div class=\"overlay\" *ngIf=\"submitted\">\r\n                <i class=\"fa fa-refresh fa-spin\"></i>\r\n            </div>    \r\n        </div>    \r\n    </div>\r\n</div>\r\n"

/***/ }),

/***/ 311:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">Payors</h3></div>\r\n            <div class=\"box-body row\">\r\n                <div class=\"col-lg-12\"   id=\"accordion\">\r\n                        <div class=\"panel panel-default\">\r\n                            <div class=\"panel-heading\">\r\n                                <h4 class=\"panel-title\">\r\n                                    <a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\">Search and Filter</a>\r\n                                </h4>\r\n                            </div>\r\n                            <div id=\"collapseOne\" class=\"panel-collapse collapse out\">\r\n                                <div class=\"panel-body\">\r\n                                    Will add search and filter UI\r\n                                </div>\r\n                            </div> \r\n                        </div> \r\n                </div>\r\n                <div class=\"col-lg-11 col-lg-offset-1\">\r\n                    <table width=\"100%\" class=\"table table-striped table-bordered table-hover\" id=\"dataTables-example\">\r\n                        <thead>\r\n                            <tr>\r\n                                <th>ID</th>\r\n                                <th>Billing Details</th>\r\n                                <th width=\"20%\">Notes</th>\r\n                                <th>Created On</th>\r\n                                <th>Updated On</th>\r\n                                <th>Action</th>\r\n                            </tr>\r\n                        </thead>\r\n                        <tbody> \r\n                            <ng-container *ngFor=\"let payor of payors\">\r\n                            <tr>\r\n                                <td>{{payor.payorId}}</td>\r\n                                <td>\r\n                                <b>Name</b>: {{payor.billToName}}<br/>\r\n                                <b>Address 1</b>: {{payor.billToAddress1}}<br/>\r\n                                <b>Address 2</b>: {{payor.billToAddress2}}<br/>\r\n                                <b>City</b>: {{payor.billToCity}}<br/>\r\n                                <b>State</b>: {{payor.billToState}}<br/>\r\n                                <b>Phone Number</b>: {{payor.phoneNumber}}<br/>\r\n                                </td>\r\n                                <td>{{payor.notes}}</td>\r\n                                <td class=\"center\">{{payor.createdOn | date:\"medium\"}}</td>\r\n                                <td class=\"center\">{{payor.updatedOn | date:\"medium\"}}</td>\r\n                                <td>\r\n                                    <button type=\"button\" class=\"btn btn-xs btn-primary\" title =\"View\"><i class=\"fa fa-eye-slash\"></i></button>                     \r\n                                    <button type=\"button\" class=\"btn btn-xs btn-info\"  title =\"Edit\"><i class=\"fa fa-pencil-square\"></i></button>                     \r\n                                    <button type=\"button\" class=\"btn btn-xs btn-danger\"  title =\"Delete\"><i class=\"fa fa-trash-o\"></i></button>                     \r\n                                </td>\r\n                            </tr>\r\n                            </ng-container>\r\n                        </tbody>\r\n                        <tfoot>\r\n                        <tr>\r\n                            <td colspan=\"3\"></td>\r\n                            <td colspan=\"3\" class=\"right\">\r\n                                <button type=\"button\" class=\"btn btn-default\"  (click)=\"prev()\" *ngIf=\"pageNumber>1\">Prev</button> \r\n                                <button type=\"button\" class=\"btn btn-info\">{{pageNumber}}</button>\r\n                                <button type=\"button\" class=\"btn btn-warning\" (click)=\"next()\">Next</button>\r\n                            </td>\r\n                            </tr>\r\n                        </tfoot>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n             <div class=\"overlay\" *ngIf=\"loading\">\r\n                <i class=\"fa fa-refresh fa-spin\"></i>\r\n            </div> \r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 312:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">General widget</h3></div>\r\n            <div class=\"box-body\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-primary\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-group fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\">26</div>\r\n                                        <div>Payors</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/payors'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-success\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-user fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\">26</div>\r\n                                        <div>Users</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/payors'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-warning\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-credit-card fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\"></div>\r\n                                        <div>Claims</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/claims'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 313:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3 \">\r\n        <div class=\"login-logo\">\r\n            <img [src]=\"'assets/logo/Color All.png'\" style=\"width:150px\" class=\"img-square\">\r\n        </div> \r\n    </div>\r\n</div>\r\n<div class=\"row\" *ngIf=\"!registered\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3\">        \r\n        <form role=\"form\"  [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"register()\">\r\n            <h4>Please complete form to register</h4>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"Email\" class=\"form-control\" placeholder=\"Email address\" formControlName=\"Email\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('Email').errors && submitted\">Email is required</p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"firstname\" class=\"form-control\" placeholder=\"Firstname\" formControlName=\"firstname\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('firstname').errors && submitted\">Firstname is required</p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"lastname\" class=\"form-control\" placeholder=\"Lastname\" formControlName=\"lastname\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('lastname').errors && submitted\">Lastname is required</p>\r\n                </div>                                \r\n                <div class=\"form-group\">\r\n                        <input type=\"password\" name=\"Password\" class=\"form-control bottom\" placeholder=\"Password\"  formControlName=\"Password\" required (focus)=\"submitted=false\">\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('Password').errors && submitted\">\r\n                            {{this.form.get('Password').getError('required') ? 'Password is required': 'Password validation creteria'}}\r\n                        </p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                        <input type=\"password\" name=\"password\" class=\"form-control bottom\" placeholder=\"Repeat password\"  formControlName=\"ConfirmPassword\" required (focus)=\"submitted=false\">\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('ConfirmPassword').errors && submitted\">\r\n                            Repeated password does not match password entry\r\n                        </p>\r\n                </div>                                \r\n                <div class=\"form-group\">\r\n                    <span class=\"help-block\"><a [routerLink]=\"'/recover-lost-password'\">Forgotten password?</a></span>\r\n                </div>\r\n                <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"register()\">Register</button>\r\n            </form>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\" *ngIf=\"registered\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-12\">&nbsp;</div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-md-6 col-md-offset-4\">\r\n                <div class=\"alert alert-success\">\r\n                    <strong>Success!</strong> An email has been sent for you to verifiy your email address.\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n"

/***/ }),

/***/ 51:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimManager; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ClaimManager = (function () {
    function ClaimManager(http, events) {
        this.http = http;
        this.events = events;
        this.claims = [];
    }
    ClaimManager.prototype.search = function (data) {
        var _this = this;
        this.http.getClaimsData(data).map(function (res) { return res.json(); })
            .subscribe(function (result) {
            _this.claims = result;
            console.log(result);
        }, function (err) {
            console.log(err);
        });
    };
    return ClaimManager;
}());
ClaimManager = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["f" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__events_service__["a" /* EventsService */]) === "function" && _b || Object])
], ClaimManager);

var _a, _b;
//# sourceMappingURL=claim-manager.js.map

/***/ }),

/***/ 593:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(158);


/***/ }),

/***/ 67:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export Module */
/* unused harmony export Role */
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return UserProfile; });
var Module = (function () {
    function Module(id, name) {
        this.id = id;
        this.name = name;
    }
    return Module;
}());

var Role = (function () {
    function Role(id, canView, canEdit, canAdd, canDelete, module) {
        this.id = id;
        this.canAdd = canAdd;
        this.canEdit = canEdit;
        this.canView = canView;
        this.canDelete = canDelete;
        this.module = module;
    }
    return Role;
}());

var UserProfile = (function () {
    function UserProfile(id, login, displayName, email, userName, avatarUrl, createdOn, roles) {
        this.id = id;
        this.login = login;
        this.userName = userName;
        this.displayName = displayName;
        this.avatarUrl = avatarUrl;
        this.email = email;
        this.roles = roles;
    }
    return UserProfile;
}());

//# sourceMappingURL=profile.js.map

/***/ }),

/***/ 94:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(27);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppLayoutComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var AppLayoutComponent = (function () {
    function AppLayoutComponent(router, profileManager) {
        this.router = router;
        this.profileManager = profileManager;
    }
    AppLayoutComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(AppLayoutComponent.prototype, "isLoggedIn", {
        get: function () {
            if (this.profileManager.profile) {
                window['jQuery']('body').addClass('sidebar-mini');
                return true;
            }
            else {
                window['jQuery']('body').removeClass('sidebar-mini');
                window['jQuery']('body').addClass('sidebar-collapse');
                return false;
            }
        },
        enumerable: true,
        configurable: true
    });
    return AppLayoutComponent;
}());
AppLayoutComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-layout',
        template: __webpack_require__(303),
        styles: [__webpack_require__(267)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _b || Object])
], AppLayoutComponent);

var _a, _b;
//# sourceMappingURL=app-layout.component.js.map

/***/ }),

/***/ 95:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__ = __webpack_require__(51);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimsComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ClaimsComponent = (function () {
    function ClaimsComponent(claimManager, http) {
        this.claimManager = claimManager;
        this.http = http;
    }
    ClaimsComponent.prototype.ngOnInit = function () {
        window['jQuery']('body').addClass('sidebar-collapse');
    };
    return ClaimsComponent;
}());
ClaimsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-claim',
        template: __webpack_require__(306),
        styles: [__webpack_require__(270)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _b || Object])
], ClaimsComponent);

var _a, _b;
//# sourceMappingURL=claim.component.js.map

/***/ }),

/***/ 96:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(36);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Error404Component; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var Error404Component = (function () {
    function Error404Component(_location) {
        this._location = _location;
    }
    Error404Component.prototype.ngOnInit = function () {
    };
    Error404Component.prototype.backClicked = function () {
        this._location.back();
    };
    return Error404Component;
}());
Error404Component = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-error404',
        template: __webpack_require__(307),
        styles: [__webpack_require__(271)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common__["c" /* Location */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common__["c" /* Location */]) === "function" && _a || Object])
], Error404Component);

var _a;
//# sourceMappingURL=error404.component.js.map

/***/ }),

/***/ 97:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__ = __webpack_require__(27);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__models_profile__ = __webpack_require__(67);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var LoginComponent = (function () {
    function LoginComponent(formBuilder, http, router, events, profileManager) {
        this.formBuilder = formBuilder;
        this.http = http;
        this.router = router;
        this.events = events;
        this.profileManager = profileManager;
        this.submitted = false;
        this.emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        this.form = this.formBuilder.group({
            email: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].pattern(this.emailRegex)])],
            password: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            grant_type: ['password'],
            rememberMe: [false],
        });
    }
    LoginComponent.prototype.reset = function () {
        this.router.navigate(['/recover-lost-password']);
    };
    LoginComponent.prototype.register = function () {
        this.router.navigate(['/register']);
    };
    LoginComponent.prototype.login = function () {
        var _this = this;
        this.submitted = true;
        if (this.form.valid) {
            try {
                this.http.login('username=' + this.form.get('email').value + '&password=' + this.form.get('password').value + "&grant_type=password", { 'Content-Type': 'x-www-form-urlencoded' }).subscribe(function (res) {
                    var data = res.json();
                    localStorage.setItem("user", JSON.stringify(data));
                    _this.events.broadcast('login', true);
                    _this.events.broadcast('profile', res.json());
                    _this.http.setAuth(data.access_token);
                    _this.profileManager.profile = new __WEBPACK_IMPORTED_MODULE_5__models_profile__["a" /* UserProfile */](data.id || data.userName, data.userName, data.userName, data.userName, data.userName);
                    _this.profileManager.setProfile(new __WEBPACK_IMPORTED_MODULE_5__models_profile__["a" /* UserProfile */](data.id || data.userName, data.userName, data.userName, data.userName, data.userName));
                    _this.router.navigate(['/main/private']);
                }, function (error) {
                    // if (error.status !== 500) {
                    _this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' });
                    // }
                });
            }
            catch (e) {
                this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' });
            }
            finally {
            }
        }
    };
    LoginComponent.prototype.ngOnInit = function () {
    };
    return LoginComponent;
}());
LoginComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-login',
        template: __webpack_require__(308),
        styles: [__webpack_require__(272)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_6__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__services_events_service__["a" /* EventsService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__["a" /* ProfileManager */]) === "function" && _e || Object])
], LoginComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=login.component.js.map

/***/ }),

/***/ 98:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MainComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var MainComponent = (function () {
    function MainComponent() {
    }
    MainComponent.prototype.ngOnInit = function () {
    };
    return MainComponent;
}());
MainComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-main',
        template: __webpack_require__(309),
        styles: [__webpack_require__(273)]
    }),
    __metadata("design:paramtypes", [])
], MainComponent);

//# sourceMappingURL=main.component.js.map

/***/ }),

/***/ 99:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PasswordResetComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var PasswordResetComponent = (function () {
    function PasswordResetComponent(formBuilder, http, router) {
        this.formBuilder = formBuilder;
        this.http = http;
        this.router = router;
        this.submitted = false;
        this.emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        this.form = this.formBuilder.group({
            email: ["", __WEBPACK_IMPORTED_MODULE_2__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_2__angular_forms__["d" /* Validators */].required, __WEBPACK_IMPORTED_MODULE_2__angular_forms__["d" /* Validators */].pattern(this.emailRegex)])],
        });
    }
    PasswordResetComponent.prototype.ngOnInit = function () {
    };
    PasswordResetComponent.prototype.resetPassword = function () {
        var _this = this;
        if (this.form.valid) {
            this.submitted = true;
            this.http.changepassword({ email: this.form.get('email').value })
                .subscribe(function (res) {
                _this.submitted = false;
            }, function (error) {
                _this.form.get('email').setErrors({ "error": "Incorrect email address" });
                _this.submitted = false;
            });
        }
    };
    PasswordResetComponent.prototype.register = function () {
        this.router.navigate(['/register']);
    };
    PasswordResetComponent.prototype.login = function () {
        this.router.navigate(['/login']);
    };
    PasswordResetComponent.prototype.submit = function () {
        var _this = this;
        this.submitted = true;
        if (this.form.valid) {
            try {
                this.http.passwordreset(this.form.value).subscribe(function (res) {
                    _this.router.navigate(['/main/login']);
                }, function (error) {
                    if (error.status !== 500) {
                        _this.form.get('email').setErrors({ 'auth': 'Incorrect email' });
                    }
                });
            }
            catch (e) {
                this.form.get('email').setErrors({ 'auth': 'Incorrect email' });
            }
            finally {
            }
        }
    };
    return PasswordResetComponent;
}());
PasswordResetComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* Component */])({
        selector: 'app-password-reset',
        template: __webpack_require__(310),
        styles: [__webpack_require__(274)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* Router */]) === "function" && _c || Object])
], PasswordResetComponent);

var _a, _b, _c;
//# sourceMappingURL=password-reset.component.js.map

/***/ })

},[593]);
//# sourceMappingURL=main.bundle.js.map