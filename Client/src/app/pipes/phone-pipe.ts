import {Pipe} from '@angular/core';

@Pipe({
    name: 'phone'
})
export class PhonePipe{
    transform(val, args) {
        if(!val) return val;
        val = String(val);
        let newStr = '';
        newStr += '('+val.substring(0,3)+') ';
        newStr += val.substring(3,6)+'-';
        newStr += val.substring(6,val.length);
        return newStr;
    }
}