import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({ name: 'bridgeportDate' })
export class BridgeportDatePipe implements PipeTransform {
  constructor() {}

  transform(date: Date | string, format: string = 'MM/dd/yyyy'): string {
    return new DatePipe('en-US').transform(date, format);
  }
}
