webpackJsonp([2,5],{

/***/ 100:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-layout',
        template: __webpack_require__(351),
        styles: [__webpack_require__(305)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _b || Object])
], AppLayoutComponent);

var _a, _b;
//# sourceMappingURL=app-layout.component.js.map

/***/ }),

/***/ 101:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimNote; });
var ClaimNote = (function () {
    function ClaimNote(noteText, noteType) {
        this.noteText = noteText;
        this.noteType = noteType;
    }
    return ClaimNote;
}());

//# sourceMappingURL=claim-note.js.map

/***/ }),

/***/ 102:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_sweetalert2__ = __webpack_require__(628);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_sweetalert2___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_sweetalert2__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_notification__ = __webpack_require__(40);
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
        this.expanded = false;
        this.expandedBlade = 0;
    }
    ClaimsComponent.prototype.expand = function (expanded, expandedBlade) {
        this.expanded = expanded;
        this.expandedBlade = expandedBlade;
    };
    ClaimsComponent.prototype.minimize = function () {
        this.expanded = false;
        this.expandedBlade = 0;
    };
    ClaimsComponent.prototype.ngOnInit = function () {
        window['jQuery']('body').addClass('sidebar-collapse');
    };
    ClaimsComponent.prototype.addPrescriptionNote = function (text, TypeId, prescriptionNoteId) {
        var _this = this;
        if (text === void 0) { text = ""; }
        if (prescriptionNoteId === void 0) { prescriptionNoteId = null; }
        var selectedNotes = [];
        var prescriptionNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
        this.claimManager.PrescriptionNoteTypes.forEach(function (note) {
            prescriptionNoteTypeIds = prescriptionNoteTypeIds + '<option value="' + note.prescriptionNoteTypeId + '"' + (note.prescriptionNoteTypeId == TypeId ? "selected" : "") + '>' + note.typeName + '</option>';
        });
        var selectedPrecriptions = '';
        this.claimManager.selectedClaim.prescriptions.forEach(function (prescription) {
            if (prescription.selected) {
                selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex">' + prescription.labelName + '</span> &nbsp; ';
                selectedNotes.push(Number(prescription.rxNumber));
            }
        });
        __WEBPACK_IMPORTED_MODULE_3_sweetalert2___default()({
            title: 'New Prescription Note',
            html: "\n              <div class=\"form-group\">\n                  <label id=\"prescriptionNoteTypeLabel\">Prescription Note type</label>\n                  <select class=\"form-control\" id=\"prescriptionNoteTypeId\">\n                    " + prescriptionNoteTypeIds + "\n                  </select>\n              </div>\n              <div class=\"form-group\">\n                  <label id=\"noteTextLabel\">Note Text</label>\n                  <textarea class=\"form-control\"  id=\"noteText\" rows=\"3\">" + text + "</textarea>\n              </div>\n              <div style=\"text-align:left\">\n                  <h4 class=\"text-green\">Prescriptions</h4>\n                  " + selectedPrecriptions + "              \n              </div>\n        ",
            showCancelButton: true,
            showLoaderOnConfirm: true,
            confirmButtonText: "Save",
            preConfirm: function () {
                return new Promise(function (resolve) {
                    resolve([
                        window['jQuery']('#prescriptionNoteTypeId').val(),
                        window['jQuery']('#noteText').val()
                    ]);
                });
            },
            onOpen: function () {
                window['jQuery']('#prescriptionNoteTypeId').focus();
            }
        }).then(function (result) {
            if (result[0] == "") {
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])('Please select one type!');
                setTimeout(function () {
                    _this.addPrescriptionNote(result[1], result[0]);
                    window['jQuery']('#prescriptionNoteTypeLabel').css({ "color": "red" });
                }, 200);
            }
            else if (result[1] == "") {
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])('Note Text is required!');
                setTimeout(function () {
                    _this.addPrescriptionNote(result[1], result[0]);
                    window['jQuery']('#noteTextLabel').css({ "color": "red" });
                }, 200);
            }
            else {
                __WEBPACK_IMPORTED_MODULE_3_sweetalert2___default()({ title: "", html: "Saving note... <br/> <i class='fa fa-refresh fa-2x fa-spin'></i>", showConfirmButton: false });
                _this.http.savePrescriptionNote({
                    claimId: _this.claimManager.selectedClaim.claimId,
                    noteText: result[1],
                    prescriptionNoteTypeId: Number(result[0]),
                    prescriptions: selectedNotes,
                    prescriptionNoteId: prescriptionNoteId
                }).single().subscribe(function (result) {
                    console.log(result);
                    __WEBPACK_IMPORTED_MODULE_3_sweetalert2___default.a.close();
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["a" /* success */])("Noted successfully saved");
                }, function (error) {
                    setTimeout(function () {
                        _this.addPrescriptionNote(result[1], result[0]);
                        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])('Server error!');
                    }, 200);
                });
            }
        }).catch(__WEBPACK_IMPORTED_MODULE_3_sweetalert2___default.a.noop);
    };
    return ClaimsComponent;
}());
ClaimsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim',
        template: __webpack_require__(354),
        styles: [__webpack_require__(308)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _b || Object])
], ClaimsComponent);

var _a, _b;
//# sourceMappingURL=claim.component.js.map

/***/ }),

/***/ 103:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_http__ = __webpack_require__(71);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfirmEmailComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ConfirmEmailComponent = (function () {
    function ConfirmEmailComponent(route, req) {
        this.route = route;
        this.req = req;
        this.confirmed = 0;
    }
    ConfirmEmailComponent.prototype.ngOnInit = function () {
        this.hashChange = this.route.params.subscribe(function (params) {
            if (params['code'] && params['userId']) {
                console.log(params['code'], params['userId']);
            }
        });
    };
    return ConfirmEmailComponent;
}());
ConfirmEmailComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-confirm-email',
        template: __webpack_require__(355),
        styles: [__webpack_require__(309)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["c" /* ActivatedRoute */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["c" /* ActivatedRoute */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */]) === "function" && _b || Object])
], ConfirmEmailComponent);

var _a, _b;
//# sourceMappingURL=confirm-email.component.js.map

/***/ }),

/***/ 104:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(39);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-error404',
        template: __webpack_require__(356),
        styles: [__webpack_require__(310)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common__["Location"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common__["Location"]) === "function" && _a || Object])
], Error404Component);

var _a;
//# sourceMappingURL=error404.component.js.map

/***/ }),

/***/ 105:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__models_profile__ = __webpack_require__(55);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__models_notification__ = __webpack_require__(40);
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
                this.http.login('userName=' + this.form.get('email').value + '&password=' + this.form.get('password').value + "&grant_type=password", { 'Content-Type': 'x-www-form-urlencoded' }).subscribe(function (res) {
                    var data = res.json();
                    _this.events.broadcast('login', true);
                    _this.http.setAuth(data.access_token);
                    _this.http.profile().map(function (res) { return res.json(); }).subscribe(function (res) {
                        _this.profileManager.profile = new __WEBPACK_IMPORTED_MODULE_5__models_profile__["a" /* UserProfile */](data.id || res.email, res.email, res.firstName, res.lastName, res.email, res.email, null, data.createdOn);
                        _this.profileManager.setProfile(new __WEBPACK_IMPORTED_MODULE_5__models_profile__["a" /* UserProfile */](data.id || res.email, res.email, res.firstName, res.lastName, res.email, res.email, null, data.createdOn));
                        var user = res;
                        res.access_token = data.access_token;
                        localStorage.setItem("user", JSON.stringify(res));
                        console.log(_this.profileManager.profile);
                        _this.router.navigate(['/main/private']);
                        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_7__models_notification__["a" /* success */])('Welcome back');
                    }, function (err) { return console.log(err); });
                }, function (error) {
                    // if (error.status !== 500) {
                    var err = error.json();
                    _this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' });
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_7__models_notification__["b" /* warn */])(err.error_description);
                    // }
                });
            }
            catch (e) {
                this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' });
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_7__models_notification__["b" /* warn */])('Incorrect login or password');
            }
            finally {
            }
        }
        else {
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_7__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
        }
    };
    LoginComponent.prototype.ngOnInit = function () {
    };
    return LoginComponent;
}());
LoginComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-login',
        template: __webpack_require__(357),
        styles: [__webpack_require__(311)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_6__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__services_events_service__["a" /* EventsService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_profile_manager__["a" /* ProfileManager */]) === "function" && _e || Object])
], LoginComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=login.component.js.map

/***/ }),

/***/ 106:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-main',
        template: __webpack_require__(358),
        styles: [__webpack_require__(312)]
    }),
    __metadata("design:paramtypes", [])
], MainComponent);

//# sourceMappingURL=main.component.js.map

/***/ }),

/***/ 107:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__(19);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-password-reset',
        template: __webpack_require__(359),
        styles: [__webpack_require__(313)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* Router */]) === "function" && _c || Object])
], PasswordResetComponent);

var _a, _b, _c;
//# sourceMappingURL=password-reset.component.js.map

/***/ }),

/***/ 108:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-payors',
        template: __webpack_require__(360),
        styles: [__webpack_require__(314)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object])
], PayorsComponent);

var _a;
//# sourceMappingURL=payors.component.js.map

/***/ }),

/***/ 109:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
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
    };
    return PrivateComponent;
}());
PrivateComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-private',
        template: __webpack_require__(361),
        styles: [__webpack_require__(315)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], PrivateComponent);

var _a, _b, _c;
//# sourceMappingURL=private.component.js.map

/***/ }),

/***/ 110:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_profile__ = __webpack_require__(55);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__models_notification__ = __webpack_require__(40);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ProfileComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var ProfileComponent = (function () {
    function ProfileComponent(formBuilder, claimManager, http, profileManager) {
        this.formBuilder = formBuilder;
        this.claimManager = claimManager;
        this.http = http;
        this.profileManager = profileManager;
        this.submitted = false;
        this.loading = false;
        this.registered = false;
        this.emailRegex = '^[A-Za-z0-9]+(\.[_A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,15})$';
        this.saveLogin = false;
        this.saveEmail = false;
        this.loginError = '';
        this.emailError = '';
        if (this.profileManager.profile == null) {
            this.profileManager.profile = new __WEBPACK_IMPORTED_MODULE_4__models_profile__["a" /* UserProfile */]('', '', '', '', '');
        }
        this.form = this.formBuilder.group({
            oldPassword: ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            newPassword: ["", __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])],
            confirmPassword: ["", __WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_1__angular_forms__["d" /* Validators */].required])]
        });
    }
    ProfileComponent.prototype.ngOnInit = function () {
    };
    ProfileComponent.prototype.updatePassword = function () {
        var _this = this;
        this.submitted = true;
        if (this.form.valid && this.form.get('newPassword').value !== this.form.get('confirmPassword').value) {
            this.form.get('confirmPassword').setErrors({ "unmatched": "Confirm password does not match password" });
        }
        if (this.form.valid && !this.loading) {
            this.loading = true;
            try {
                this.http.changepassword(this.form.value).subscribe(function (res) {
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_6__models_notification__["a" /* success */])('Password successfully changed');
                    _this.registered = true;
                    _this.loading = false;
                }, function (error) {
                    var err = error.json() || ({ "Message": "Server error!" });
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_6__models_notification__["b" /* warn */])(err.Message);
                    _this.loading = false;
                });
            }
            catch (e) {
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_6__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
            }
        }
        else {
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_6__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
        }
    };
    return ProfileComponent;
}());
ProfileComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-profile',
        template: __webpack_require__(362),
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__["a" /* ClaimManager */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_http_service__["a" /* HttpService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_5__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__services_profile_manager__["a" /* ProfileManager */]) === "function" && _d || Object])
], ProfileComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=profile.component.js.map

/***/ }),

/***/ 111:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_notification__ = __webpack_require__(40);
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
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])('Password and Confirmed Password did not match password');
        }
        if (this.form.valid) {
            try {
                this.http.register(this.form.value).subscribe(function (res) {
                    console.log("Successful registration");
                    _this.router.navigate(['/logon']);
                    //console.log(res.json());
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["a" /* success */])("You have been signup successfully");
                    _this.registered = true;
                }, function (error) {
                    var err = error.json();
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])(err.error_description);
                });
            }
            catch (e) {
            }
            finally {
            }
        }
        else {
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
        }
    };
    return RegisterComponent;
}());
RegisterComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-register',
        template: __webpack_require__(363),
        styles: [__webpack_require__(316)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _c || Object])
], RegisterComponent);

var _a, _b, _c;
//# sourceMappingURL=register.component.js.map

/***/ }),

/***/ 112:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_notification__ = __webpack_require__(40);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__components_confirm_component__ = __webpack_require__(99);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ng2_bootstrap_modal__ = __webpack_require__(89);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ng2_bootstrap_modal___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_ng2_bootstrap_modal__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return UsersComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var UsersComponent = (function () {
    function UsersComponent(http, formBuilder, dialogService) {
        this.http = http;
        this.formBuilder = formBuilder;
        this.dialogService = dialogService;
        this.users = [];
        this.pageSize = 5;
        this.userRole = 'User';
        this.adminRole = 'Admin';
        this.roles = [];
        this.submitted = false;
        this.form = this.formBuilder.group({
            userName: [null],
            isAdmin: [null]
        });
        this.loading = false;
        this.getRoles();
        this.getUsers(1);
    }
    UsersComponent.prototype.next = function () {
        this.getUsers(this.pageNumber + 1);
    };
    UsersComponent.prototype.prev = function () {
        if (this.pageNumber > 1) {
            this.getUsers(this.pageNumber - 1);
        }
    };
    UsersComponent.prototype.ngOnInit = function () {
    };
    UsersComponent.prototype.getUsers = function (pageNumber) {
        var _this = this;
        this.loading = true;
        this.http.getUsers(pageNumber, this.pageSize).map(function (res) { _this.loading = false; return res.json(); }).subscribe(function (result) {
            result.forEach(function (element) {
                if (element.roles.includes(_this.userRole)) {
                    element.user = true;
                }
                else {
                    element.user = false;
                }
                if (element.roles.includes(_this.adminRole)) {
                    element.admin = true;
                }
                else {
                    element.admin = false;
                }
                element.activated = false;
                _this.users.push(element);
            });
            _this.pageNumber = pageNumber;
        }, function (err) {
            console.log(err);
        });
    };
    UsersComponent.prototype.getRoles = function () {
        var _this = this;
        var data = '';
        this.http.getRoles(data).map(function (res) { return res.json(); }).subscribe(function (result) {
            _this.roles = result.reduce(function (result, role) {
                result[role.name] = { name: role.name, id: role.id, users: role.users };
                return result;
            }, {});
        }, function (err) {
            console.log(err);
        });
    };
    UsersComponent.prototype.changeStatus = function (index, event) {
        var _this = this;
        var title = 'Activate/Deactivate';
        var msg = '';
        if (!event) {
            msg = 'Are you sure you want to disable ' + this.users[index].fullName + ' from the entire site?';
        }
        else {
            msg = 'Are you sure you want to enable ' + this.users[index].fullName + ' to use the entire site?';
        }
        var disposable = this.dialogService.addDialog(__WEBPACK_IMPORTED_MODULE_4__components_confirm_component__["a" /* ConfirmComponent */], {
            title: title,
            message: msg
        })
            .subscribe(function (isConfirmed) {
            //We get dialog result
            if (isConfirmed) {
                _this.processStatusChange(index, event);
            }
            else {
                _this.users[index].activated = !event;
            }
        });
    };
    UsersComponent.prototype.processStatusChange = function (index, event) {
        // console.log(this.users[index].active);
    };
    UsersComponent.prototype.processRoleChange = function (index, role, event) {
        var data;
        if (event) {
            data = { Id: this.roles[role].id, EnrolledUsers: this.users[index].id };
        }
        else {
            data = { Id: this.roles[role].id, RemovedUsers: this.users[index].id };
        }
        this.processRoleChangeRequest(data, role, index, event);
    };
    UsersComponent.prototype.showRoleConfirm = function (index, role, event) {
        var _this = this;
        var title = 'Update Role';
        var msg = '';
        var action = (event) ? 'Assgin ' + role + ' role to ' : 'Revoke ' + role + ' role from ';
        // console.log(this.users[index].admin , this.users[index].user , role , this.userRole ,event);
        if (this.users[index].admin && role == this.userRole && !event) {
            msg = 'Warning, revoking ' + this.users[index].fullName + ' from the User role will also revoke them from the Admin role.';
        }
        else {
            msg = 'Please confirm to ' + action + this.users[index].fullName;
        }
        var disposable = this.dialogService.addDialog(__WEBPACK_IMPORTED_MODULE_4__components_confirm_component__["a" /* ConfirmComponent */], {
            title: title,
            message: msg
        })
            .subscribe(function (isConfirmed) {
            //We get dialog result
            if (isConfirmed) {
                _this.processRoleChange(index, role, event);
            }
            else {
                if (role == _this.adminRole) {
                    _this.users[index].admin = !event;
                }
                if (role == _this.userRole) {
                    _this.users[index].user = !event;
                }
            }
        });
        //We can close dialog calling disposable.unsubscribe();
        //If dialog was not closed manually close it by timeout
        // setTimeout(() => {
        //   disposable.unsubscribe();
        // }, 10000);
    };
    UsersComponent.prototype.processRoleChangeRequest = function (data, role, index, event) {
        var _this = this;
        var msg = '';
        if (event) {
            msg = 'Assigned ' + this.users[index].firstName + ' ' + this.users[index].lastName + ' to the ' + role + ' role Successfully';
        }
        else {
            msg = 'Removed ' + this.users[index].firstName + ' ' + this.users[index].lastName + ' from the ' + role + ' role Successfully';
        }
        try {
            this.http.assignUserRole(data).subscribe(function (res) {
                console.log("Successful updated role");
                if (_this.users[index].admin && role == _this.userRole && !event) {
                    _this.users[index].admin = false;
                }
                else if (role == _this.adminRole && _this.users[index].admin) {
                    _this.users[index].user = true;
                }
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__models_notification__["a" /* success */])(msg);
            }, function (error) {
                var err = error.json();
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__models_notification__["b" /* warn */])('Some error occured, please try again');
                console.log(err.message);
            });
        }
        catch (e) {
        }
        finally {
        }
    };
    UsersComponent.prototype.search = function () {
        console.log(this.form.value);
    };
    return UsersComponent;
}());
UsersComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-users',
        template: __webpack_require__(364),
        styles: [__webpack_require__(317)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_5_ng2_bootstrap_modal__["DialogService"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5_ng2_bootstrap_modal__["DialogService"]) === "function" && _c || Object])
], UsersComponent);

var _a, _b, _c;
//# sourceMappingURL=users.component.js.map

/***/ }),

/***/ 113:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__http_service__ = __webpack_require__(12);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__http_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__profile_manager__ = __webpack_require__(26);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_1__profile_manager__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__claim_manager__ = __webpack_require__(20);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "e", function() { return __WEBPACK_IMPORTED_MODULE_2__claim_manager__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__events_service__ = __webpack_require__(18);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "c", function() { return __WEBPACK_IMPORTED_MODULE_3__events_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__auth_guard__ = __webpack_require__(224);
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "d", function() { return __WEBPACK_IMPORTED_MODULE_4__auth_guard__["a"]; });





//# sourceMappingURL=services.barrel.js.map

/***/ }),

/***/ 12:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map__ = __webpack_require__(133);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_http__ = __webpack_require__(71);
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
        return this.http.post("/oauth/token", data, { headers: headers });
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
        return this.http.put(this.baseUrl + "/account/changepassword", data, { headers: this.headers });
    };
    //register user
    HttpService.prototype.register = function (data) {
        return this.http.post(this.baseUrl + "/account/create", data);
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
    HttpService.prototype.getUsers = function (pageNumber, pageSize) {
        return this.http.get(this.baseUrl + "/account/users/?pageNumber=" + pageNumber + "&pageSize=" + pageSize, { headers: this.headers });
    };
    HttpService.prototype.getRoles = function (data) {
        return this.http.post(this.baseUrl + "/roles/", data, { headers: this.headers });
    };
    HttpService.prototype.assignUserRole = function (data) {
        return this.http.post(this.baseUrl + "/roles/ManageUsersInRole", data, { headers: this.headers });
    };
    Object.defineProperty(HttpService.prototype, "headers", {
        get: function () {
            var header = new __WEBPACK_IMPORTED_MODULE_2__angular_http__["b" /* Headers */]();
            header.append('Authorization', "Bearer " + this.token);
            //header.append('Authorization',"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTM4NCJ9.eyJuYW1laWQiOiJjYTcwNjJkZC04ZDEyLTQ4ODItOWUwNy01N2QxNWFmOGQ0YzAiLCJ1bmlxdWVfbmFtZSI6ImpvZ3dheWlAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS9hY2Nlc3Njb250cm9sc2VydmljZS8yMDEwLzA3L2NsYWltcy9pZGVudGl0eXByb3ZpZGVyIjoiQVNQLk5FVCBJZGVudGl0eSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiYjBlM2I2NjUtNTBhZS00NDU1LTkzYWItNWI5NGQ0MjRhMWRiIiwicm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiaXNzIjoiTE9DQUwgQVVUSE9SSVRZIiwiYXVkIjoiOWYyYzBhYzlkMGRiMGE5ZDE4NDM4YzgyOTZmN2FhYzExYjMwZGJjMTc2OTY1YmJiMDlhMjIyZDcwNTViZDE2MCIsImV4cCI6MTQ5OTg1NDE4MSwibmJmIjoxNDk4NjQ0NTgxfQ.4COg0PkqIRM1biFb9md65BYVHbkq2mAa-LgvRNsiAek-YXK9fOz3vXtOoZATUj3-");
            return header;
        },
        enumerable: true,
        configurable: true
    });
    HttpService.prototype.getNotetypes = function () {
        return this.http.get(this.baseUrl + "/claimnotes/notetypes", { headers: this.headers });
    };
    HttpService.prototype.getPrescriptionNotetypes = function () {
        return this.http.get(this.baseUrl + "/prescriptionnotes/notetypes", { headers: this.headers });
    };
    HttpService.prototype.saveClaimNote = function (data) {
        return this.http.post(this.baseUrl + "/claimnotes/savenote?claimId=" + data.claimId + "&noteText=" + data.noteText + "&noteTypeId=" + data.noteTypeId, {}, { headers: this.headers });
    };
    HttpService.prototype.savePrescriptionNote = function (data) {
        return this.http.post(this.baseUrl + "/prescriptionnotes/savenote", data, { headers: this.headers });
    };
    return HttpService;
}());
HttpService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_http__["c" /* Http */]) === "function" && _a || Object])
], HttpService);

var _a;
//# sourceMappingURL=http-service.js.map

/***/ }),

/***/ 173:
/***/ (function(module, exports) {

function webpackEmptyContext(req) {
	throw new Error("Cannot find module '" + req + "'.");
}
webpackEmptyContext.keys = function() { return []; };
webpackEmptyContext.resolve = webpackEmptyContext;
module.exports = webpackEmptyContext;
webpackEmptyContext.id = 173;


/***/ }),

/***/ 174:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__(208);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__(210);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__(225);




if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["enableProdMode"])();
}
__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 18:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__ = __webpack_require__(130);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [])
], EventsService);

//# sourceMappingURL=events-service.js.map

/***/ }),

/***/ 20:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable__ = __webpack_require__(126);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_immutable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_claim__ = __webpack_require__(222);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_claim_note__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__events_service__ = __webpack_require__(18);
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
        this.claims = __WEBPACK_IMPORTED_MODULE_0_immutable__["OrderedMap"]();
        this.loading = false;
        this.notetypes = [];
        this.prescriptionNotetypes = [];
    }
    ClaimManager.prototype.search = function (data) {
        var _this = this;
        this.loading = true;
        this.http.getClaimsData(data).map(function (res) { return res.json(); })
            .subscribe(function (result) {
            _this.loading = false;
            _this.selected = undefined;
            if (result.name) {
                _this.claims = __WEBPACK_IMPORTED_MODULE_0_immutable__["OrderedMap"]();
                var c = new __WEBPACK_IMPORTED_MODULE_1__models_claim__["a" /* Claim */](-10, result.claimNumber, result.dateEntered, result.injuryDate, result.gender, result.carrier, result.adjustor, result.adjustorPhoneNumber, result.dateEntered, result.adjustorPhoneNumber, result.name, result.firstName, result.lastName);
                _this.claims = _this.claims.set(-10, c);
                var claim = _this.claims.get(-10);
                claim.setPrescription(result.prescriptions);
                claim.setPayment(result.payments);
                claim.setEpisodes(result.episodes);
                claim.setClaimNotes(result.claimNotes ? new __WEBPACK_IMPORTED_MODULE_2__models_claim_note__["a" /* ClaimNote */](result.claimNotes[0].noteText, result.claimNotes[0].noteType.key) : null);
                claim.setPrescriptionNotes(result.prescriptionNotes);
            }
            else {
                var res = result;
                _this.claims = __WEBPACK_IMPORTED_MODULE_0_immutable__["OrderedMap"]();
                result.forEach(function (claim) {
                    var c = new __WEBPACK_IMPORTED_MODULE_1__models_claim__["a" /* Claim */](claim.claimId, claim.claimNumber, claim.dateEntered, claim.injuryDate, claim.gender, claim.carrier, claim.adjustor, claim.adjustorPhoneNumber, claim.dateEntered, claim.adjustorPhoneNumber, claim.name, claim.firstName, claim.lastName);
                    _this.claims = _this.claims.set(claim.claimId, c);
                });
            }
        }, function (err) {
            _this.loading = false;
            console.log(err);
        }, function () {
            _this.events.broadcast("claim-updated");
        });
        this.http.getNotetypes().map(function (res) { return res.json(); })
            .subscribe(function (result) {
            console.log("Claim Notes", result);
            _this.notetypes = result;
        }, function (err) {
            _this.loading = false;
            console.log(err);
        });
        this.http.getPrescriptionNotetypes().map(function (res) { return res.json(); })
            .subscribe(function (result) {
            console.log("Prescription Notes", result);
            _this.prescriptionNotetypes = result;
        }, function (err) {
            _this.loading = false;
            console.log(err);
        });
    };
    Object.defineProperty(ClaimManager.prototype, "dataSize", {
        get: function () {
            return this.claims.size;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimManager.prototype, "claimsData", {
        get: function () {
            return this.claims.asImmutable().toArray();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimManager.prototype, "NoteTypes", {
        get: function () {
            return this.notetypes;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimManager.prototype, "PrescriptionNoteTypes", {
        get: function () {
            return this.prescriptionNotetypes;
        },
        enumerable: true,
        configurable: true
    });
    ClaimManager.prototype.getClaimsDataById = function (id) {
        var _this = this;
        this.selected = id;
        var claim = this.claims.get(id);
        if (id !== -10) {
            this.loading = true;
            this.http.getClaimsData({ claimId: id }).map(function (res) { return res.json(); })
                .subscribe(function (result) {
                _this.loading = false;
                claim.setPrescription(result.prescriptions);
                claim.setPayment(result.payments);
                claim.setEpisodes(result.episodes);
                claim.setClaimNotes(result.claimNotes ? new __WEBPACK_IMPORTED_MODULE_2__models_claim_note__["a" /* ClaimNote */](result.claimNotes[0].noteText, result.claimNotes[0].noteType.key) : null);
                claim.setPrescriptionNotes(result.prescriptionNotes);
            }, function (err) {
                _this.loading = false;
                console.log(err);
            }, function () {
                _this.events.broadcast("claim-updated");
            });
        }
    };
    Object.defineProperty(ClaimManager.prototype, "selectedClaim", {
        get: function () {
            return this.claims.get(this.selected);
        },
        enumerable: true,
        configurable: true
    });
    return ClaimManager;
}());
ClaimManager = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */]) === "function" && _b || Object])
], ClaimManager);

var _a, _b;
//# sourceMappingURL=claim-manager.js.map

/***/ }),

/***/ 209:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_profile__ = __webpack_require__(55);
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
                var profile = new __WEBPACK_IMPORTED_MODULE_3__models_profile__["a" /* UserProfile */](us.id || us.email, us.email, us.firstName || us.email, us.lastName || us.email, us.email || us.email, us.email, us.avatarUrl, us.createdOn);
                this.profileManager.setProfile(profile);
                this.profileManager.profile = profile;
                var auth = localStorage.getItem("token");
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-root',
        template: "<router-outlet></router-outlet>"
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], AppComponent);

var _a, _b, _c;
//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 210:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__(41);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_http__ = __webpack_require__(71);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_common__ = __webpack_require__(39);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_component__ = __webpack_require__(209);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_ng2_bootstrap_modal__ = __webpack_require__(89);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_ng2_bootstrap_modal___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_ng2_bootstrap_modal__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__components_confirm_component__ = __webpack_require__(99);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__layouts_header_header_component__ = __webpack_require__(220);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__layouts_app_layout_component__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__layouts_sidebar_sidebar_component__ = __webpack_require__(221);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__pages_private_private_component__ = __webpack_require__(109);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__pages_login_login_component__ = __webpack_require__(105);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__pages_register_register_component__ = __webpack_require__(111);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__pages_main_main_component__ = __webpack_require__(106);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__pages_password_reset_password_reset_component__ = __webpack_require__(107);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__pages_error404_error404_component__ = __webpack_require__(104);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__app_routing__ = __webpack_require__(211);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__pages_profile_profile_component__ = __webpack_require__(110);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__ = __webpack_require__(113);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__pages_payors_payors_component__ = __webpack_require__(108);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__pages_claim_claim_component__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__components_claim_search_claim_search_component__ = __webpack_require__(219);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23__components_claim_result_claim_result_component__ = __webpack_require__(217);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_24__components_claim_payment_claim_payment_component__ = __webpack_require__(215);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_25__components_claim_images_claim_images_component__ = __webpack_require__(213);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_26__components_claim_prescriptions_claim_prescriptions_component__ = __webpack_require__(216);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_27__components_claim_note_claim_note_component__ = __webpack_require__(214);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_28__components_claim_episode_claim_episode_component__ = __webpack_require__(212);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_29__components_claim_script_note_claim_script_note_component__ = __webpack_require__(218);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_30__pages_users_users_component__ = __webpack_require__(112);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_31__pages_confirm_email_confirm_email_component__ = __webpack_require__(103);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_32__pages_users_filter_user_pipe__ = __webpack_require__(223);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Pipe"])({ name: 'safeStyle' }),
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Pipe"])({ name: 'safeURL' }),
    __metadata("design:paramtypes", [typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* DomSanitizer */]) === "function" && _b || Object])
], SafeUrlPipe);

var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */],
            __WEBPACK_IMPORTED_MODULE_7__components_confirm_component__["a" /* ConfirmComponent */],
            __WEBPACK_IMPORTED_MODULE_9__layouts_app_layout_component__["a" /* AppLayoutComponent */],
            __WEBPACK_IMPORTED_MODULE_16__pages_error404_error404_component__["a" /* Error404Component */],
            __WEBPACK_IMPORTED_MODULE_8__layouts_header_header_component__["a" /* HeaderComponent */],
            __WEBPACK_IMPORTED_MODULE_12__pages_login_login_component__["a" /* LoginComponent */],
            __WEBPACK_IMPORTED_MODULE_12__pages_login_login_component__["a" /* LoginComponent */],
            __WEBPACK_IMPORTED_MODULE_14__pages_main_main_component__["a" /* MainComponent */],
            __WEBPACK_IMPORTED_MODULE_15__pages_password_reset_password_reset_component__["a" /* PasswordResetComponent */],
            __WEBPACK_IMPORTED_MODULE_13__pages_register_register_component__["a" /* RegisterComponent */],
            SafeStylePipe, SafeUrlPipe, __WEBPACK_IMPORTED_MODULE_21__pages_claim_claim_component__["a" /* ClaimsComponent */], __WEBPACK_IMPORTED_MODULE_18__pages_profile_profile_component__["a" /* ProfileComponent */],
            __WEBPACK_IMPORTED_MODULE_10__layouts_sidebar_sidebar_component__["a" /* SidebarComponent */], __WEBPACK_IMPORTED_MODULE_11__pages_private_private_component__["a" /* PrivateComponent */], __WEBPACK_IMPORTED_MODULE_20__pages_payors_payors_component__["a" /* PayorsComponent */], __WEBPACK_IMPORTED_MODULE_22__components_claim_search_claim_search_component__["a" /* ClaimSearchComponent */], __WEBPACK_IMPORTED_MODULE_23__components_claim_result_claim_result_component__["a" /* ClaimResultComponent */], __WEBPACK_IMPORTED_MODULE_24__components_claim_payment_claim_payment_component__["a" /* ClaimPaymentComponent */], __WEBPACK_IMPORTED_MODULE_25__components_claim_images_claim_images_component__["a" /* ClaimImagesComponent */], __WEBPACK_IMPORTED_MODULE_26__components_claim_prescriptions_claim_prescriptions_component__["a" /* ClaimPrescriptionsComponent */], __WEBPACK_IMPORTED_MODULE_27__components_claim_note_claim_note_component__["a" /* ClaimNoteComponent */], __WEBPACK_IMPORTED_MODULE_28__components_claim_episode_claim_episode_component__["a" /* ClaimEpisodeComponent */], __WEBPACK_IMPORTED_MODULE_29__components_claim_script_note_claim_script_note_component__["a" /* ClaimScriptNoteComponent */], __WEBPACK_IMPORTED_MODULE_30__pages_users_users_component__["a" /* UsersComponent */], __WEBPACK_IMPORTED_MODULE_31__pages_confirm_email_confirm_email_component__["a" /* ConfirmEmailComponent */], __WEBPACK_IMPORTED_MODULE_32__pages_users_filter_user_pipe__["a" /* FilterUserPipe */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["b" /* BrowserModule */],
            __WEBPACK_IMPORTED_MODULE_6_ng2_bootstrap_modal__["BootstrapModalModule"],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["a" /* FormsModule */],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["b" /* ReactiveFormsModule */],
            __WEBPACK_IMPORTED_MODULE_3__angular_http__["a" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_17__app_routing__["a" /* RoutingModule */]
        ],
        providers: [
            __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__["a" /* HttpService */], __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__["b" /* ProfileManager */], __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__["c" /* EventsService */], __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__["d" /* AuthGuard */], __WEBPACK_IMPORTED_MODULE_19__services_services_barrel__["e" /* ClaimManager */],
            {
                provide: __WEBPACK_IMPORTED_MODULE_4__angular_common__["LocationStrategy"],
                useClass: __WEBPACK_IMPORTED_MODULE_4__angular_common__["HashLocationStrategy"]
            }
        ],
        entryComponents: [
            __WEBPACK_IMPORTED_MODULE_7__components_confirm_component__["a" /* ConfirmComponent */]
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]]
    })
], AppModule);

var _a, _b;
//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 211:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__layouts_app_layout_component__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__pages_login_login_component__ = __webpack_require__(105);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__pages_private_private_component__ = __webpack_require__(109);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__pages_register_register_component__ = __webpack_require__(111);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__pages_main_main_component__ = __webpack_require__(106);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__pages_password_reset_password_reset_component__ = __webpack_require__(107);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__pages_error404_error404_component__ = __webpack_require__(104);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__pages_payors_payors_component__ = __webpack_require__(108);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__pages_users_users_component__ = __webpack_require__(112);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__pages_claim_claim_component__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__services_services_barrel__ = __webpack_require__(113);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__pages_profile_profile_component__ = __webpack_require__(110);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__pages_confirm_email_confirm_email_component__ = __webpack_require__(103);
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
                canActivate: [__WEBPACK_IMPORTED_MODULE_12__services_services_barrel__["d" /* AuthGuard */]],
                children: [
                    {
                        path: 'private',
                        component: __WEBPACK_IMPORTED_MODULE_4__pages_private_private_component__["a" /* PrivateComponent */]
                    },
                    {
                        path: 'profile',
                        component: __WEBPACK_IMPORTED_MODULE_13__pages_profile_profile_component__["a" /* ProfileComponent */]
                    },
                    {
                        path: 'payors',
                        component: __WEBPACK_IMPORTED_MODULE_9__pages_payors_payors_component__["a" /* PayorsComponent */]
                    },
                    {
                        path: 'users',
                        component: __WEBPACK_IMPORTED_MODULE_10__pages_users_users_component__["a" /* UsersComponent */]
                    },
                    {
                        path: 'claims',
                        component: __WEBPACK_IMPORTED_MODULE_11__pages_claim_claim_component__["a" /* ClaimsComponent */]
                    }
                ]
            }
        ]
    },
    { path: 'confirm-email/:userId/:code', component: __WEBPACK_IMPORTED_MODULE_14__pages_confirm_email_confirm_email_component__["a" /* ConfirmEmailComponent */] },
    { path: '404', component: __WEBPACK_IMPORTED_MODULE_8__pages_error404_error404_component__["a" /* Error404Component */] },
    { path: '**', redirectTo: '/404' }
];
var RoutingModule = (function () {
    function RoutingModule() {
    }
    return RoutingModule;
}());
RoutingModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        imports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* RouterModule */].forRoot(routes)],
        exports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* RouterModule */]]
    })
], RoutingModule);

//# sourceMappingURL=app.routing.js.map

/***/ }),

/***/ 212:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimEpisodeComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ClaimEpisodeComponent = (function () {
    function ClaimEpisodeComponent(claimManager) {
        this.claimManager = claimManager;
    }
    ClaimEpisodeComponent.prototype.ngOnInit = function () {
    };
    return ClaimEpisodeComponent;
}());
ClaimEpisodeComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-episode',
        template: __webpack_require__(343),
        styles: [__webpack_require__(297)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object])
], ClaimEpisodeComponent);

var _a;
//# sourceMappingURL=claim-episode.component.js.map

/***/ }),

/***/ 213:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimImagesComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ClaimImagesComponent = (function () {
    function ClaimImagesComponent(claimManager) {
        this.claimManager = claimManager;
    }
    ClaimImagesComponent.prototype.ngOnInit = function () {
    };
    return ClaimImagesComponent;
}());
ClaimImagesComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-images',
        template: __webpack_require__(344),
        styles: [__webpack_require__(298)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object])
], ClaimImagesComponent);

var _a;
//# sourceMappingURL=claim-images.component.js.map

/***/ }),

/***/ 214:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_notification__ = __webpack_require__(40);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_claim_note__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_forms__ = __webpack_require__(25);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimNoteComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ClaimNoteComponent = (function () {
    function ClaimNoteComponent(claimManager, formBuilder, http) {
        this.claimManager = claimManager;
        this.formBuilder = formBuilder;
        this.http = http;
        this.form = this.formBuilder.group({
            //claimId: [this.claimManager.selectedClaim.claimId],
            noteText: [null, __WEBPACK_IMPORTED_MODULE_5__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_5__angular_forms__["d" /* Validators */].required])],
            noteTypeId: [null, __WEBPACK_IMPORTED_MODULE_5__angular_forms__["d" /* Validators */].compose([__WEBPACK_IMPORTED_MODULE_5__angular_forms__["d" /* Validators */].required])]
        });
    }
    ClaimNoteComponent.prototype.ngOnInit = function () {
    };
    ClaimNoteComponent.prototype.ngAfterViewChecked = function () {
        var text = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote ? this.claimManager.selectedClaim.claimNote.noteText : null;
        var noteTypeId = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote ? this.claimManager.selectedClaim.claimNote.noteType : null;
        if (this.claimManager.selectedClaim.claimNote !== undefined && this.form.get("noteText").value == null && this.form.get("noteText").value !== this.claimManager.selectedClaim.claimNote.noteText) {
            this.form.patchValue({
                noteTypeId: noteTypeId,
                noteText: text
            });
        }
    };
    ClaimNoteComponent.prototype.saveNote = function () {
        var _this = this;
        this.claimManager.loading = true;
        if (this.form.valid) {
            try {
                var note = this.form.value;
                note.claimId = this.claimManager.selectedClaim.claimId;
                this.http.saveClaimNote(this.form.value).subscribe(function (res) {
                    if (!_this.claimManager.selectedClaim.claimNote) {
                        _this.claimManager.selectedClaim.claimNote = new __WEBPACK_IMPORTED_MODULE_4__models_claim_note__["a" /* ClaimNote */](_this.form.value['noteText'], _this.form.value['noteTypeId']);
                    }
                    else {
                        _this.claimManager.selectedClaim.claimNote.noteText = _this.form.value['noteText'];
                    }
                    _this.claimManager.selectedClaim.editing = false;
                    _this.claimManager.loading = false;
                }, function (error) {
                    console.log(error);
                    _this.claimManager.loading = false;
                    var err = error.json();
                    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__models_notification__["b" /* warn */])(err.error_description);
                });
            }
            catch (e) {
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
                this.claimManager.loading = false;
            }
        }
        else {
            console.log(this.form.value);
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__models_notification__["b" /* warn */])('Error in fields. Please correct to proceed!');
            this.claimManager.loading = false;
        }
    };
    return ClaimNoteComponent;
}());
ClaimNoteComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-note',
        template: __webpack_require__(345),
        styles: [__webpack_require__(299)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_http_service__["a" /* HttpService */]) === "function" && _c || Object])
], ClaimNoteComponent);

var _a, _b, _c;
//# sourceMappingURL=claim-note.component.js.map

/***/ }),

/***/ 215:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimPaymentComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ClaimPaymentComponent = (function () {
    function ClaimPaymentComponent(claimManager) {
        this.claimManager = claimManager;
    }
    ClaimPaymentComponent.prototype.ngOnInit = function () {
    };
    return ClaimPaymentComponent;
}());
ClaimPaymentComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-payment',
        template: __webpack_require__(346),
        styles: [__webpack_require__(300)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object])
], ClaimPaymentComponent);

var _a;
//# sourceMappingURL=claim-payment.component.js.map

/***/ }),

/***/ 216:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_events_service__ = __webpack_require__(18);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimPrescriptionsComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ClaimPrescriptionsComponent = (function () {
    function ClaimPrescriptionsComponent(claimManager, events) {
        this.claimManager = claimManager;
        this.events = events;
    }
    ClaimPrescriptionsComponent.prototype.ngOnInit = function () {
        this.events.on("claim-updated", function () {
            setTimeout(function () {
                window['jQuery']('input[type="checkbox"].flat-red, input[type="radio"].flat-red')
                    .iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_flat-green'
                });
            }, 1000);
        });
    };
    return ClaimPrescriptionsComponent;
}());
ClaimPrescriptionsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-prescriptions',
        template: __webpack_require__(347),
        styles: [__webpack_require__(301)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_events_service__["a" /* EventsService */]) === "function" && _b || Object])
], ClaimPrescriptionsComponent);

var _a, _b;
//# sourceMappingURL=claim-prescriptions.component.js.map

/***/ }),

/***/ 217:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_events_service__ = __webpack_require__(18);
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
    function ClaimResultComponent(claimManager, formBuilder, router, events) {
        this.claimManager = claimManager;
        this.formBuilder = formBuilder;
        this.router = router;
        this.events = events;
    }
    ClaimResultComponent.prototype.ngOnInit = function () {
    };
    ClaimResultComponent.prototype.view = function (claimID) {
        this.claimManager.getClaimsDataById(claimID);
        this.minimize();
    };
    return ClaimResultComponent;
}());
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Object)
], ClaimResultComponent.prototype, "expand", void 0);
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Object)
], ClaimResultComponent.prototype, "minimize", void 0);
ClaimResultComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-result',
        template: __webpack_require__(348),
        styles: [__webpack_require__(302)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_events_service__["a" /* EventsService */]) === "function" && _d || Object])
], ClaimResultComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=claim-result.component.js.map

/***/ }),

/***/ 218:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__ = __webpack_require__(20);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ClaimScriptNoteComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ClaimScriptNoteComponent = (function () {
    function ClaimScriptNoteComponent(claimManager) {
        this.claimManager = claimManager;
    }
    ClaimScriptNoteComponent.prototype.ngOnInit = function () {
    };
    return ClaimScriptNoteComponent;
}());
ClaimScriptNoteComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-script-note',
        template: __webpack_require__(349),
        styles: [__webpack_require__(303)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object])
], ClaimScriptNoteComponent);

var _a;
//# sourceMappingURL=claim-script-note.component.js.map

/***/ }),

/***/ 219:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__ = __webpack_require__(20);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-claim-search',
        template: __webpack_require__(350),
        styles: [__webpack_require__(304)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_claim_manager__["a" /* ClaimManager */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["c" /* FormBuilder */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* Router */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__services_events_service__["a" /* EventsService */]) === "function" && _e || Object])
], ClaimSearchComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=claim-search.component.js.map

/***/ }),

/***/ 220:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__(19);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-header',
        template: __webpack_require__(352),
        styles: [__webpack_require__(306)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _d || Object])
], HeaderComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=header.component.js.map

/***/ }),

/***/ 221:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_http_service__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__(19);
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
            return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-sidebar',
        template: __webpack_require__(353),
        styles: [__webpack_require__(307)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_events_service__["a" /* EventsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_router__["a" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _d || Object])
], SidebarComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=sidebar.component.js.map

/***/ }),

/***/ 222:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Claim; });
var Claim = (function () {
    function Claim(claimId, claimNumber, dateOfBirth, injuryDate, gender, carrier, adjustor, adjustorPhoneNumber, dateEntered, adjustorFaxNumber, name, firstName, lastName) {
        this.prescription = [];
        this.prescriptionNote = [];
        this.payment = [];
        this.episode = [];
        this.editing = false;
        this.claimId = claimId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.name = name;
        this.claimNumber = claimNumber;
        this.dateOfBirth = dateOfBirth;
        this.injuryDate = injuryDate;
        this.gender = gender;
        this.carrier = carrier;
        this.adjustor = adjustor;
        this.adjustorPhoneNumber = adjustorPhoneNumber;
        this.dateEntered = dateEntered;
        this.adjustorFaxNumber = adjustorFaxNumber;
    }
    Claim.prototype.setPrescription = function (prescription) {
        if (prescription) {
            this.prescription = prescription;
        }
    };
    Object.defineProperty(Claim.prototype, "prescriptions", {
        get: function () {
            return this.prescription;
        },
        enumerable: true,
        configurable: true
    });
    Claim.prototype.setPayment = function (payments) {
        if (payments) {
            this.payment = payments;
        }
    };
    Object.defineProperty(Claim.prototype, "payments", {
        get: function () {
            return this.payment;
        },
        enumerable: true,
        configurable: true
    });
    Claim.prototype.setEpisodes = function (episodes) {
        if (episodes) {
            this.episode = episodes;
        }
    };
    Object.defineProperty(Claim.prototype, "episodes", {
        get: function () {
            return this.episode;
        },
        enumerable: true,
        configurable: true
    });
    Claim.prototype.setPrescriptionNotes = function (prescriptionNotes) {
        if (prescriptionNotes) {
            this.prescriptionNote = prescriptionNotes;
        }
    };
    Object.defineProperty(Claim.prototype, "prescriptionNotes", {
        get: function () {
            return this.prescriptionNote;
        },
        enumerable: true,
        configurable: true
    });
    Claim.prototype.setClaimNotes = function (claimNote) {
        if (claimNote) {
            this.claimNote = claimNote;
        }
    };
    Object.defineProperty(Claim.prototype, "claimNotes", {
        get: function () {
            return this.claimNote;
        },
        enumerable: true,
        configurable: true
    });
    return Claim;
}());

//# sourceMappingURL=claim.js.map

/***/ }),

/***/ 223:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FilterUserPipe; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var FilterUserPipe = (function () {
    function FilterUserPipe() {
    }
    FilterUserPipe.prototype.transform = function (users, searchText, isAdmin) {
        if (searchText == null && (isAdmin == null || !isAdmin))
            return users;
        return users.filter(function (user) {
            if (isAdmin && searchText == null) {
                return (user.admin);
            }
            else if (isAdmin && searchText != null) {
                return (user.admin) && (user.firstName.toLowerCase().includes(searchText.toLowerCase()) ||
                    user.lastName.toLowerCase().includes(searchText.toLowerCase()));
            }
            else {
                return user.firstName.toLowerCase().includes(searchText.toLowerCase()) ||
                    user.lastName.toLowerCase().includes(searchText.toLowerCase());
            }
        });
    };
    return FilterUserPipe;
}());
FilterUserPipe = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Pipe"])({
        name: 'filterUser'
    })
], FilterUserPipe);

//# sourceMappingURL=filter-user.pipe.js.map

/***/ }),

/***/ 224:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_events_service__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_add_operator_first__ = __webpack_require__(132);
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
                console.log("User is logged in");
                return true;
            }
            else {
                console.log(e, "User is not logged in");
                _this.router.navigate(['/login']);
            }
        }).catch(function (e) {
            console.log(e);
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
                return this.profileManager.userInfo(us.email).single().map(function (res) { return res.email ? true : false; });
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_events_service__["a" /* EventsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* Router */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_profile_manager__["a" /* ProfileManager */]) === "function" && _c || Object])
], AuthGuard);

var _a, _b, _c;
//# sourceMappingURL=auth.guard.js.map

/***/ }),

/***/ 225:
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

/***/ 26:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable__ = __webpack_require__(126);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_immutable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_immutable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_profile__ = __webpack_require__(55);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__http_service__ = __webpack_require__(12);
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
        var profile = new __WEBPACK_IMPORTED_MODULE_2__models_profile__["a" /* UserProfile */](u.id || u.userName, u.login || u.userName, u.firstName || u.userName, u.lastName || u.userName, u.email || u.userName, u.userName, u.avatarUrl, u.createdOn);
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
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__http_service__["a" /* HttpService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__events_service__["a" /* EventsService */]) === "function" && _b || Object])
], ProfileManager);

var _a, _b;
//# sourceMappingURL=profile-manager.js.map

/***/ }),

/***/ 297:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 298:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 299:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}\r\n\r\n.red{\r\n    color:red;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 300:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 301:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 302:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\ntable{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    padding-bottom:8px !important;\r\n    padding-top:3px !important;\r\n    font-size: 9pt;\r\n    word-wrap: break-word;\r\n}\r\ntr.active,tr.active td{\r\n    background:lightblue !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 303:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".table-responsive{\r\n    overflow-x: hidden;\r\n}\r\n    table{\r\n    table-layout:fixed;\r\n    width:99% !important;\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    font-size:9pt;\r\n    word-wrap: break-word;\r\n    padding-left:2px !important;\r\n    padding-right:2px  !important;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 304:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 305:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, " .content-wrapper{\r\n     min-height:90vh;\r\n }", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 306:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 307:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 308:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".row.data{\r\n    font-size:9.6pt;\r\n}\r\n.row.data .box{\r\n    margin-bottom:0px;\r\n}\r\n.box{\r\n    border:1.5px solid #d2d6de;\r\n}\r\n.box .box-body{\r\n    overflow-x: hidden;\r\n}\r\ntd,th{\r\n    padding-bottom:1px !important;\r\n    padding-top:1px !important;\r\n}\r\n.fittoSreen .box-body{\r\n    height:180px;\r\n    overflow-y: scroll;\r\n}\r\n\r\n.fittoSreen .box-body.claims{\r\n    height:250px;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 309:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "app-confirm-email,.wrapper{\r\n    background: purple;\r\n    height:100vh !important;\r\n    background: linear-gradient(to right, purple, brown); /*Safari 5.1-6*/ /*Opera 11.1-12*/ /*Fx 3.6-15*/ /*Standard*/\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 310:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 311:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".center-form{\r\n        width: 500px;\r\n        margin: auto;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 312:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 313:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".center-form{\r\n        width: 500px;\r\n        margin: auto;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 314:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 315:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 316:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".center-form{\r\n        width: 500px;\r\n        margin: auto;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 317:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(3)();
// imports


// module
exports.push([module.i, ".switch {\r\n  position: relative;\r\n  display: inline-block;\r\n  width: 60px;\r\n  height: 34px;\r\n}\r\n\r\n.switch input {display:none;}\r\n\r\n.slider {\r\n  position: absolute;\r\n  cursor: pointer;\r\n  top: 0;\r\n  left: 0;\r\n  right: 0;\r\n  bottom: 0;\r\n  background-color: #ccc;\r\n  transition: .4s;\r\n}\r\n\r\n.slider:before {\r\n  position: absolute;\r\n  content: \"\";\r\n  height: 26px;\r\n  width: 26px;\r\n  left: 4px;\r\n  bottom: 4px;\r\n  background-color: white;\r\n  transition: .4s;\r\n}\r\n\r\ninput:checked + .slider {\r\n  background-color: #4caf50;\r\n}\r\n\r\ninput:focus + .slider {\r\n  box-shadow: 0 0 1px #4caf50;\r\n}\r\n\r\ninput:checked + .slider:before {\r\n  -webkit-transform: translateX(26px);\r\n  transform: translateX(26px);\r\n}\r\n\r\n/* Rounded sliders */\r\n.slider.round {\r\n  border-radius: 34px;\r\n}\r\n\r\n.slider.round:before {\r\n  border-radius: 50%;\r\n}\r\n\r\n.active-switch {\r\n  margin: 50px auto;\r\n  position: relative;\r\n}\r\n\r\n.active-switch label {\r\n  width: 100%;\r\n  height: 100%;\r\n  position: relative;\r\n  display: block;\r\n}\r\n\r\n.active-switch input {\r\n  top: 0; \r\n  right: 0; \r\n  bottom: 0; \r\n  left: 0;\r\n  opacity: 0;\r\n  z-index: 100;\r\n  position: absolute;\r\n  width: 100%;\r\n  height: 100%;\r\n  cursor: pointer;\r\n}\r\n\r\n.btn-circle {\r\n  width: 30px;\r\n  height: 30px;\r\n  text-align: center;\r\n  padding: 6px 0;\r\n  font-size: 12px;\r\n  line-height: 1.428571429;\r\n  border-radius: 50%;\r\n}\r\n.btn-circle.btn-lg {\r\n  width: 50px;\r\n  height: 50px;\r\n  padding: 10px 16px;\r\n  font-size: 18px;\r\n  line-height: 1.33;\r\n}\r\n.btn-circle.btn-xl {\r\n  width: 70px;\r\n  height: 70px;\r\n  padding: 10px 16px;\r\n  font-size: 24px;\r\n  line-height: 1.33;\r\n}\r\n/* CSS used here will be applied after bootstrap.css */\r\n\r\n/* CSS used here will be applied after bootstrap.css */\r\n\r\n.demo {\r\n  padding:0px;\r\n  color:green;\r\n}\r\n\r\n.demo label{\r\n top:3px; left:15px;\r\n margin-right:30px;     \r\n position:relative;     \r\n}  \r\n  \r\n\r\ninput.faChkRnd, input.faChkSqr {\r\n  visibility: hidden;\r\n}\r\n\r\ninput.faChkRnd:checked:after, input.faChkRnd:after,\r\ninput.faChkSqr:checked:after, input.faChkSqr:after {\r\n  visibility: visible;\r\n  font-family: FontAwesome;\r\n  font-size:45px;height: 35px; width: 35px;\r\n  position: relative;\r\n  top: -3px;\r\n  left: 0px;\r\n  background-color:#FFF;\r\n  display: inline-block;\r\n}\r\n\r\ninput.faChkRnd:checked:after {\r\n  content: '\\F058';\r\n}\r\n\r\ninput.faChkRnd:after {\r\n  content: '\\F10C';\r\n}\r\n\r\ninput.faChkSqr:checked:after {\r\n  content: '\\F14A';\r\n}\r\n\r\ninput.faChkSqr:after {\r\n  content: '\\F096';\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 343:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <thead>\r\n              <tr>\r\n                <th>Date</th>\r\n                <th>By</th>\r\n                <th>Notes</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody *ngIf=\"claimManager.selectedClaim\">\r\n              <tr *ngFor=\"let episode of claimManager.selectedClaim.episodes\">\r\n                <td>{{episode.date  | date:\"shortDate\"}}</td>\r\n                <td>{{episode.by}}</td>\r\n                <td>{{episode.note}}</td>               \r\n              </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>"

/***/ }),

/***/ 344:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <thead>\r\n              <tr>\r\n                <th>Date</th>\r\n                <th>RxNum</th>\r\n                <th>Type</th>\r\n                <th>File</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody>\r\n              <!--<tr *ngFor=\"let pay of claimManager.selectedClaim.payments\">\r\n                <td>{{pay.date  | date:\"shortDate\"}}</td>\r\n                <td>{{pay.checkNumber}}</td>\r\n                <td>{{pay.RxNum}}</td>\r\n                <td>{{pay.checkAmount}}</td>               \r\n              </tr>-->\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>"

/***/ }),

/***/ 345:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\"  *ngIf=\"claimManager.selectedClaim && !claimManager.selectedClaim.editing\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <tbody>\r\n                  <tr>\r\n                    <td *ngIf=\"claimManager.selectedClaim && claimManager.selectedClaim.claimNote\">\r\n                      {{claimManager.selectedClaim.claimNote.noteText}}\r\n                    </td>\r\n                  </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>\r\n\r\n  <div class=\"row invoice-info\" *ngIf=\"claimManager.selectedClaim && claimManager.selectedClaim.editing\">\r\n      <form role=\"form\"  [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"saveNote()\">\r\n        <div class=\"form-group col-sm-offset-1 col-sm-10 invoice-col\" [class.has-error]=\"form.get('noteTypeId').errors\">\r\n              <label> <i class=\"fa fa-times-circle-o\" *ngIf=\"form.get('noteTypeId').errors\"></i> Claim Types</label>\r\n              <select class=\"form-control\" formControlName=\"noteTypeId\">\r\n                <option *ngFor=\"let note of claimManager.NoteTypes\" [value]=\"note.key\">{{note.value}}</option>\r\n              </select>\r\n        </div>\r\n        <div class=\"form-group col-sm-offset-1 col-sm-10 invoice-col\"  [class.has-error]=\"form.get('noteText').errors\">\r\n            <label>  <i class=\"fa fa-times-circle-o\" *ngIf=\"form.get('noteText').errors\"></i>  Claim Text</label>\r\n            <textarea class=\"form-control\"  name=\"noteText\" class=\"form-control\" formControlName=\"noteText\" focus-on></textarea>\r\n        </div>\r\n        <div class=\"form-group col-sm-offset-1 col-sm-10 invoice-col text-right\">\r\n          <button class=\"btn bg-purple btn-flat\" type=\"button\" (click)=\"saveNote()\">Save</button>\r\n          <button class=\"btn btn-danger btn-flat margin\" type=\"button\" (click)=\"claimManager.selectedClaim.editing=false\">Cancel</button>  \r\n        </div>\r\n      </form>\r\n  </div>"

/***/ }),

/***/ 346:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table>\r\n              <thead>\r\n              <tr>\r\n                <th>Date</th>\r\n                <th>CheckNum</th>\r\n                <th>RxNum</th>\r\n                <th>Check Amount</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody  *ngIf=\"claimManager.selectedClaim\">\r\n              <tr *ngFor=\"let pay of claimManager.selectedClaim.payments\">\r\n                <td>{{pay.date  | date:\"shortDate\"}}</td>\r\n                <td>{{pay.checkNumber}}</td>\r\n                <td>{{pay.RxNum}}</td>\r\n                <td>{{pay.checkAmount}}</td>               \r\n              </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>"

/***/ }),

/***/ 347:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <thead>\r\n              <tr>\r\n                <th>&nbsp;</th>\r\n                <th>RxNum</th>\r\n                <th>labelName</th>\r\n                <th>Bill To</th>\r\n                <th>Inv #</th>\r\n                <th>Inv Amount</th>\r\n                <th>Amount Paid</th>\r\n                <th>Outstanding</th>\r\n                <th>Inv Date</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody  *ngIf=\"claimManager.selectedClaim\">\r\n              <tr *ngFor=\"let prescription of claimManager.selectedClaim.prescriptions\">\r\n                <td>&nbsp;&nbsp;<input type=\"checkbox\" [checked]=\"prescription.selected\" (click)=\"prescription.selected=!prescription.selected\"></td>\r\n                <td>{{prescription.rxNumber}}</td> \r\n                <td>{{prescription.labelName}}</td>\r\n                <td>{{prescription.billTo}}</td>\r\n                <td>{{prescription.invoiceNumber}}</td>               \r\n                <td>{{prescription.invoiceAmount}}</td>               \r\n                <td>{{prescription.amountPaid}}</td>               \r\n                <td>{{prescription.outstanding}}</td>               \r\n                <td>{{prescription.invoiceDate | date:\"shortDate\"}}</td>               \r\n              </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>"

/***/ }),

/***/ 348:
/***/ (function(module, exports) {

module.exports = "<ng-container   *ngIf=\"claimManager.dataSize==1 || claimManager.selected\">\r\n    <div class=\"row invoice-info\" *ngFor=\"let claim of claimManager.claimsData\">\r\n      <div class=\"col-sm-6 invoice-col\" *ngIf=\"claimManager.dataSize==1 || claim.claimId==claimManager.selectedClaim.claimId\">\r\n        <address>\r\n          Name: {{claim.name || (claim.firstName+' '+claim.lastName)}}<br>\r\n          DOB: {{claim.dateOfBirth}}<br>\r\n          Gender: {{claim.gender}}<br>\r\n          Carrier: {{claim.carrier}}<br/>\r\n          Adjustor : {{claim.adjustor}}<br>\r\n          Adjustor Ph : {{claim.adjustorPhoneNumber}}<br><br>\r\n          Eligibility Entered: {{claim.dateEntered}}<br><br>\r\n        </address>\r\n      </div>\r\n      <!-- /.col -->\r\n      <div class=\"col-sm-6 invoice-col\"  *ngIf=\"claimManager.dataSize==1 || claim.claimId==claimManager.selectedClaim.claimId\">\r\n        <address>\r\n          Claim #: {{claim.claimNumber}}<br>\r\n          Injury Date: {{claim.injuryDate}}<br>\r\n          Adjustor Fax : {{claim.adjustorFaxNumber}}<br>\r\n        </address>\r\n        <br/><br/><br/><br/><br/>\r\n        <span class=\"label label-warning\" style=\"cursor:pointer;font-size:9pt\" (click)=\"view(claim.claimId);\"  *ngIf=\"claimManager.dataSize==1\"> View </span>\r\n        <button type=\"button\" class=\"btn btn-flat bg-purple btn-sm\" (click)=\"claimManager.selected=undefined\"  style=\"font-size:10pt\"  *ngIf=\"claimManager.dataSize>1\"> Back to Claim Results </button>\r\n      </div>\r\n  </div>\r\n</ng-container>\r\n<ng-container *ngIf=\"claimManager.dataSize>1 && ! claimManager.selected\">\r\n  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped table-hover\">\r\n              <thead>\r\n              <tr>\r\n                <th>Claim #</th>\r\n                <th>Name</th>\r\n                <th>Carrier</th>\r\n                <th>Injury Date</th>\r\n                <th>&nbsp;</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody>\r\n                <ng-container *ngFor=\"let claim of claimManager.claimsData\">\r\n                  <tr [class.active]=\"claimManager.selected && claim.claimId==claimManager.selectedClaim.claimId\">                \r\n                    <td>{{claim.claimNumber}}</td>\r\n                    <td *ngIf=\"claim.name\">{{claim.name}}</td>\r\n                    <td *ngIf=\"!claim.name\">{{claim.firstName}}  {{claim.lastName}}</td>\r\n                    <td>{{claim.carrier}}</td>\r\n                    <td>{{claim.injuryDate}}</td>\r\n                    <td><span class=\"label label-warning\" style=\"cursor:pointer;font-size:9pt\" (click)=\"view(claim.claimId)\"> View </span></td>\r\n                  </tr>\r\n                </ng-container>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>\r\n</ng-container>\r\n"

/***/ }),

/***/ 349:
/***/ (function(module, exports) {

module.exports = "  <div class=\"row invoice-info\">\r\n        <div class=\"col-sm-12 invoice-col\">\r\n          <div class=\"table-responsive\">\r\n            <table class=\"table no-margin table-striped\">\r\n              <thead>\r\n              <tr>\r\n                <th>Date</th>\r\n                <th>Type</th>\r\n                <th>By</th>\r\n                <th>Notes</th>\r\n              </tr>\r\n              </thead>\r\n              <tbody  *ngIf=\"claimManager.selectedClaim\">\r\n              <tr *ngFor=\"let pNotes of claimManager.selectedClaim.prescriptionNotes\">\r\n                <td>{{pNotes.date  | date:\"shortDate\"}}</td>\r\n                <td>{{pNotes.type}}</td>\r\n                <td>{{pNotes.enteredBy}}</td>\r\n                <td>{{pNotes.note}}</td>               \r\n              </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </div>\r\n  </div>"

/***/ }),

/***/ 350:
/***/ (function(module, exports) {

module.exports = "<form role=\"form\"  [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"search()\">\r\n    <div class=\"row\">\r\n        <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Claim #</label>\r\n              <input class=\"form-control\" name=\"claimNumber\" class=\"form-control\" formControlName=\"claimNumber\" (change)=\"textChange('claimNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>FirstName</label>\r\n              <input class=\"form-control\"  name=\"firstName\" class=\"form-control\" formControlName=\"firstName\"  (change)=\"textChange('firstName')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n       <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Last Name</label>\r\n              <input class=\"form-control\"  name=\"lastName\" class=\"form-control\" formControlName=\"lastName\"  (change)=\"textChange('lastName')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Rx Number</label>\r\n              <input class=\"form-control\"  name=\"rxNumber\" class=\"form-control\" formControlName=\"rxNumber\"  (change)=\"textChange('rxNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n      <div class=\"col-md-2\">\r\n          <div class=\"form-group\">\r\n              <label>Invoice #</label>\r\n              <input class=\"form-control\"  name=\"invoiceNumber\" class=\"form-control\" formControlName=\"invoiceNumber\"  (change)=\"textChange('invoiceNumber')\" (focus)=\"submitted=false\" focus-on>\r\n          </div>\r\n      </div>\r\n\r\n      <div class=\"col-md-2\">\r\n           <label>&nbsp;</label>\r\n          <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"search()\">Search</button>\r\n      </div>\r\n    </div>\r\n</form>\r\n"

/***/ }),

/***/ 351:
/***/ (function(module, exports) {

module.exports = "<div class=\"wrapper\" style=\"height: auto;\">\r\n    <!--top header -->\r\n    <app-header></app-header>\r\n    <app-sidebar *ngIf=\"isLoggedIn\"></app-sidebar>\r\n    <div class=\"content-wrapper\">\r\n        <router-outlet></router-outlet>\r\n    </div>\r\n    <!-- /.content-wrapper -->\r\n</div>"

/***/ }),

/***/ 352:
/***/ (function(module, exports) {

module.exports = "<header class=\"main-header\">\r\n    <!-- Logo -->\r\n    <a [routerLink]=\"'/'\" class=\"logo\">\r\n      <!-- mini logo for sidebar mini 50x50 pixels -->\r\n      <span class=\"logo-mini\"><b>BR</b>-C</span>\r\n      <!-- logo for regular state and mobile devices -->\r\n      <span class=\"logo-lg\">Bridgeport Claims</span>\r\n    </a>\r\n    <nav class=\"navbar navbar-static-top\">\r\n            <!-- Sidebar toggle button, check if user is logged in-->\r\n            <a href=\"#\" class=\"sidebar-toggle\" data-toggle=\"offcanvas\" role=\"button\" *ngIf=\"profileManager.profile\">\r\n                <span class=\"sr-only\">Toggle navigation</span>\r\n                <span class=\"icon-bar\"></span>\r\n                <span class=\"icon-bar\"></span>\r\n                <span class=\"icon-bar\"></span>\r\n            </a>\r\n            <!-- Top right menu items, also check if user is logged in-->\r\n            <div class=\"navbar-custom-menu\">\r\n                <ul class=\"nav navbar-nav\" *ngIf=\"!profileManager.profile\">                        \r\n                    <li><a [routerLink]=\"'/register'\">Register</a></li>\r\n                    <li><a [routerLink]=\"'/login'\">Login</a></li>\r\n                </ul>\r\n                <ul class=\"nav navbar-nav\" *ngIf=\"profileManager.profile\">                        \r\n                    <li routerLinkActive=\"active\" *ngIf=\"profileManager.profile\">\r\n                        <!--[routerLink]=\"'/profile'\"--> \r\n                        <a   class=\"navbar-link\" [routerLink]=\"'/main/profile'\">My Account</a>\r\n                    </li>\r\n                    <li routerLinkActive=\"active\" *ngIf=\"profileManager.profile\">\r\n                        <!--[routerLink]=\"'/profile'\"--> \r\n                        <a   class=\"navbar-link\" [routerLink]=\"'/main/profile'\">Logged in as {{profileManager.profile? profileManager.profile.firstName+' '+profileManager.profile.lastName : ''}}</a>\r\n                    </li>\r\n                    <li routerLinkActive=\"active\" *ngIf=\"profileManager.profile\">\r\n                        <a  style=\"cursor:pointer;\" (click)=\"logout()\" class=\"navbar-link\">Logout</a>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n\r\n    </nav>\r\n    \r\n</header>"

/***/ }),

/***/ 353:
/***/ (function(module, exports) {

module.exports = "<aside class=\"main-sidebar\">\r\n    <!-- sidebar: style can be found in sidebar.less -->\r\n    <section class=\"sidebar\">\r\n      <!-- Sidebar user panel -->\r\n      <div class=\"user-panel\">\r\n        <div class=\"pull-left image\">\r\n          <img [src]=\"'assets/logo/Color Logo.jpg'\" class=\"img-square\" [alt]=\"userName\">\r\n          <br style=\"line-height:2em\" *ngIf=\"!avatar\">\r\n        </div>\r\n        <div class=\"pull-left info\">\r\n          <p>{{userName}}</p>\r\n        </div>\r\n      </div>\r\n      <!-- sidebar menu: : style can be found in sidebar.less -->\r\n      <ul class=\"sidebar-menu\">\r\n        <li>\r\n          <a [routerLink]=\"'/main/private'\">\r\n            <i class=\"fa fa-dashboard\"></i> <span>Dashboard</span>\r\n          </a>\r\n        </li>\r\n        <li>\r\n            <a  [routerLink]=\"'/main/payors'\">\r\n              <i class=\"fa fa-user fa-fw\"></i> \r\n              <span>Payors</span>\r\n            </a>\r\n        </li>       \r\n        <li>\r\n            <a  [routerLink]=\"'/main/users'\">\r\n              <i class=\"fa fa-user fa-fw\"></i> \r\n              <span>Users</span>\r\n            </a>\r\n        </li>       \r\n        <li>\r\n            <a  [routerLink]=\"'/main/claims'\">\r\n              <i class=\"fa fa-credit-card fa-fw\"></i> \r\n              <span>Claims</span>\r\n            </a>\r\n        </li>       \r\n      </ul>\r\n    </section>\r\n    <!-- /.sidebar -->\r\n  </aside>"

/***/ }),

/***/ 354:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">Bridgeport Claims</h3></div>\r\n            <div class=\"box-body\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-sm-12\"   id=\"accordion\">\r\n                            <app-claim-search></app-claim-search>\r\n                    </div>\r\n                </div>\r\n                <div class=\"row data\" [class.fittoSreen]=\"claimManager.selected && !expanded\">\r\n                    <div [class.col-sm-5]=\"!expanded\"  [class.col-sm-12]=\"expanded && (expandedBlade==1 || expandedBlade==2 || expandedBlade==3)\" *ngIf=\"!expanded || (expandedBlade==1 || expandedBlade==2 || expandedBlade==3)\"  style=\"padding-right:0px;\">\r\n                        <div class=\"box\" *ngIf=\"(!expanded && expandedBlade==0) || expandedBlade==1\">\r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Claims</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                        <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,1)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body claims\">\r\n                                <app-claim-result [expand]=\"expand\" [minimize]=\"minimize\"></app-claim-result>\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"box\" *ngIf=\"(claimManager.selected && expandedBlade==0) || expandedBlade==2\">                    \r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Notes</u></h4>\r\n                                <div class=\"box-tools pull-right\" *ngIf=\"claimManager.selectedClaim && !claimManager.selectedClaim.editing\">\r\n                                    <div class=\"btn-group\" data-toggle=\"btn-toggle\">                            \r\n                                        <button type=\"button\" class=\"btn btn-flat bg-purple btn-sm\" (click)=\"claimManager.selectedClaim.editing=true\" *ngIf=\"!claimManager.selectedClaim.claimNotes\">Add New</button>                                \r\n                                        <button type=\"button\" class=\"btn btn-flat bg-purple btn-sm\" (click)=\"claimManager.selectedClaim.editing=true\" *ngIf=\"claimManager.selectedClaim.claimNotes\"   title=\"Edit Note\">Edit</button>                                \r\n                                    </div>\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                    <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,2)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                    &nbsp;&nbsp;&nbsp;\r\n                                </div>                       \r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-note></app-claim-note>\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"box\" *ngIf=\"(claimManager.selected  && expandedBlade==0) || expandedBlade==3\">                            \r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Episodes</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                    <div class=\"btn-group\" data-toggle=\"btn-toggle\">                            \r\n                                        <button type=\"button\" class=\"btn btn-flat bg-purple btn-sm\"   title=\"New Episode\">Add New</button>                                \r\n                                    </div>\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                    <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,3)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                    &nbsp;&nbsp;&nbsp;\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-episode></app-claim-episode>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                    <div [class.col-sm-7]=\"!expanded\"  [class.col-sm-12]=\"expanded && (expandedBlade==4 || expandedBlade==5 || expandedBlade==6 || expandedBlade==7)\" *ngIf=\"!expanded || (expandedBlade==4 || expandedBlade==5 || expandedBlade==6 || expandedBlade==7)\" style=\"padding-left:0px;\">\r\n                        <div class=\"box\" *ngIf=\"(claimManager.selected && expandedBlade==0) || expandedBlade==4\">                            \r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Prescriptions</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                        <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,4)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-prescriptions></app-claim-prescriptions>\r\n                            </div>\r\n                            <div class=\"box-footer\">\r\n                                <div class=\"btn-group\">\r\n                                    <button class=\"btn bg-purple btn-flat btn-small btn-block left\" type=\"button\" (click)=\"addPrescriptionNote()\">Add Note</button>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"box box-warn\" *ngIf=\"(claimManager.selected && expandedBlade==0) || expandedBlade==5\">                            \r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Script Notes</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                        <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,5)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-script-note></app-claim-script-note>\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"box box-warn\" *ngIf=\"(claimManager.selected && expandedBlade==0) || expandedBlade==6\">                            \r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Payments</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                        <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,6)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-payment></app-claim-payment>\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"box box-warn\" *ngIf=\"(claimManager.selected && expandedBlade==0) || expandedBlade==7\">\r\n                            <div class=\"box-header\">\r\n                                <h4 class=\"box-title text-center\"><u>Images</u></h4>\r\n                                <div class=\"box-tools pull-right\">\r\n                                        <button type=\"button\" *ngIf=\"expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"minimize()\"   title=\"Minimize\"><i class=\"fa fa-minus\"></i></button>\r\n                                        <button type=\"button\" *ngIf=\"!expanded\" class=\"btn btn-flat bg-green btn-sm\" (click)=\"expand(true,7)\"   title=\"Expand blade\"><i class=\"fa fa-expand\"></i></button>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"box-body\">\r\n                                <app-claim-images></app-claim-images>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n             <div class=\"overlay\" *ngIf=\"claimManager.loading\" style=\"text-align:center;\">\r\n                <img src=\"assets/1.gif\">\r\n            </div> \r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 355:
/***/ (function(module, exports) {

module.exports = " <div class=\"wrapper\">\r\n    <div class=\"row\" *ngIf=\"confirmed==0\"> \r\n      <div class=\"col-md-8 col-md-offset-2\">\r\n          <br><br><br>\r\n          <div class=\"box\">\r\n              <div class=\"box-body text-center\">\r\n                    <br><br><br>\r\n                    <h2>\r\n                      Confirming your email address ...\r\n                    </h2>\r\n                    <br><br><br>\r\n              </div>\r\n              <div class=\"overlay\" style=\"text-align:center;\">\r\n                  <!--<img src=\"assets/1.gif\" *ngIf=\"loading\">-->\r\n                  <i class=\"fa fa-refresh fa-2x fa-spin\"></i>\r\n              </div> \r\n          </div>\r\n      </div>\r\n  </div>\r\n  <div class=\"row\" *ngIf=\"confirmed==1\">\r\n      <div class=\"row\">\r\n          <div class=\"col-md-12\">&nbsp;</div>\r\n      </div>\r\n      <div class=\"row\">\r\n          <div class=\"col-md-6 col-md-offset-4\">\r\n              <div class=\"alert alert-success\">\r\n                  <strong>Success!</strong> An email has been sent for you to verifiy your email address.\r\n              </div>\r\n          </div>\r\n      </div>\r\n  </div>\r\n  <div class=\"row\" *ngIf=\"confirmed==2\">\r\n      <div class=\"row\">\r\n          <div class=\"col-md-12\">&nbsp;</div>\r\n      </div>\r\n      <div class=\"row\">\r\n          <div class=\"col-md-6 col-md-offset-4\">\r\n              <div class=\"alert alert-danger\">\r\n                  <strong>Error!</strong> An email has been sent for you to verifiy your email address.\r\n              </div>\r\n          </div>\r\n      </div>\r\n  </div>\r\n </div>"

/***/ }),

/***/ 356:
/***/ (function(module, exports) {

module.exports = "<div class=\"container\">\r\n    <div class=\"row\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-8 col-md-offset-2\">\r\n                 <h3>We can't seem to find the page you're looking for</h3>\r\n                <div class=\"row\">\r\n                    <div class=\"span5\">\r\n                        Please choose one of the locations below:\r\n                    </div>\r\n                </div>\r\n                <div class=\"row\"><br/></div>\r\n                <div class=\"row\">\r\n                    <div class=\"col-md-3\">\r\n                         <a [routerLink]=\"'/main/private'\" class=\"btn btn-primary btn-md btn-block\">Home</a>\r\n                    </div>\r\n                    <div class=\"col-md-3 col-md-offset-1\">\r\n                         <a (click)=\"backClicked()\" class=\"btn btn-default btn-md btn-block\">Go back</a>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>"

/***/ }),

/***/ 357:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3 \">\r\n        <div class=\"login-logo\">\r\n            <img [src]=\"'assets/logo/Color All.png'\" style=\"width:250px; padding-top: 50px;\" class=\"img-square\">\r\n        </div> \r\n    </div>\r\n</div>\r\n<div class=\"row\">\r\n    <div class=\"col-lg-12\">\r\n        <form role=\"form\" [formGroup]=\"form\" class=\"center-form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"login()\">\r\n            <h3>Please sign in</h3>\r\n            <div class=\"form-group\">\r\n                <input type=\"text\" name=\"email\" class=\"form-control\" placeholder=\"Email address\" formControlName=\"email\" (focus)=\"submitted=false\" required focus-on>\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').value!='' && form.get('email').errors && submitted\">Incorrect email</p>\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').value =='' && submitted\">Email is required</p>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <input type=\"password\" name=\"password\" class=\"form-control bottom\" placeholder=\"Password\" formControlName=\"password\" required (focus)=\"submitted=false\">\r\n                <p class=\"text-danger form-control-static\" *ngIf=\"form.get('password').errors && submitted\"> {{this.form.get('password').getError('required') ? 'Password is required': 'Incorrect email or password'}}</p>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <span class=\"help-block\"><a [routerLink]=\"'/recover-lost-password'\">Forgotten password?</a></span>\r\n            </div>\r\n            <div class=\"checkbox\">\r\n                <label><input type=\"checkbox\" value=\"true\" formControlName=\"rememberMe\"> Remember me</label>\r\n            </div>\r\n            <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"login()\">Sign in</button>\r\n        </form>\r\n    </div>\r\n</div>"

/***/ }),

/***/ 358:
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ 359:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-lg-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-body\"> \r\n                <form role=\"form\" class=\"center-form\" [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\">\r\n                    <h4>Enter  your email to recover lost password</h4>\r\n                    <div class=\"form-group\">\r\n                        <input type=\"text\" formControlName=\"email\" class=\"form-control\" placeholder=\"Email address or login\"\r\n                            required>\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('email').errors && form.get('email').value\">Invalid Email address</p>\r\n                    </div>\r\n                    <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"submit()\">Reset password</button>\r\n                </form>\r\n            </div>\r\n            <div class=\"overlay\" *ngIf=\"submitted\">\r\n                <i class=\"fa fa-refresh fa-spin\"></i>\r\n            </div>    \r\n        </div>    \r\n    </div>\r\n</div>\r\n"

/***/ }),

/***/ 360:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">Payors</h3></div>\r\n            <div class=\"box-body row\">\r\n                <div class=\"col-lg-12\"   id=\"accordion\">\r\n                        <div class=\"panel panel-default\">\r\n                            <div class=\"panel-heading\">\r\n                                <h4 class=\"panel-title\">\r\n                                    <a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\">Search and Filter</a>\r\n                                </h4>\r\n                            </div>\r\n                            <div id=\"collapseOne\" class=\"panel-collapse collapse out\">\r\n                                <div class=\"panel-body\">\r\n                                    Will add search and filter UI\r\n                                </div>\r\n                            </div> \r\n                        </div> \r\n                </div>\r\n                <div class=\"col-lg-11 col-lg-offset-1\">\r\n                    <table width=\"100%\" class=\"table table-striped table-bordered table-hover\" id=\"dataTables-example\">\r\n                        <thead>\r\n                            <tr>\r\n                                <th>ID</th>\r\n                                <th>Billing Details</th>\r\n                                <th width=\"20%\">Notes</th>\r\n                                <th>Created On</th>\r\n                                <th>Updated On</th>\r\n                                <th>Action</th>\r\n                            </tr>\r\n                        </thead>\r\n                        <tbody> \r\n                            <ng-container *ngFor=\"let payor of payors\">\r\n                            <tr>\r\n                                <td>{{payor.payorId}}</td>\r\n                                <td>\r\n                                <b>Name</b>: {{payor.billToName}}<br/>\r\n                                <b>Address 1</b>: {{payor.billToAddress1}}<br/>\r\n                                <b>Address 2</b>: {{payor.billToAddress2}}<br/>\r\n                                <b>City</b>: {{payor.billToCity}}<br/>\r\n                                <b>State</b>: {{payor.billToState}}<br/>\r\n                                <b>Phone Number</b>: {{payor.phoneNumber}}<br/>\r\n                                </td>\r\n                                <td>{{payor.notes}}</td>\r\n                                <td class=\"center\">{{payor.createdOn | date:\"medium\"}}</td>\r\n                                <td class=\"center\">{{payor.updatedOn | date:\"medium\"}}</td>\r\n                                <td>\r\n                                    <button type=\"button\" class=\"btn btn-xs btn-primary\" title =\"View\"><i class=\"fa fa-eye-slash\"></i></button>                     \r\n                                    <button type=\"button\" class=\"btn btn-xs btn-info\"  title =\"Edit\"><i class=\"fa fa-pencil-square\"></i></button>                     \r\n                                    <button type=\"button\" class=\"btn btn-xs btn-danger\"  title =\"Delete\"><i class=\"fa fa-trash-o\"></i></button>                     \r\n                                </td>\r\n                            </tr>\r\n                            </ng-container>\r\n                        </tbody>\r\n                        <tfoot>\r\n                        <tr>\r\n                            <td colspan=\"3\"></td>\r\n                            <td colspan=\"3\" class=\"right\">\r\n                                <button type=\"button\" class=\"btn btn-default\"  (click)=\"prev()\" *ngIf=\"pageNumber>1\">Prev</button> \r\n                                <button type=\"button\" class=\"btn btn-info\">{{pageNumber}}</button>\r\n                                <button type=\"button\" class=\"btn btn-warning\" (click)=\"next()\">Next</button>\r\n                            </td>\r\n                            </tr>\r\n                        </tfoot>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n             <div class=\"overlay\" *ngIf=\"loading\">\r\n                <i class=\"fa fa-refresh fa-spin\"></i>\r\n            </div> \r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 361:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\"><h3 class=\"box-title\">General widget</h3></div>\r\n            <div class=\"box-body\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-primary\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-group fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\">26</div>\r\n                                        <div>Payors</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/payors'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-success\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-user fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\">26</div>\r\n                                        <div>Users</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/users'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-3 col-md-6\">\r\n                        <div class=\"panel panel-warning\">\r\n                            <div class=\"panel-heading\">\r\n                                <div class=\"row\">\r\n                                    <div class=\"col-xs-3\">\r\n                                        <i class=\"fa fa-credit-card fa-5x\"></i>\r\n                                    </div>\r\n                                    <div class=\"col-xs-9 text-right\">\r\n                                        <div class=\"huge\"></div>\r\n                                        <div>Claims</div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                            <a [routerLink]=\"'/main/claims'\">\r\n                                <div class=\"panel-footer\">\r\n                                    <span class=\"pull-left\">Manage</span>\r\n                                    <span class=\"pull-right\"><i class=\"fa fa-arrow-circle-right\"></i></span>\r\n                                    <div class=\"clearfix\"></div>\r\n                                </div>\r\n                            </a>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n </div>"

/***/ }),

/***/ 362:
/***/ (function(module, exports) {

module.exports = "<div class=\"container\">\r\n    <div class=\"row\">&nbsp;</div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-6 col-md-offset-3\">\r\n            <div class=\"box\">\r\n                <div class=\"box-body\">\r\n                    <form role=\"form\" [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"updatePassword()\">\r\n                        <div class=\"form-group\">\r\n                            <label>Email</label>\r\n                            <input type=\"text\" name=\"email\" class=\"form-control\"                                \r\n                                value=\"{{profileManager.profile.email}}\" disabled>                            \r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label>First Name</label>\r\n                            <input type=\"text\" name=\"firstName\" class=\"form-control\"                                \r\n                                value=\"{{profileManager.profile.firstName}}\" disabled>                            \r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label>Last Name</label>\r\n                            <input type=\"text\" name=\"lastName\" class=\"form-control\"                                \r\n                                value=\"{{profileManager.profile.lastName}}\" disabled>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <input type=\"password\" formControlName=\"oldPassword\" class=\"form-control\"\r\n                                placeholder=\"Current password\"\r\n                                ng-model=\"currentPassword\" required>\r\n                            <p class=\"text-danger form-control-static\" *ngIf=\"form.get('oldPassword').errors && submitted\">\r\n                                Current password is required!</p>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <input type=\"password\" formControlName=\"newPassword\" class=\"form-control\" placeholder=\"New password\"\r\n                                ng-model=\"newPassword\" required>\r\n                            <p class=\"text-danger form-control-static\" *ngIf=\"form.get('newPassword').errors && submitted\">New password is required!</p>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <input type=\"password\" formControlName=\"confirmPassword\" class=\"form-control\"\r\n                                placeholder=\"Repeat new password\" ng-model=\"confirmPassword\" bs-match=\"newPassword\"\r\n                                required>\r\n                            <p class=\"text-danger form-control-static\"\r\n                            *ngIf=\"form.get('confirmPassword').errors && submitted\">Repeat Password does not match password!</p>\r\n                        </div>\r\n                        <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"updatePassword()\">Change password\r\n                        </button>\r\n                    </form>\r\n                </div>\r\n                <div class=\"overlay\" *ngIf=\"loading\" style=\"text-align:center;\">\r\n                    <!--<img src=\"assets/1.gif\" *ngIf=\"loading\">-->\r\n                    <i class=\"fa fa-refresh fa-2x fa-spin\"></i>\r\n                </div> \r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>"

/***/ }),

/***/ 363:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-xs-10 col-sm-6 col-md-6 col-lg-6 col-xs-offset-1 col-sm-offset-3 col-md-offset-3 col-lg-offset-3 \">\r\n        <div class=\"login-logo\">\r\n            <img [src]=\"'assets/logo/Color All.png'\" style=\"width:150px\" class=\"img-square\">\r\n        </div> \r\n    </div>\r\n</div>\r\n<div class=\"row\" *ngIf=\"!registered\">\r\n    <div class=\"col-lg-12\">        \r\n        <form role=\"form\"  [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"register()\" class=\"center-form\">\r\n            <h4>Please complete form to register</h4>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"Email\" class=\"form-control\" placeholder=\"Email address\" formControlName=\"Email\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('Email').errors && submitted\">Email is required</p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"firstname\" class=\"form-control\" placeholder=\"Firstname\" formControlName=\"firstname\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('firstname').errors && submitted\">Firstname is required</p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input class=\"form-control\"  name=\"lastname\" class=\"form-control\" placeholder=\"Lastname\" formControlName=\"lastname\" (focus)=\"submitted=false\" required focus-on>\r\n                    <p class=\"text-danger form-control-static\" *ngIf=\"form.get('lastname').errors && submitted\">Lastname is required</p>\r\n                </div>                                \r\n                <div class=\"form-group\">\r\n                        <input type=\"password\" name=\"Password\" class=\"form-control bottom\" placeholder=\"Password\"  formControlName=\"Password\" required (focus)=\"submitted=false\">\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('Password').errors && submitted\">\r\n                            {{this.form.get('Password').getError('required') ? 'Password is required': 'Password validation creteria'}}\r\n                        </p>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                        <input type=\"password\" name=\"password\" class=\"form-control bottom\" placeholder=\"Repeat password\"  formControlName=\"ConfirmPassword\" required (focus)=\"submitted=false\">\r\n                        <p class=\"text-danger form-control-static\" *ngIf=\"form.get('ConfirmPassword').errors && submitted\">\r\n                            Repeated password does not match password entry\r\n                        </p>\r\n                </div>                                \r\n                <div class=\"form-group\">\r\n                    <span class=\"help-block\"><a [routerLink]=\"'/recover-lost-password'\">Forgotten password?</a></span>\r\n                </div>\r\n                <button class=\"btn btn-primary btn-block\" type=\"button\" (click)=\"register()\">Register</button>\r\n            </form>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\" *ngIf=\"registered\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-12\">&nbsp;</div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-md-6 col-md-offset-4\">\r\n                <div class=\"alert alert-success\">\r\n                    <strong>Success!</strong> An email has been sent for you to verifiy your email address.\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n"

/***/ }),

/***/ 364:
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n    <div class=\"col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"box\">\r\n            <div class=\"box-header with-border\">\r\n                <h3 class=\"box-title\">Users</h3>\r\n            </div>\r\n            <div class=\"box-body row\">\r\n                <div class=\"col-lg-12\" id=\"accordion\">\r\n                    <div class=\"panel panel-default\">\r\n                        <div class=\"panel-heading\">\r\n                            <h4 class=\"panel-title\">\r\n                                <a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\">Search and Filter</a>\r\n                            </h4>\r\n                        </div>\r\n                        <div id=\"collapseOne\" class=\"panel-collapse collapse out\">\r\n                            <div class=\"panel-body\">\r\n                                <form role=\"form\" [formGroup]=\"form\" autocomplete=\"off\" autocapitalize=\"none\" autocomplete=\"off\" (keyup.enter)=\"search()\">\r\n                                    <div class=\"row\">\r\n                                        <div class=\"col-md-2\">\r\n                                            <div class=\"form-group\">\r\n                                                <label>Name</label>\r\n                                                <input class=\"form-control\" name=\"userName\" class=\"form-control\" formControlName=\"userName\" [(ngModel)]=\"userName\" (focus)=\"submitted=false\"\r\n                                                    focus-on>\r\n                                            </div>\r\n                                        </div>\r\n                                        <div class=\"col-md-2\">\r\n                                            <label>Display Only Admin</label>\r\n                                            <div class=\"form-group\">\r\n                                                <label class=\"switch\">\r\n                                                    <input  type=\"checkbox\" class=\"toggle-switch-checkbox\" formControlName=\"isAdmin\" [(ngModel)]=\"isAdmin\">\r\n                                                    <div class=\"slider round\"></div>\r\n                                                </label>\r\n                                            </div>\r\n                                        </div>\r\n                                    </div>\r\n                                </form>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-lg-11 col-lg-offset-1\">\r\n                        <table width=\"100%\" class=\"table table-striped table-bordered table-hover\" id=\"dataTables-example\">\r\n                            <thead>\r\n                                <tr>\r\n                                    <th>User Name</th>\r\n\r\n                                    <th>First Name</th>\r\n                                    <th>Last Name</th>\r\n                                    <th>Email Confirmed</th>\r\n                                    <th>Registered Date</th>\r\n                                    <th>Role User</th>\r\n                                    <th>Role Admin</th>\r\n                                    <th>Activate / Deactivate</th>\r\n                                </tr>\r\n                            </thead>\r\n                            <tbody>\r\n                                <ng-container *ngFor=\"let user of (users | filterUser : userName : isAdmin);let i = index\">\r\n                                    <tr>\r\n                                        <td>{{user.userName}}</td>\r\n                                        <td>{{user.firstName}}</td>\r\n                                        <td>{{user.lastName}}</td>\r\n                                        <td>{{user.emailConfirmed}}</td>\r\n                                        <td class=\"center\">{{user.registeredDate | date:\"shortDate\"}}</td>\r\n                                        <td>\r\n                                            <label class=\"switch\">\r\n                                            <input type=\"checkbox\" class=\"checkbox checkbox-slider--b checkbox-slider-md\" [(ngModel)]=\"user.user\" (ngModelChange)=\"showRoleConfirm(i,userRole,$event)\">\r\n                                            <div class=\"slider round\"></div>\r\n                                        </label>\r\n                                        </td>\r\n                                        <td>\r\n                                            <label class=\"switch\">\r\n                                            <input type=\"checkbox\" class=\"toggle-switch-checkbox\" [(ngModel)]=\"user.admin\" (ngModelChange)=\"showRoleConfirm(i,adminRole,$event)\">\r\n                                            <div class=\"slider round\"></div>\r\n                                        </label>\r\n                                        </td>\r\n                                        <td>\r\n                                            <div class=\"demo text-center\">                    \r\n                                                    <input type=\"checkbox\" class=\"faChkRnd\" [(ngModel)]=\"user.activated\" (ngModelChange)=\"changeStatus(i,$event)\"><label></label>\r\n                                            </div>\r\n                                        </td>\r\n                                    </tr>\r\n                                </ng-container>\r\n                            </tbody>\r\n                            <!--<tfoot>\r\n                        <tr>\r\n                            <td colspan=\"3\"></td>\r\n                            <td colspan=\"3\" class=\"right\">\r\n                                <button type=\"button\" class=\"btn btn-default\"  (click)=\"prev()\" *ngIf=\"pageNumber>1\">Prev</button> \r\n                                <button type=\"button\" class=\"btn btn-info\">{{pageNumber}}</button>\r\n                                <button type=\"button\" class=\"btn btn-warning\" (click)=\"next()\">Next</button>\r\n                            </td>\r\n                            </tr>\r\n                        </tfoot>-->\r\n                        </table>\r\n                    </div>\r\n                </div>\r\n                <div class=\"overlay\" *ngIf=\"loading\">\r\n                    <i class=\"fa fa-refresh fa-spin\"></i>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>"

/***/ }),

/***/ 40:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["b"] = warn;
/* harmony export (immutable) */ __webpack_exports__["a"] = success;
function warn(title) {
    jQuery.notifyClose();
    jQuery.notify({
        // options
        icon: "glyphicon glyphicon-warning-sign",
        message: title !== undefined ? title : 'Please correct the errors highlighted in red'
    }, {
        // settings
        type: 'warning',
        animate: {
            enter: 'animated swing',
            exit: 'animated fadeOutUp'
        },
        offset: 50,
        delay: 5000,
        z_index: 9991031,
        allow_dismiss: true,
        placement: {
            from: 'bottom',
            align: 'right'
        }
    });
}
function success(message) {
    jQuery.notify({
        // options
        icon: "glyphicon glyphicon-warning-sign",
        message: message
    }, {
        // settings
        type: 'success',
        animate: {
            enter: 'animated fadeInDown',
            exit: 'animated fadeOutUp'
        },
        offset: 50,
        delay: 5000,
        allow_dismiss: true,
        placement: {
            from: 'bottom',
            align: 'right'
        }
    });
}
//# sourceMappingURL=notification.js.map

/***/ }),

/***/ 55:
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
    function UserProfile(id, login, firstName, lastName, email, userName, avatarUrl, createdOn, roles) {
        this.id = id;
        this.login = login;
        this.userName = userName;
        this.firstName = firstName;
        this.lastName = lastName;
        this.avatarUrl = avatarUrl;
        this.email = email;
        this.roles = roles;
    }
    return UserProfile;
}());

//# sourceMappingURL=profile.js.map

/***/ }),

/***/ 702:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(174);


/***/ }),

/***/ 99:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal__ = __webpack_require__(89);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfirmComponent; });
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ConfirmComponent = (function (_super) {
    __extends(ConfirmComponent, _super);
    function ConfirmComponent(dialogService) {
        return _super.call(this, dialogService) || this;
    }
    ConfirmComponent.prototype.confirm = function () {
        // we set dialog result as true on click on confirm button, 
        // then we can get dialog result from caller code 
        this.result = true;
        this.close();
    };
    return ConfirmComponent;
}(__WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal__["DialogComponent"]));
ConfirmComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'confirm',
        template: "<div class=\"modal-dialog\">\n                <div class=\"modal-content\">\n                   <div class=\"modal-header\">\n                     <button type=\"button\" class=\"close\" (click)=\"close()\" >&times;</button>\n                     <h4 class=\"modal-title\">{{title || 'Confirm'}}</h4>\n                   </div>\n                   <div class=\"modal-body\">\n                     <p>{{message || 'Are you sure?'}}</p>\n                   </div>\n                   <div class=\"modal-footer\">\n                     <button type=\"button\" class=\"btn btn-primary\" (click)=\"confirm()\">OK</button>\n                     <button type=\"button\" class=\"btn btn-default\" (click)=\"close()\" >Cancel</button>\n                   </div>\n                 </div>\n              </div>"
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal__["DialogService"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1_ng2_bootstrap_modal__["DialogService"]) === "function" && _a || Object])
], ConfirmComponent);

var _a;
//# sourceMappingURL=confirm.component.js.map

/***/ })

},[702]);
//# sourceMappingURL=main.bundle.js.map