
/**
 * Created by Bahgat on 1/15/16.
 */

import { OnDestroy, Component, ViewContainerRef, ViewChild, AfterViewInit, ApplicationRef } from '@angular/core';
import { Observable } from 'rxjs';
import { WindowInstance } from './WindowInstance';
import { Subscription } from 'rxjs/Subscription';
import { global, CustomPosition } from './utils';
declare var $: any;




const makeInputObservable =
    (node: Node, eventName: string, useCapture?: boolean): Observable<UIEvent> =>
        Observable.fromEventPattern<MouseEvent>(
            (handler) => { node.addEventListener(eventName, <EventListener>handler, useCapture); },
            (handler) => { node.removeEventListener(eventName, <EventListener>handler, useCapture); }


        );


export class DragEvent {
    public cancelled = false;
    constructor(public mouseDown: UIEvent, public mouseMove: UIEvent, public position: CustomPosition, public offset: CustomPosition) {
    }
}


@Component({
    selector: 'bootstrap-modal',


    // inputs: ['dialogInstance.Myconfig.size.height' ],
    host: {
        'role': 'dialog',
        '[style.top]': 'dialogInstance.config.position.top + \'px\'',
        '[style.left]': 'dialogInstance.config.position.left +\'px\'',
        '[style.z-index]': 'compZIndex',
        style: `
            position: absolute;
            display: block; `,
        class: 'ngPopup  in',

    },

    template: `

    <div class="modal-dialog "
    [style]="'height:'+dialogInstance.config.size.height + 'px !important'|safeStyle"
    [style.width.px]="dialogInstance.config.size.width"
  >

 <div class="resizeCorner"  [hidden]="!dialogInstance.config.isResizable"  >
<div class="left-top-corner"></div>
<div class="left-bottom-corner"></div>
<div class="right-top-corner"  ></div>
<div class="right-bottom-corner"></div>
</div>

<div class="resizeBar"  [hidden]="!dialogInstance.config.isResizable"    >
<div class="top-bar" ></div>
<div class="right-bar" ></div>
<div class="bottom-bar"></div>
<div class="left-bar"></div>
</div>


<div class="titleBar" id="titleBar"  style="cursor:pointer;" (dblclick)="headerDblclick($event)"  >
    <span class="title">{{dialogInstance.config.Title}} </span><div class="iconGroup">
    <span   [style.display]="dialogInstance.config.canMinimize==false? 'none' : 'inline' " [class]="MinimizeStatus == false ? 'glyphicon glyphicon-minus' : 'glyphicon glyphicon-plus' "     (click)="minimize($event)"></span >
    <span [style.display]="dialogInstance.config.canMaximize==false? 'none' : 'inline' "   [class]="MaxmizeStatus == false ? 'glyphicon glyphicon-resize-full' : 'glyphicon glyphicon-resize-small' "  (click)="maximize($event)"></span>
     <span class="glyphicon glyphicon-remove" (click)="close()"></span>
   </div>
</div>
   <div class="modal-content"    >
      <!--<div class="modal-content"   style="display: block;height:100%;width:100%" >-->

            <div id="xxx" #modalDialog></div>
         </div>
    </div>
` ,


    // background: red;
    //        overflow: auto;

    // width: 100px;
    //        height: 300px;
})

export class BootstrapWindowContainer implements OnDestroy, AfterViewInit {

    set config(value: any) {
        this._config = value;
        this.setConfig(this._config);
    }
    dialogInstance: WindowInstance;
    compZIndex: number;

    private _originalWidth: number;
    private _originalHeight: number;
    private _originalLeft: number;
    private _originalTop: number;
    MaxmizeStatus = false;
    MinimizeStatus = false;
    //noinspection JSAnnotator
    private _dragSubscription: Subscription;
    private _isDragging = false;
    private _axis: string;
    private _config: any;
    private _mouseDelay = 50;
    private _mouseDelayMet: boolean;
    private _mouseDelayTimer: number;
    private _mouseDistance = 5;
    private _mouseDistanceMet = false;
    private _containment: ClientRect = null;
    private _dragOffsetX: number;
    private _dragOffsetY: number;
    private _elementStartX: number;
    private _elementStartY: number;
    private _model: any;

    listenerToDrag: any;
    public finish_Init;
    // modalDialog
    @ViewChild('modalDialog', { read: ViewContainerRef }) modalDialog_ViewContainer: ViewContainerRef;
    public viewRef;



    sizeChangedlisteners: Array<IDragListener> = Array<IDragListener>();
    ngAfterViewInit(): any {
        setTimeout(_ => this.finish_Init(this.modalDialog_ViewContainer));

    }

    set_finish_Init(callback) {
        this.finish_Init = callback;
    }

    constructor(dialogInstance: WindowInstance) {
        // this.SV = SV;
        global.MaxZIndex++;
        this.compZIndex = global.MaxZIndex;

        this.dialogInstance = dialogInstance;
        const win = $('body:not(.sidebar-collapse)');
        if (win.length === 0) {
            this.dialogInstance.config.position.left = this.dialogInstance.config.position.left - 50;
        } else {
            this.dialogInstance.config.position.left = this.dialogInstance.config.position.left - 250;
        }
        this.dialogInstance.config.BlockParentUI = true;

    }


    // onClick(dragEvent: DragEvent) {
    //    if( this.compZIndex !=SV.MaxZIndex) {
    //        SV.MaxZIndex++;
    //        this.compZIndex = SV.MaxZIndex;
    //    }
    // }

    documentKeypress(event: KeyboardEvent) {
        if (this.dialogInstance.config.keyboard &&
            (<Array<number>>this.dialogInstance.config.keyboard).indexOf(event.keyCode) > -1) {
            this.dialogInstance.close();
        }
    }

    close() {
        this.dialogInstance.close();
    }
    headerDblclick(event) {
        if (event.srcElement.className === 'titleBar' || event.srcElement.className === 'title') {
            this.maximize(event);
        }
    }



    maximize(event) {
        if (!this.dialogInstance.config.canMaximize) {
            return;
        }
        const x = 0, y = 53;
        $('.ngPopup .titleBar .iconGroup').css({ 'margin-top': '0px' });
        $('.ngPopup .titleBar .iconGroup span').css({ 'color': 'lightgrey' });
        const win = $('body:not(.sidebar-collapse)');
        if (!this.MaxmizeStatus) {

            this._originalTop = this.dialogInstance.config.position.top;
            this._originalLeft = this.dialogInstance.config.position.left;

            if (this.MinimizeStatus) {
                this.MinimizeStatus = false;
            } else {
                this._originalHeight = this.dialogInstance.config.size.height;
                this._originalWidth = this.dialogInstance.config.size.width;
            }


            this.dialogInstance.config.position.top = 53; // this.dialogInstance.config.minusTop;//jQuery('#af-header-0')[0].offsetHeight
            if (win.length === 0) {
                this.dialogInstance.config.minusLeft = 50;
            } else {
                this.dialogInstance.config.minusLeft = 230;
            }
            this.dialogInstance.config.position.left = this.dialogInstance.config.minusLeft;

            // backdropRef
            // if(elementRef.nativeElement.children[0].tagName=="ION-NAVBAR")
            // {
            //    config.size.height = elementRef.nativeElement.parentElement.offsetHeight-50
            // }

            if (this.dialogInstance.config.navbarHeight > 0) {



                this.dialogInstance.config.size.height = this.dialogInstance.backdropRef.location.nativeElement.closest('ion-page').offsetHeight - this.dialogInstance.config.minusHeight - this.dialogInstance.config.navbarHeight; // 40 بتاعة الهيدر
                this.dialogInstance.config.size.width = this.dialogInstance.backdropRef.location.nativeElement.closest('ion-page').offsetWidth - this.dialogInstance.config.minusWidth;

                // this.dialogInstance.config.size.height  = this.dialogInstance.backdropRef.location.nativeElement.offsetHeight-this.dialogInstance.config.navbarHeight;//40 بتاعة الهيدر
                // this.dialogInstance.config.size.width = this.dialogInstance.backdropRef.location.nativeElement.offsetWidth-3;



            } else {
                this.dialogInstance.config.size.height = this.dialogInstance.backdropRef.location.nativeElement.offsetParent.clientHeight - this.dialogInstance.config.minusTop - 50;
                this.dialogInstance.config.size.width = this.dialogInstance.backdropRef.location.nativeElement.offsetParent.clientWidth - this.dialogInstance.config.minusLeft - 10;
                // this.dialogInstance.config.size.height  = this.dialogInstance.backdropRef.location.nativeElement.closest(".modal-dialog").offsetHeight-44;//40 بتاعة الهيدر
                // this.dialogInstance.config.size.width = this.dialogInstance.backdropRef.location.nativeElement.closest(".modal-dialog").offsetWidth-5;

                // this.dialogInstance.config.size.height  = this.dialogInstance.backdropRef.location.nativeElement.offsetHeight-44;//40 بتاعة الهيدر
                // this.dialogInstance.config.size.width = this.dialogInstance.backdropRef.location.nativeElement.offsetWidth-5;

            }



        } else {
            this.dialogInstance.config.position.top = this._originalTop;
            this.dialogInstance.config.position.left = this._originalLeft;
            this.dialogInstance.config.size.width = this._originalWidth;
            this.dialogInstance.config.size.height = this._originalHeight;
            $('.ngPopup').css('bottom', 'unset');
            $('.ngPopup').css('top', (window.innerHeight / 2) - 50);
            $('#taskbar').css('display', 'none');
        }

        this.MaxmizeStatus = !this.MaxmizeStatus;

        this.sizeChanged(event);
    }

    minimize(e) {
        //    tempSizeForminimize
        //  tempPositionminimize
        if (!this.MinimizeStatus) {
            this.dialogInstance.config.BlockParentUI = false;
            $('.ngPopup .modal-content').css({ 'padding': '0px' });
            $('.ngPopup .titleBar .iconGroup').css({ 'margin-top': '0px' });
            $('.ngPopup .titleBar .iconGroup span').css({ 'color': 'black' });
            if (this.MaxmizeStatus) {
                this.dialogInstance.config.size.width = this.dialogInstance.config.minWidth;
                this.dialogInstance.config.size.height = 1;
                //  this.dialogInstance.config.position.top = this._originalTop;
                this.dialogInstance.config.position.left = this._originalLeft;
                this.MaxmizeStatus = false;
            } else {
                this._originalHeight = this.dialogInstance.config.size.height;
                this._originalWidth = this.dialogInstance.config.size.width;
                this._originalTop = this.dialogInstance.config.position.top;
                this._originalLeft = this.dialogInstance.config.position.left;

                this.dialogInstance.config.size.width = this.dialogInstance.config.minWidth;
                this.dialogInstance.config.size.height = 1;
            }
            $('.ngPopup').css('position', 'fixed');
            $('.ngPopup').css('bottom', 0);
            $('.ngPopup').css('top', 'unset');
            $('#taskbar').css('display', 'none');
            // توسيط الشاشة
            // this.dialogInstance.config.position.top = (window.innerHeight / 2 - this.tempSize.height / 2)
            // this.dialogInstance.config.position.left = (window.innerWidth / 2 - this.tempSize.width / 2)


        } else {
            $('.ngPopup .modal-content').css({ 'padding': '15px' });
            $('.ngPopup .titleBar .iconGroup').css({ 'margin-top': '0px' });
            $('.ngPopup .titleBar .iconGroup span').css({ 'color': 'lightgrey' });
            this.dialogInstance.config.BlockParentUI = true;
            global.MaxZIndex++;
            this.dialogInstance.config.position.top = this._originalTop;
            this.dialogInstance.config.position.left = this._originalLeft;
            this.dialogInstance.config.size.width = this._originalWidth;
            this.dialogInstance.config.size.height = this._originalHeight;
            $('.ngPopup').css('bottom', 'unset');
            $('.ngPopup').css('top', (window.innerHeight / 2) - 50);
            $('#taskbar').css('display', 'none');
        }

        this.MinimizeStatus = !this.MinimizeStatus;

        this.sizeChanged(e);


    }

    public initEvents() {

        //  click keydown keyup keypress mouseover mouseenter  mouseout mouseleave mousedown mouseup mousemove change blur focus scroll resize load unload beforeunload
        const mouseDownObservable = Observable.fromEvent(this.dialogInstance.bootstrapRef.location.nativeElement, 'mousedown').filter((md: MouseEvent) => md.which === 1);
        const mouseMoveObservable = Observable.fromEvent(document, 'mousemove');

        const mouseUpObservable = Observable.fromEvent(document, 'mouseup');
        const clickObservable = makeInputObservable(this.dialogInstance.bootstrapRef.location.nativeElement, 'click', true);
        const dragObservable = mouseDownObservable.flatMap((mouseDownEvent: MouseEvent) => {
            // mouseDownEvent.preventDefault();
            mouseDownEvent.stopPropagation();
            this._start();
            return mouseMoveObservable
                .map((mouseMoveEvent: MouseEvent) => {
                    this._update(mouseDownEvent, mouseMoveEvent);


                    return new DragEvent(mouseDownEvent, mouseMoveEvent, this._generatePosition(mouseMoveEvent), new CustomPosition(this._dragOffsetX, this._dragOffsetY));
                })
                .filter((e) => {
                    return this._isDragging;

                }
                )
                .takeUntil(mouseUpObservable.map((mouseUpEvent) => {
                    clearInterval(this._mouseDelayTimer);
                    if (this._isDragging) {
                        // this.mydragCode(mouseUpEvent);
                        // this.dragStop.next(mouseUpEvent);
                    }
                })
                    .zip(clickObservable.map((clickEvent: MouseEvent) => {
                        if (this._isDragging) {
                            clickEvent.stopPropagation();
                            this._isDragging = false;
                            this.sizeChanged(event);
                        }
                    }))

                );
        });

        // mouseDownObservable.flatMap(<DragEvent>((mouseDownEvent: MouseEvent) =>{},MouseEvent>)();


        this._dragSubscription = dragObservable.subscribe((event) => {
            this.onDrag(event);
            // this.drag.next(event);
            setTimeout(() => {
                if (!event.cancelled) {

                    if (event.mouseDown.srcElement.className !== 'titleBar' && event.mouseDown.srcElement.className !== 'title') {
                        this.MaxmizeStatus = false;
                        this.MinimizeStatus = false;
                    }
                    if (event.mouseDown.srcElement.className === 'titleBar' || event.mouseDown.srcElement.className === 'title') {
                        this.dialogInstance.config.position.top = event.position.top;
                        this.dialogInstance.config.position.left = event.position.left;
                    } else if (event.mouseDown.srcElement.className === 'bottom-bar') {
                        this.dialogInstance.config.size.height = this._originalHeight + event.offset.top;
                    } else if (event.mouseDown.srcElement.className === 'right-bar') {
                        this.dialogInstance.config.size.width = this._originalWidth + event.offset.left;
                    } else if (event.mouseDown.srcElement.className === 'left-bar') {
                        this.dialogInstance.config.size.width = this._originalWidth - event.offset.left;
                        this.dialogInstance.config.position.left = this._originalLeft + event.offset.left;

                    } else if (event.mouseDown.srcElement.className === 'top-bar') {
                        this.dialogInstance.config.position.top = this._originalTop + event.offset.top;
                        this.dialogInstance.config.size.height = this._originalHeight - event.offset.top;
                    } else if (event.mouseDown.srcElement.className === 'right-bottom-corner') {
                        this.dialogInstance.config.size.height = this._originalHeight + event.offset.top;
                        this.dialogInstance.config.size.width = this._originalWidth + event.offset.left;
                    } else if (event.mouseDown.srcElement.className === 'left-bottom-corner') {
                        this.dialogInstance.config.size.height = this._originalHeight + event.offset.top;
                        this.dialogInstance.config.size.width = this._originalWidth - event.offset.left;
                        this.dialogInstance.config.position.left = this._originalLeft + event.offset.left;
                    } else if (event.mouseDown.srcElement.className === 'left-top-corner') {
                        this.dialogInstance.config.position.top = this._originalTop + event.offset.top;
                        this.dialogInstance.config.size.height = this._originalHeight - event.offset.top;
                        this.dialogInstance.config.size.width = this._originalWidth - event.offset.left;
                        this.dialogInstance.config.position.left = this._originalLeft + event.offset.left;
                    } else if (event.mouseDown.srcElement.className === 'right-top-corner') {
                        this.dialogInstance.config.size.width = this._originalWidth + event.offset.left;
                        this.dialogInstance.config.position.top = this._originalTop + event.offset.top;
                        this.dialogInstance.config.size.height = this._originalHeight - event.offset.top;
                    }


                    //  event.cancelled = true;
                }
            });
        });
    }






    public ngOnDestroy(): void {
        this._dragSubscription.unsubscribe();
    }
    public setConfig(config: any): void {
        for (const key in config) {
            const value = config[key];
            switch (key) {
                case 'axis':
                    this._axis = value;
                    break;
                case 'delay':
                    this._mouseDelay = parseInt(value);
                    break;
                case 'distance':
                    this._mouseDistance = parseInt(value);
                    break;
                case 'containment':
                    this._containment = value;
                    break;
                case 'model':
                    this._model = value;
                    break;
            }
        }
    }

    private _generatePosition(event: MouseEvent): CustomPosition {
        const posX = (this._axis === 'y') ? this._elementStartX : this._elementStartX + this._dragOffsetX;
        const posY = (this._axis === 'x') ? this._elementStartY : this._elementStartY + this._dragOffsetY;
        return new CustomPosition(posX, posY);
    }

    private _start(): void {
        if (this.compZIndex !== global.MaxZIndex) {
            global.MaxZIndex++;
            this.compZIndex = global.MaxZIndex;
        }

        this._isDragging = false;
        this._mouseDelayMet = this._mouseDelay === 0;
        this._mouseDistanceMet = this._mouseDistance === 0;
        this._elementStartX = this.dialogInstance.bootstrapRef.location.nativeElement.offsetLeft;
        this._elementStartY = this.dialogInstance.bootstrapRef.location.nativeElement.offsetTop;
        if (!this._mouseDelayMet) {

            this._mouseDelayTimer = window.setTimeout(() => {
                this._mouseDelayMet = true;
            }, this._mouseDelay);
        }
    }

    private _update(mouseDownEvent: MouseEvent, mouseMoveEvent: MouseEvent): void {
        this._dragOffsetX = mouseMoveEvent.clientX - mouseDownEvent.clientX;
        this._dragOffsetY = mouseMoveEvent.clientY - mouseDownEvent.clientY;
        this._mouseDistanceMet = Math.abs(this._dragOffsetX) > this._mouseDistance || Math.abs(this._dragOffsetY) > this._mouseDistance;
        if (!this._isDragging && this._mouseDelayMet && this._mouseDistanceMet) {
            this.onDragStart(event);
            // this.dragStart.next(event);
            this._isDragging = true;
        }
    }

    private _setStyle(styleName: string, styleValue: string) {
        if (this._model) {
            this._model[styleName] = styleValue;
        } else {
            try {
                // this._renderer.setElementStyle(this.dialogInstance.bootstrapRef.location, styleName, styleValue);
            } catch (e) {
            }
        }
    }


    onDrag(dragEvent: DragEvent) {
    }
    onDrag1(dragEvent: DragEvent) {
    }

    public onDragStart(event: Event) {
        this._originalWidth = this.dialogInstance.config.size.width;
        this._originalHeight = this.dialogInstance.config.size.height;
        this._originalLeft = this.dialogInstance.config.position.left;
        this._originalTop = this.dialogInstance.config.position.top;
        // this.resizeStart.next(event);
    }

    addSizeChangedListener(listener: IDragListener) {
        this.sizeChangedlisteners.push(listener);
    }


    sizeChanged(event: Event) {
        for (let i = 0; i < this.sizeChangedlisteners.length; i++) {
            try {
                // setTimeout(() => {
                this.sizeChangedlisteners[i].onAfterDialogSizeChanged(event);
                // },5 );

            } catch (ex) {

            }
        }
        // for (var listener:IDragListener in this.sizeChangedlisteners) {
        //
        //             this.sizeChangedlisteners[listener].onAfterDialogSizeChanged(event);
        // }
    }
}

export interface IDragListener {
    onAfterDialogSizeChanged(event: Event);
}

