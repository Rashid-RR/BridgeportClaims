import { Component, OnInit, Renderer2, AfterViewInit, NgZone, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { EventsService } from '../../services/events-service';
import { Prescription } from '../../models/prescription';
import { PrescriptionNote } from '../../models/prescription-note';
import swal from 'sweetalert2';
import { DatePipe } from '@angular/common';
import { SortColumnInfo } from '../../directives/table-sort.directive';
declare var jQuery: any;

@Component({
  selector: 'app-claim-outstanding',
  templateUrl: './claim-outstanding.component.html',
  styleUrls: ['./claim-outstanding.component.css']
})
export class ClaimOutstandingComponent implements OnInit, AfterViewInit {

  @ViewChild('outstandingTable') table: ElementRef;
  sortColumn: SortColumnInfo;
  checkAll: Boolean = false;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;

  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    public claimManager: ClaimManager,
    private events: EventsService,
    private http: HttpService
  ) {
    this.claimManager.onClaimIdChanged.subscribe(() => {
      // this.fetchData();
    });
  }

  ngOnInit() {
    // this.fetchData();
    this.events.on('claim-updated', () => {

    });
    this.cloneTableHeading();
    this.events.on('reload:prespcriptions', () => {
      this.fetchData();
    });
  }

  ngAfterViewInit() {

  }


  cloneTableHeading() {

  }

  cloneBoxHeader() {
    const cln = document.getElementById;
  }

  updateTableHeadingWidth() {
    setTimeout(() => {
      const fixedHeader = document.getElementById('fixed-header');
      const fixedMaxHeader = document.getElementById('fixed-max-header');
      const mainTable = document.getElementById('maintable');
      if (fixedHeader) {
        if (mainTable) {
          const tableWidth = mainTable.clientWidth.toString();
          fixedHeader.style.width = tableWidth + 'px';
        }
      } else {
        if (mainTable) {
          const tableWidth = mainTable.clientWidth.toString();
          try {
            fixedMaxHeader.style.width = tableWidth + 'px';
          } catch (e) { }
        }
      }
    }, 500);
  }
  clicked() {

  }
  showNotes(prescriptionId: Number) {
    this.claimManager.loading = true;
    this.http.getPrescriptionNotes(prescriptionId).subscribe(res => {
      const notes: Array<PrescriptionNote> = res;
      this.displayNotes(notes);
    }, error => {
      this.claimManager.loading = false;
    });
  }

  displayNotes(notes: Array<PrescriptionNote>) {

    let notesHTML = '';
    notes.forEach(note => {

      const noteDate = this.dp.transform(note.rxDate, 'shortDate');
      notesHTML = notesHTML + `
            <tr>
              <td>` + noteDate + `</td>
              <td>` + note.type + `</td>
              <td>` + note.enteredBy + `</td>
              <td style="white-space: pre-wrap;">` + note.note + `</td>
            </tr>`;
    });
    const html = `<div class="row invoice-info">
              <div class="col-sm-12 invoice-col" style="text-align:left;font-size:10pt">
                <div class="table-responsive">
                  <table class="table no-margin table-striped">
                    <thead>
                    <tr>
                      <th>Rx Date</th>
                      <th>Type</th>
                      <th>By</th>
                      <th width="75%">Notes</th>
                    </tr>
                    </thead>
                    <tbody>
                    ` + notesHTML + `
                    </tbody>
                  </table>
                </div>
              </div>
        </div>`;
    this.claimManager.loading = false;
    swal({
      title: 'Prescription Note' + (notes.length > 1 ? 's' : ''),
      width: window.innerWidth * 3 / 4 + 'px',
      html: html
    }).then(_ => {

    }).catch(()=>{});
  }

  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  fetchData() {
    if (!this.claimManager.selectedClaim.loadingOutstanding) {
      this.claimManager.selectedClaim.loadingOutstanding = true;
      const page = 1;
      const page_size = 1000;
      let sort = 'rxDate';
      let sort_dir: 'asc' | 'desc' = 'desc';
      if (this.sortColumn) {
        sort = this.sortColumn.column;
        sort_dir = this.sortColumn.dir;
      }
      this.http.getOutstandingPrescriptions(this.claimManager.selectedClaim.claimId, sort, sort_dir,
        page, page_size)
        .subscribe((data:any) => {
          this.claimManager.selectedClaim.outstanding = data.results;
          this.claimManager.selectedClaim.totalOutstandingAmount = data.totalOutstandingAmount;
          this.claimManager.selectedClaim.numberOutstanding = data.totalRows;
          this.claimManager.selectedClaim.loadingOutstanding = false;
        }, () => {
          this.claimManager.selectedClaim.loadingOutstanding = false;
        });
    }
  }
  uncheckMain() {
    jQuery('input#selectAllOutstanding').attr({ 'checked': false });
  }
  select(p: any, $event, index) {
    p.selected = $event.target.checked;
    if (!$event.target.checked) {
      this.checkAll = false;
      this.uncheckMain();
    }
    if (this.selectMultiple) {
      for (let i = this.lastSelectedIndex; i < index; i++) {
        try {
          const p = jQuery('#row' + i).attr('prescription');
          const prescription = JSON.parse(p);
          const data = this.claimManager.selectedClaim.outstanding.find(pres =>
            pres.prescriptionId === prescription.prescriptionId);
          data.selected = true;
        } catch (e) { }
      }
    }
    this.lastSelectedIndex = index;
  }
  setSelected(p: any, s: Boolean) {
    p.selected = !p.selected;
    if (!p.selected) {
      this.checkAll = false;
      this.uncheckMain();
    }
  }
  selectAllCheckBox($event) {
    this.checkAll = $event.target.checked;
    if (this.checkAll) {
      this.claimManager.selectedClaim.outstanding.forEach(c => {
        c.selected = true;
      });
    } else {
      this.claimManager.selectedClaim.outstanding.forEach(c => {
        c.selected = false;
      });
      this.uncheckMain();
    }
  }
}
