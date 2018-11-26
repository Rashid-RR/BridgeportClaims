import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'displayRoles'
})
export class DisplayRolesPipe implements PipeTransform {
  transform(value: String, args?: any): String {
     if (!value) {
       return value;
     }
     return value.toString().replace(/,/g, ', ');
  }
}
