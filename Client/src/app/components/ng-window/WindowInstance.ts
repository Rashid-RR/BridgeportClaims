 

import {ComponentRef} from "@angular/core";
import { PromiseCompleter } from './utils';
import {WindowConfig} from "./WindowConfig";

export class WindowInstance {
    private _bootstrapRef: ComponentRef<any>;
    private _backdropRef: ComponentRef<any> ;
    private _resultDeffered: PromiseCompleter<any> = new PromiseCompleter<any>();
    contentRef: ComponentRef<any>;
    public inElement: boolean;

    constructor(public config: WindowConfig) {


    }

    set backdropRef(value: ComponentRef<any>) {
        this._backdropRef = value;
    }

    get backdropRef()
    {
        return this._backdropRef;
    }
    set bootstrapRef(value: ComponentRef<any>) {
        this._bootstrapRef = value;
    }

    get bootstrapRef()
    {
        return this._bootstrapRef;
    }

    /**
     * A Promise that is resolved on a close event and rejected on a dimiss event.
     * @returns {Promise<T>|any|*|Promise<any>}
     */
    get result():Promise<any> {
        return this._resultDeffered.promise;
    }

    private dispose() {
        this._bootstrapRef.destroy();
        this._backdropRef.destroy();
    }
    /**
     *  Close the modal with a return value, i.e: result.
     */
    close(result: any = null) {
        this.dispose();
       // this._resultDeffered.resolve(result);
    }


    dismiss(){
        this.dispose();
    }



}



