import { Component, OnInit, Renderer2, AfterViewInit, NgZone, HostListener, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import { HttpService } from "../../services/http-service";
import { EventsService } from "../../services/events-service";
import { Prescription } from "../../models/prescription";
import { PrescriptionNote } from "../../models/prescription-note";
import swal from "sweetalert2";
import { DatePipe } from '@angular/common';
import { SortColumnInfo } from "../../directives/table-sort.directive";
declare var jQuery: any;

@Component({
  selector: 'app-claim-prescriptions',
  templateUrl: './claim-prescriptions.component.html',
  styleUrls: ['./claim-prescriptions.component.css']
})
export class ClaimPrescriptionsComponent implements OnInit, AfterViewChecked, AfterViewInit {

  checkAll: Boolean = false;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;
  @ViewChild('prescriptionTable') table: ElementRef;
  sortColumn: SortColumnInfo;

  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    public claimManager: ClaimManager,
    private events: EventsService,
    private http: HttpService
  ) { }

  ngOnInit() {
    this.events.on("claim-updated", () => {
      setTimeout(() => {
        window['jQuery']('input[type="checkbox"].flat-red, input[type="radio"].flat-red')
          .iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
          });
      }, 1000);
    })
    this.cloneTableHeading();
    /* this.claimManager.onClaimIdChanged.subscribe(() => {
      this.fetchData();
    }); */
  }

  ngAfterViewInit() {
    this.rd.listen(this.table.nativeElement, 'keydown', ($event) => {
      if ($event.keyCode == 16) {
        this.selectMultiple = true;
      }
    })
    this.rd.listen(this.table.nativeElement, 'keyup', ($event) => {
      if ($event.keyCode == 16) {
        this.selectMultiple = false;
      }
    })

  }
  log(y) {
    console.log(y);
  }
  activateClaimCheckBoxes() {
    jQuery('#selectAllCheckBox').click();
  }

  ngAfterViewChecked() {
 
  }

  cloneTableHeading() {
    
  }

  cloneBoxHeader() {
    let cln = document.getElementById
  }

  updateTableHeadingWidth() {
    setTimeout(() => {
      let fixedHeader = document.getElementById('fixed-header');
      let fixedMaxHeader = document.getElementById('fixed-max-header');
      let mainTable = document.getElementById('maintable');
      if (fixedHeader) {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          fixedHeader.style.width = tableWidth + 'px';
        }
      } else {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          try {
            fixedMaxHeader.style.width = tableWidth + 'px';
          } catch (e) { }
        }
      }
    }, 500)
  }
  clicked() {

  }

  uncheckMain() {
    jQuery("input#selectAllCheckBox").attr({ "checked": false })
  }
  select(p: any, $event, index) {
    p.selected = $event.target.checked;
    if (!$event.target.checked) {
      this.checkAll = false;
      this.uncheckMain();
    }
    if (this.selectMultiple) {
      for (var i = this.lastSelectedIndex; i < index; i++) {
        try {
          let p = jQuery('#row' + i).attr('prescription');
          let prescription = JSON.parse(p);
          let data = this.claimManager.selectedClaim.prescriptions.find(pres => pres.prescriptionId == prescription.prescriptionId);// .get(prescription.prescriptionId);
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
      this.claimManager.selectedClaim.prescriptions.forEach(c => {
        c.selected = true;        
      })
    } else {
      this.claimManager.selectedClaim.prescriptions.forEach(c => {
        c.selected = false;
      });
      this.uncheckMain();
    }
  }

  showNotes(prescriptionId: Number) {
    this.claimManager.loading = true;
    this.http.getPrescriptionNotes(prescriptionId).single().subscribe(res => {
      let notes: Array<PrescriptionNote> = res.json();
      this.displayNotes(notes);
    }, error => {
      this.claimManager.loading = false;
    });
  }

  displayNotes(notes: Array<PrescriptionNote>) {

    let notesHTML = '';
    notes.forEach(note => {

      let noteDate = this.dp.transform(note.rxDate, "shortDate");
      notesHTML = notesHTML + `
            <tr>
              <td>`+ noteDate + `</td>
              <td>`+ note.type + `</td>
              <td>`+ note.enteredBy + `</td>
              <td style="white-space: pre-wrap;">`+ note.note + `</td>               
            </tr>`;
    })
    let html = `<div class="row invoice-info">
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
                    `+ notesHTML + `
                    </tbody>
                  </table>
                </div>
              </div>
        </div>`;
    this.claimManager.loading = false;
    swal({
      title: 'Claim Note',
      width: window.innerWidth * 3 / 4 + "px",
      html: html
    }).then(success => {

    }).catch(swal.noop)
  }

  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  fetchData() {
  this.claimManager.loadingPrescription = true;
    let page = 1;
    let page_size = 1000;
    let sort: string = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc';
    if (this.sortColumn) {
      sort = this.sortColumn.column;
      sort_dir = this.sortColumn.dir;
    }
    this.http.getPrescriptions(this.claimManager.selectedClaim.claimId, sort, sort_dir,
      page, page_size).map(p => p.json())
      .subscribe(results => {      
        this.claimManager.selectedClaim.setPrescription(results);
        this.claimManager.loadingPrescription = false;
      });
  }

}
