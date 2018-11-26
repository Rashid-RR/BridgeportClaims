import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort'
})
export class ArraySortPipe  implements PipeTransform {
  transform(array: any[], field: string, dir: string= 'asc'): any[] {
    if (dir === 'asc') {
    array.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return -1;
      } else if (a[field] > b[field]) {
        return 1;
      } else {
        return 0;
      }
    });
  } else {
      array.sort((a: any, b: any) => {
        if (a[field] > b[field]) {
          return -1;
        } else if (a[field] < b[field]) {
          return 1;
        } else {
          return 0;
        }
      });
    }
    return array;
  }
}
