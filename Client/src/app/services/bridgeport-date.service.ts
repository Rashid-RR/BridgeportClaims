import { Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';

@Injectable({
    providedIn: 'root'
})
export class BridgeportDateService {

    public static FORMAT_DATE = 'MM/dd/yyyy'; // 'yyyy-MM-dd';
    public static FORMAT_DATE_TIME = 'MM/dd/yyyy hh:mma'; // 'yyyy-MM-ddTHH:mm:ss';
    public static FORMAT_DATE_SHORT = 'MM/dd/yy';
    public static FORMAT_DATE_ISO = 'yyyy-MM-dd';
    public static FORMAT_DATE_RAW = 'yyyyMMdd';

    static MS_PER_DAY = 1000 * 60 * 60 * 24;


    formatDate(date: Date | string | null): string | null {

        if (!date) {
            return null;
        }

        const p = new DatePipe('en-US');
        return p.transform(new Date(date), BridgeportDateService.FORMAT_DATE);
    }

    formatDateIso(date: Date | string | null): string | null {
        if (!date) {
            return null;
        }
        const p = new DatePipe('en-US');
        return p.transform(new Date(date), BridgeportDateService.FORMAT_DATE_ISO);
    }

    formatDateTime(date: Date | string | null): string | null {
        if (!date) {
            return null;
        }
        const p = new DatePipe('en-US');
        return p.transform(new Date(date), BridgeportDateService.FORMAT_DATE_TIME);
    }

    formatDateShort(date: Date | string | null): string | null {
        if (!date) {
            return null;
        }
        const p = new DatePipe('en-US');
        return p.transform(new Date(date), BridgeportDateService.FORMAT_DATE_SHORT);
    }

    formatDateRaw(date: Date | string | null): string | null {
      if (!date) {
          return null;
      }
      const p = new DatePipe('en-US');
      return p.transform(new Date(date), BridgeportDateService.FORMAT_DATE_RAW);
  }

    asDate(date: string | Date | null | undefined): Date | null {
        if (!date) {
            return null;
        }

        return new Date(date);
    }

    getDatePart(date: Date | string | null): Date | null {
        const d = this.asDate(date);
        if (!d) {
            return null;
        }

        return new Date(d.getFullYear(), d.getMonth(), d.getDate());
    }

    dateDiffInDays(a: Date | string, b: Date): number {

        if (!a || !b) {
            return 0;
        }

        const d1 = new Date(a);
        const d2 = new Date(b);

        // Discard the time and time-zone information.
        const utc1 = Date.UTC(d1.getFullYear(), d1.getMonth(), d1.getDate());
        const utc2 = Date.UTC(d2.getFullYear(), d2.getMonth(), d2.getDate());

        return Math.floor((utc2 - utc1) / BridgeportDateService.MS_PER_DAY);
    }

    isEmpty(date: Date | null) {
        if (!date) {
            return true;
        }

        if (date.toString() === '0001-01-01T00:00:00') {
            return true;
        }

        return false;
    }

    toLocaleDate(date: Date | string | null): string | null {

        const d = this.asDate(date);

        if (!d) {
            return null;
        }

        return d.toLocaleDateString();
    }
}
