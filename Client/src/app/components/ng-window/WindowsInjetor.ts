﻿import {
    ReflectiveInjector,
    ComponentRef,
    ApplicationRef,
    ResolvedReflectiveProvider,
    Injectable,
    ViewContainerRef,
    ViewChild,
    ComponentFactory,
    ComponentFactoryResolver
} from '@angular/core';

import {WindowConfig} from './WindowConfig';
import {WindowInstance} from './WindowInstance';
import {WindowBackdrop} from './ModalBackdrop';
import {BootstrapWindowContainer} from './BootstrapWindowContainer';


@Injectable()
export class WindowsInjetor {

    // @ViewChild('target', {read: ViewContainerRef}) target;

    constructor(private componentFactoryResolver: ComponentFactoryResolver, private appRef: ApplicationRef) {

    }


    /**
     * openWindow a new modal window.
     * @param componentType A Component class (type) to render in the window. e.g: `ModalContent`.
     * @param ParentViewContainerRefORRootApp The parent location of the component. Note that it is not a rendered hierarchy, it is an injection hierarchy. e.g: the `ElementRef` of your current router view, etc...
     * @param parentInjector The injector used to create new component, if your `componentType` needs special injection (e.g: ModalContnetData) make sure you supply a suitable Injector.
     * @param config Modal configuration/options.
     * @returns Promise<WindowInstance> A promise of WindowInstance.
     */




    public openWindow(componentType: any,
                      options?: WindowConfig,
                      ParentViewContainerRefORRootApp?: ViewContainerRef,
                      anchorNameWillBe?: string,
                      bindings?: ResolvedReflectiveProvider[]): Promise<any> {


        if (!ParentViewContainerRefORRootApp) {

            // for ionic
            if ((this.appRef.components[0] as any )._component._overlayPortal) {
                 ParentViewContainerRefORRootApp = (this.appRef.components[0] as any )._component._overlayPortal._viewport;
            } else { //   for not ionic 
                const appInstance = this.appRef['components'][0].instance;

                if (!appInstance.viewContainerRef) {
                    const appName = this.appRef['components'][0].instance.viewContainerRef.element.nativeElement.localName;
                    throw new Error(`Missing 'viewContainerRef' declaration in ${appName} constructor , please declare viewContainerRef in the constructor of your app root component : ${appName} witn name viewContainerRef :(`);

                } else {
                    ParentViewContainerRefORRootApp = appInstance.viewContainerRef;
                }
            }
        }
        const config = options ? options : new WindowConfig();
        //   config = (config) ? WindowConfig.makeValid(config, _config) : _config;
        const dialog = new WindowInstance(config);
        let temp_bindings = ReflectiveInjector.resolve([

            {provide: WindowConfig, useValue: config},
            {provide: ViewContainerRef, useValue: ParentViewContainerRefORRootApp}
        ]);

        if (bindings) {
            temp_bindings = temp_bindings.concat(bindings);

        }

        dialog.inElement = !!anchorNameWillBe;


        const dialogBindings = ReflectiveInjector.resolve([{provide: WindowInstance, useValue: dialog}]);

        return this.createBackdrop2(ParentViewContainerRefORRootApp, dialogBindings, anchorNameWillBe)
            .then((backdropRef: ComponentRef<any>) => {
                dialog.backdropRef = backdropRef;
                let scroll_right = 0;
                // ParentViewContainerRefORRootApp.element.nativeElement.children[0].tagName == "ION-NAVBAR" &&
                if (dialog.backdropRef.location.nativeElement.parentElement.tagName === 'SCROLL-CONTENT') {
                    config.navbarHeight = 50;
                    scroll_right = 18;
                } else {
                    scroll_right = config.minusWidth;
                }
                if (config.openAsMaximize) {

                    config.position.top = config.minusTop;
                    config.position.left = config.minusLeft;
                    if (ParentViewContainerRefORRootApp.element.nativeElement.parentElement.offsetParent) {
                        config.size.width = ParentViewContainerRefORRootApp.element.nativeElement.parentElement.offsetParent.clientWidth - scroll_right;
                        config.size.height = ParentViewContainerRefORRootApp.element.nativeElement.parentElement.offsetParent.clientHeight - config.navbarHeight - config.minusHeight;
                    } else {
                        config.size.width = ParentViewContainerRefORRootApp.element.nativeElement.parentElement.clientWidth - scroll_right;
                        config.size.height = ParentViewContainerRefORRootApp.element.nativeElement.parentElement.clientHeight - config.navbarHeight - config.minusHeight;

                    }
                } else if (config.centerInsideParent) {
                    // DOM.setStyle(userComponent, 'width', '100%');
                    // DOM.getStyle(ParentViewContainerRefORRootApp.nativeElement.innerHeight,'innerHeight')
                    // DOM.getStyle('innerWidth')
                    if (ParentViewContainerRefORRootApp.element.nativeElement.innerHeight) {
                        config.position.top = (ParentViewContainerRefORRootApp.element.nativeElement.innerHeight / 2 - config.size.height / 2);
                        config.position.left = (ParentViewContainerRefORRootApp.element.nativeElement.innerWidth / 2 - config.size.width / 2);
                    } else {
                        config.position.top = (ParentViewContainerRefORRootApp.element.nativeElement.parentElement.offsetHeight / 2 - config.size.height / 2);
                        config.position.left = (ParentViewContainerRefORRootApp.element.nativeElement.parentElement.offsetWidth / 2 - config.size.width / 2);
                    }
                }

                const modalDataBindings = ReflectiveInjector.resolve([{
                    provide: WindowInstance,
                    useValue: dialog
                }]).concat(temp_bindings);
                const that = this;
                // this.componentLoader.loadAsRoot()
                return this.injectViewToBackdrop(ParentViewContainerRefORRootApp, dialogBindings)
                    .then((bootstrapRef: ComponentRef<any>) => {
                        dialog.bootstrapRef = bootstrapRef;
                        // bootstrapRef.location.nativeElement.appendChild()
                        //  bootstrapRef.location.nativeElement.appendChild(bootstrapRef.hostView);
                        return new Promise(function (resolve, reject) {
                            bootstrapRef.instance.set_finish_Init(function (viewContainerRefx: ViewContainerRef) {
                                componentType.selector = new Date().getTime();
                                const factory: ComponentFactory<any> = that.componentFactoryResolver.resolveComponentFactory(componentType);
                                // let factory:ComponentFactory<any> = bootstrapRef.instance.componentFactoryResolver.resolveComponentFactory(componentType);
                                const contentRef = viewContainerRefx.createComponent(factory);
                                bootstrapRef.instance.initEvents();
                                dialog.contentRef = contentRef;
                                resolve(contentRef.instance);
                            });
                        });
                    });
            });
    }

    private createBackdrop2(elementRef_viewContaner: ViewContainerRef, bindings: ResolvedReflectiveProvider[], anchorName?: string): Promise<ComponentRef<any>> {

        const factory: ComponentFactory<any> = this.componentFactoryResolver.resolveComponentFactory(WindowBackdrop);
        const childInjector = ReflectiveInjector.fromResolvedProviders(bindings, elementRef_viewContaner.parentInjector);

        return Promise.resolve(elementRef_viewContaner.createComponent(factory, elementRef_viewContaner.length, childInjector));

    }

    private injectViewToBackdrop(elementRef_viewContaner: ViewContainerRef, bindings: ResolvedReflectiveProvider[]): Promise<ComponentRef<any>> {

        const factory: ComponentFactory<any> = this.componentFactoryResolver.resolveComponentFactory(BootstrapWindowContainer);
        const childInjector = ReflectiveInjector.fromResolvedProviders(bindings, elementRef_viewContaner.parentInjector);

        return Promise.resolve(elementRef_viewContaner.createComponent(factory, elementRef_viewContaner.length, childInjector));

    }

    // private createBackdrop1(elementRef_viewContaner:ViewContainerRef, bindings:ResolvedReflectiveProvider[],  anchorName?:string): ComponentRef {
    //
    //     let factory: ComponentFactory<any> = this.componentFactoryResolver.resolveComponentFactory(WindowBackdrop);
    //     let childInjector = ReflectiveInjector.fromResolvedProviders(bindings, elementRef_viewContaner.parentInjector);
    //
    //
    //      return  elementRef_viewContaner.createComponent(factory, elementRef_viewContaner.length, childInjector);
    //
    //      this.componentFactoryResolver.resolveComponentFactory(WindowBackdrop).then((factory) => {
    //         this.componentRef = elementRef_viewContaner.createComponent(factory);
    //         // this.componentRef = this.foo.createComponent(factory);
    //         // this.componentRef = this.div.createComponent(factory);
    //         // this.componentRef = this.viewContainerRef.createComponent(factory);
    //     });
    //
    //
    //     this.resolver.resolveComponent(this.type).then((factory:ComponentFactory<any>) => {
    //         this.cmpRef = this.target.createComponent(factory);
    //         // here you can set inputs and set up subscriptions to outputs
    //         // input
    //         this.cmpRef.instance.someInput = someValue;
    //         // output
    //         this.cmpRef.instance.someOutput.subscribe(event=>{ });
    //     });
    //
    //
    //
    //    return this.componentFactoryResolver.resolveComponentFactory(WindowBackdrop).then((factory:ComponentFactory<any>) => {
    //         this.cmpRef = elementRef_viewContaner.createComponent(factory);
    //         // here you can set inputs and set up subscriptions to outputs
    //         // input
    //         this.cmpRef.instance.someInput = someValue;
    //         // output
    //         this.cmpRef.instance.someOutput.subscribe(event=>{  });
    //     });
    //
    //
    //
    //
    //     if(!anchorName)
    //     {
    //         //return this.componentLoader.loadNextToLocation()
    //         var simpleComponent = this.componentFactoryResolver.resolveComponentFactory(SimpleComponent);
    //         me.componentRef = me.dynamicTarget.createComponent(simpleComponent);
    //         return this.componentLoader.loadNextToLocation(WindowBackdrop, elementRef_viewContaner, bindings);
    //     }
    //     else
    //     {
    //         return this.componentLoader.loadNextToLocation(WindowBackdrop, elementRef_viewContaner, bindings);
    //
    //        // return this.componentLoader.loadIntoLocation(WindowBackdrop, elementRef_viewContaner, anchorName, bindings);
    //     }
    // }
    //
    //

}


