import { Component, Inject, PLATFORM_ID, ViewChild, OnInit, HostListener, AfterViewInit } from '@angular/core';
import { HttpService } from "../../services/http-service";
import { EventsService } from "../../services/events-service";
import { ClaimManager } from "../../services/claim-manager";
import { PrescriptionNoteType } from "../../models/prescription-note-type";
import swal from "sweetalert2";
import { ClaimNote } from "../../models/claim-note";
import { Episode } from "../../interfaces/episode"
import { ToastsManager } from 'ng2-toastr';
import { Router } from "@angular/router";
import { DatePipe } from '@angular/common';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
import { AccountReceivableService } from '../../services/services.barrel';
import { UUID } from 'angular2-uuid';
import { DialogService } from "ng2-bootstrap-modal";
import { ConfirmComponent } from '../../components/confirm.component';
import { isPlatformBrowser } from '@angular/common';
import { Prescription } from '../../models/prescription';
//import { SnotifyService } from 'ng-snotify';

declare var $: any;

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit, AfterViewInit {

  @ViewChild('episodeSwal') private episodeSwal: SwalComponent;
  @ViewChild('prescriptionStatusSwal') private prescriptionStatusSwal: SwalComponent;

  @HostListener("window:scroll", [])
  onWindowScroll() {
    if (window.pageYOffset > 200 && this.claimManager.isPrescriptionsExpanded) {
      let fixedBoxHeader = document.getElementById('pres-box-header');
      let box = document.getElementById('pres-box');
      if (fixedBoxHeader && box) {
        let boxWidth = box.clientWidth.toString();
        fixedBoxHeader.style.width = boxWidth + 'px';
        fixedBoxHeader.style.position = 'fixed';
        fixedBoxHeader.style.top = '0';
      }
    } else if (window.pageYOffset < 200 && this.claimManager.isPrescriptionsExpanded) {
      let fixedBoxHeader = document.getElementById('pres-box-header');
      let box = document.getElementById('pres-box');
      if (fixedBoxHeader && box) {
        fixedBoxHeader.style.position = 'relative';
      }
    }
  }
  expanded: Boolean = false
  expandedBlade: Number = 0;
  over: boolean[];
  statusId: any
  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      $(".sticky-claim").sticky({ topSpacing: 53 });
    }
  }
  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private router: Router,
    //private snotifyService: SnotifyService,
    public readonly swalTargets: SwalPartialTargets,
    public claimManager: ClaimManager,
    private dialogService: DialogService,
    private http: HttpService,
    private dp: DatePipe,
    private events: EventsService,
    private toast: ToastsManager,
    private ar: AccountReceivableService,
  ) {
    this.over = new Array(5);
    this.over.fill(false);
  }

  expand(expanded: Boolean, expandedBlade: Number, table: string) {
    this.expanded = expanded;
    this.expandedBlade = expandedBlade;
    this.initializeExpandedTableBooleanValue(table);
  }
  get isVip() {
    return this.claimManager.selectedClaim && this.claimManager.selectedClaim.isVip;
  }
  isMaxBalance($event) {
    this.claimManager.loading = true;
    this.http.setMaxBalance(this.claimManager.selectedClaim.claimId, $event.target.checked).subscribe(r => {
      this.toast.success(r.message, null, { showCloseButton: true, toastLife: 8000 });
      /* this.snotifyService.success(r.message, {
        timeout: 5500,
        showProgressBar: true,
        closeOnClick: false,
        pauseOnHover: true,
        buttons: [
          {
            text: 'UNDO', action: () => {

            }, bold: false
          },
          { text: 'Close', action: (toast) => { this.snotifyService.remove(toast.id); }, bold: true },
        ]
      }); */
      this.claimManager.loading = false;
    }, err => {
      const result = err.error
      this.toast.error(result.Message);
      this.claimManager.loading = false;
    });
  }
  deleteNote() {
    if (this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimId) {
      this.dialogService.addDialog(ConfirmComponent, {
        title: "Delete Claim Note",
        message: "Are you sure you wish to remove this note?"
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.claimManager.loading = true;
            this.http.deleteClaimNote({ claimId: this.claimManager.selectedClaim.claimId }).subscribe(r => {
              this.toast.success(r.message);
              this.claimManager.selectedClaim.claimNote = undefined;
              this.claimManager.loading = false;
            }, err => {
              let result = err.error;
              this.toast.error(result.Message);
              this.claimManager.loading = false;
            })
          }
        })
    }
  }
  minimize(table: string) {
    this.expanded = false;
    this.expandedBlade = 0;
    this.initializeExpandedTableBooleanValue(table);
  }

  public initializeExpandedTableBooleanValue(table: string) {
    if (table === 'claims') {
      this.claimManager.isClaimsExpanded = !this.claimManager.isClaimsExpanded;
    } else if (table === 'notes') {
      this.claimManager.isNotesExpanded = !this.claimManager.isNotesExpanded;
    } else if (table === 'episodes') {
      this.claimManager.isEpisodesExpanded = !this.claimManager.isEpisodesExpanded;
    } else if (table === 'prescriptions') {
      this.claimManager.isPrescriptionsExpanded = !this.claimManager.isPrescriptionsExpanded;
      const fixedBoxHeader = document.getElementById('pres-box-header');
      if (fixedBoxHeader) {
        fixedBoxHeader.style.position = 'relative';
        fixedBoxHeader.style.width = '100%';
      }
    } else if (table === 'script-notes') {
      this.claimManager.isScriptNotesExpanded = !this.claimManager.isScriptNotesExpanded;
    } else if (table === 'payments') {
      this.claimManager.isPaymentsExpanded = !this.claimManager.isPaymentsExpanded;
    } else if (table === 'outstanding') {
      this.claimManager.isOutstandingExpanded = !this.claimManager.isOutstandingExpanded;
    } else if (table === 'images') {
      this.claimManager.isImagesExpanded = !this.claimManager.isImagesExpanded;
    }
  }

  ngOnInit() {
    // $('body').addClass('sidebar-collapse');
    this.events.on("edit-episode", (episode: Episode) => {
      this.episode(episode.episodeId, episode.type, (episode.episodeNote || episode['note']));
    });
    this.events.on("minimize", (...args) => {
      this.minimize(args[0]);
    });
    this.events.on('expand', (...args) => {
      this.expand(args[0], args[1], args[2]);
    })
    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['claimId']) {
        this.claimManager.search({ claimId: params['claimId'] });
      } else if (params['claimNumber']) {
        this.claimManager.search({ claimNumber: params['claimNumber'] });
      }
    });
  }

  saveStatus(data) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Change multiple prescription statuses',
      message: `Are you sure you wish to change prescription statuses for ${data.prescriptionIds.length} prescription${data.prescriptionIds.length > 1 ? 's' : ''} to ${this.statusId.statusName}?`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true;
          this.http.updateMultiplePrescriptionStatus(data).subscribe(r => {
            this.toast.success(r.message);
            $('input#selectAllCheckBox').attr({ 'checked': false })
            const selected = this.claimManager.selectedClaim.prescriptions.filter(pres => pres.selected == true);
            selected.forEach(s => {
              s.selected = false;
            })
            this.events.broadcast('reload:prespcriptions', 1);
            this.claimManager.loading = false;
          }, err => {
            const result = err.error
            this.toast.error(result.Message);
            this.claimManager.loading = false;
          })
        }
      })
  }
  updateStatus() {
    const checkboxes = $('.pescriptionCheck');
    const selectedNotes = [];
    for (let i = 0; i < checkboxes.length; i++) {
      if ($('#' + checkboxes[i].id).is(':checked')) {
        selectedNotes.push(Number(checkboxes[i].id));
      }
    }
    if (selectedNotes.length > 0) {
      this.prescriptionStatusSwal.show().then((r) => {
        if (!r.dismiss) {
          if (!this.statusId || !this.statusId.prescriptionStatusId) {
            this.toast.warning('Please select one status to from the dropdown list.');
            setTimeout(() => { this.updateStatus(); });
          } else {
            this.saveStatus({ prescriptionStatusId: this.statusId.prescriptionStatusId, prescriptionIds: selectedNotes })
          }
        }
      })
    } else {
      this.claimManager.selectedClaim.prescriptions && this.claimManager.selectedClaim.prescriptions.length > 0 ?
        this.toast.warning('Please select at least one prescription to change status.') :
        this.toast.warning('No prescriptions are present to change status.');
    }

  }
  addPrescriptionNote(type: string='prescriptions', text: String = '', TypeId?: String, prescriptionNoteId: any = null) {
    const selectedNotes = [];
    let prescriptionNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.PrescriptionNoteTypes.forEach((note: PrescriptionNoteType) => {
      prescriptionNoteTypeIds = prescriptionNoteTypeIds + '<option value="' + note.prescriptionNoteTypeId + '"' + (note.prescriptionNoteTypeId == TypeId ? 'selected' : '') + '>' + note.typeName + '</option>';
    });
    let selectedPrecriptions = '';
    let prescriptions = type=='outstanding'? this.claimManager.selectedClaim.outstanding:this.claimManager.selectedClaim.prescriptions;
    prescriptions.forEach(c => {
      if (c.selected) {
        selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex;font-size:11pt;">' + c.labelName + '</span> &nbsp; ';
        selectedNotes.push(Number(c.prescriptionId));
      }
    });
    if (selectedNotes.length > 0) {
      const width = window.innerWidth * 1.799 / 3;
      swal({
        width: width + 'px',
        title: 'New Prescription Note',
        html:
          `
                  <div class="form-group">
                      <label id="claimNoteTypeLabel">Prescription Note type</label>
                      <select class="form-control" id="prescriptionNoteTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                        ${prescriptionNoteTypeIds}
                      </select>
                  </div>
                  <div class="form-group">
                      <label id="noteTextLabel">Note Text</label>
                      <textarea class="form-control"  id="noteText"  rows="5" cols="5" style="resize: vertical;">`+ text + `</textarea>
                  </div>
                  <div class="row">
                    <div class="col-sm-6 text-left">
                          <h4 class="text-green">Prescriptions</h4>
                          ${selectedPrecriptions}
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label>Follow-up Date</label>
                        <div class="input-group date">
                          <div class="input-group-addon">
                              <i class="fa fa-calendar"></i>
                          </div>
                          <input class="form-control pull-right"  type="text" id="datepicker" name="rxDate" inputs-inputmask="'alias': 'mm/dd/yyyy'" inputs-mask focus-on>                  
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="row">
                        <div class="col-sm-6 text-left">
                            <button class="btn btn-flat btn-primary save-prescription-note" type="button" style="color:white;background-color: rgb(48, 133, 214);">Save</button>                          
                            <button class="btn btn-flat btn-default cancel-prescription-note" type="button" style="color:white;background-color: rgb(170, 170, 170);">Cancel</button>                          
                        </div>
                        <div class="col-sm-6">
                          <div class="form-group">
                            <button class="btn bg-primary btn-flat pull-right btn-md add-to-diary" type="button" style="color:white">Add to Diary</button>                          
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
            `,
        showConfirmButton: false,
        showCancelButton: false,
        showLoaderOnConfirm: true,
        customClass: 'prescription-modal',
        onOpen: function () {
          $('#prescriptionNoteTypeId').focus();
        }
      }).catch(swal.noop);
      $('#datepicker').datepicker({
        autoclose: true
      });
      $('#datepicker').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
      $('[inputs-mask]').inputmask();
      $('[data-mask]').inputmask();
      $('#prescriptionNoteTypeId').change(() => { 
        if ($('#prescriptionNoteTypeId').val()) {
          setTimeout(() => {$('#claimNoteTypeLabel').css({ 'color': '#545454' })},150);
        }
      });
      $('#noteText').change(e => {
        if ($('#noteText').val()) {
          setTimeout(() => {$('#noteTextLabel').css({ 'color': '#545454' });},150);
        }
      });
      $('.add-to-diary').click(() => {
        if (!$('#datepicker').val()) {
          this.toast.warning('Please add a Follow-up Date before adding to the Diary');
        } else if ($('#prescriptionNoteTypeId').val() === '') {
          this.toast.warning('Please select a note type in order to save your note.');
          setTimeout(() => {
            $('#claimNoteTypeLabel').css({ 'color': 'red' });
          }, 200);
        } else if ($('#noteText').val() === '') {
          this.toast.warning('A blank note cannot be saved.');
          setTimeout(() => {
            $('#noteTextLabel').css({ 'color': 'red' });
          }, 200);
        } else {
          swal.close();
          setTimeout(() => {
            swal({ title: '', html: 'Adding note to Diary... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(swal.noop);
          }, 200);
          // let followUpDate = $("#datepicker").val();
          const followUpDate = this.dp.transform($('#datepicker').val(), 'MM/dd/yyyy');
          this.http.savePrescriptionNote(
            {
              claimId: this.claimManager.selectedClaim.claimId,
              noteText: $('#noteText').val(),
              followUpDate: followUpDate,
              prescriptionNoteTypeId: Number($('#prescriptionNoteTypeId').val()),
              prescriptions: selectedNotes,
              prescriptionNoteId: prescriptionNoteId
            }).single().subscribe(res => {
              const result = res;
              prescriptions.forEach(c => {
                if (c.selected) {
                  c.noteCount = (c.noteCount || 0) + 1;
                }
                c.selected = false;
              });
              swal.close();
              this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
              this.toast.success(result.message);
            }, () => {
              setTimeout(() => {
                this.addPrescriptionNote(type,$('#noteText').val(), $('#prescriptionNoteTypeId').val());
                this.toast.error('A server error has occurred. Please contact your system administrator.');
              }, 200);
            });
        }
      });
      $('.remove-from-diary').click(() => {
        // log("Awaiting API to remove");
      });
      $('button.save-prescription-note').click(() => {
        let result = [
          $('#prescriptionNoteTypeId').val(),
          $('#noteText').val(),
          $('#datepicker').val(),
        ];
        if (result[0] === '') {
          this.toast.warning('Please select a note type in order to save your note.');
          setTimeout(() => {
            $('#claimNoteTypeLabel').css({ 'color': 'red' });
          }, 200);
        } else if (result[1] === '') {
          this.toast.warning('A blank note cannot be saved.');
          setTimeout(() => {
            $('#noteTextLabel').css({ 'color': 'red' });
          }, 200);
        } else {
          swal({ title: '', html: 'Saving note... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(swal.noop);
          this.http.savePrescriptionNote(
            {
              claimId: this.claimManager.selectedClaim.claimId,
              noteText: result[1],
              prescriptionNoteTypeId: Number(result[0]),
              prescriptions: selectedNotes,
              prescriptionNoteId: prescriptionNoteId
            }).single().subscribe(res => {
              const result = res;
              prescriptions.forEach(c => {
                if (c.selected) {
                  c.noteCount = (c.noteCount || 0) + 1;
                }
                c.selected = false;
              });
              swal.close();
              this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
              this.toast.success(result.message);
            }, () => {
              setTimeout(() => {
                this.addPrescriptionNote(type, result[1], result[0]);
                this.toast.error('A server error has occurred. Please contact your system administrator.');
              }, 200);
            });
        }
      });
      $('button.cancel-prescription-note').click(() => {
        swal.clickCancel();
      });
    } else {
      this.claimManager.selectedClaim.prescriptions && this.claimManager.selectedClaim.prescriptions.length > 0 ?
        this.toast.warning('Please select at least one prescription.') :
        this.toast.warning('No prescriptions are present to save a prescription note.');
    }
  }


  episode(id: number = undefined, TypeId: string = '1', note: string = null) {

    this.claimManager.episodeForm.reset();
    this.claimManager.episodeForm.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      // episodeId: id, // only send on episode edit
      episodeText: note,
      pharmacyNabp: null,
      episodeTypeId: TypeId
    });
    this.episodeSwal.show().then((r) => {

    });
  }

  exportLetter(type) {
    if (!this.claimManager.selectedClaim) {
      this.toast.warning('No claim loaded!');
    } else {
      const prescriptions = this.claimManager.selectedClaim.prescriptions.filter(p => p.selected === true);
      if (prescriptions.length === 0) {
        this.toast.warning('Please select one prescription before generating a letter.', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else if (prescriptions.length > 1) {
        this.toast.warning('Please select only one prescription before generating a letter.', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else {
        this.claimManager.loading = true;
        this.http.exportLetter({ claimId: this.claimManager.selectedClaim.claimId, type: type, prescriptionId: prescriptions[0].prescriptionId })
          .subscribe((result) => {
            this.claimManager.loading = false;
            this.ar.downloadFile(result);
          }, err => { 
            this.toast.error(err.statusText);
            this.claimManager.loading = false;
            try {
              const error = err.error;
            } catch (e) { }
          });
      }
    }
  }



  exportDenial(type) {
    if (!this.claimManager.selectedClaim) {
      this.toast.warning('No claim loaded!');
    } else {
      const prescriptions = this.claimManager.selectedClaim.prescriptions.filter(p => p.selected === true);
      if (prescriptions.length === 0) {
        this.toast.warning('Please select one prescription before generating a letter.', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else if (prescriptions.length > 1) {
        this.toast.warning('Please select only one prescription before generating a letter.', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else {
        this.claimManager.loading = true;
        this.http.exportLetter({ claimId: this.claimManager.selectedClaim.claimId, type: type, prescriptionId: prescriptions[0].prescriptionId })
          .subscribe((result) => {
            this.claimManager.loading = false;
            this.ar.downloadFile(result);
          }, err => {
            this.toast.error(err.statusText);
            this.claimManager.loading = false;
            try {
              const error = err.error;
            } catch (e) { }
          });
      }
    }
  }



  viewFile(type) {
    if (!this.claimManager.selectedClaim) {
      this.toast.warning('No claim loaded!');
    } else {
      const prescriptions = this.claimManager.selectedClaim.prescriptions.filter(p => p.selected === true);
      const unIndexed = prescriptions.filter(p => p.invoiceIsIndexed === false);
      const prescriptionId: Array<any> = [];
      for (let i = 0; i < prescriptions.length; i++) {
        prescriptionId.push(prescriptions[i].prescriptionId);
      }
      if (prescriptions.length === 0) {
        this.toast.warning('Please select one prescription to view invoice.', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else if (prescriptions.length > 1 && unIndexed.length > 0) {
        this.toast.warning('All Prescriptions selected must have Indexed Invoices in order to view them', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else if (prescriptions.length === 1 && !prescriptions[0].invoiceUrl) {
        this.toast.warning('Invoice file not found in selected prescription', null,
          { toastLife: 10000, showCloseButton: true }).then((toast: any) => null);
      } else {
        // https://bridgeportclaims-images.azurewebsites.net/11-17/20171124/csp201711245300.pdf used for testing
        const id = UUID.UUID();
        const doc: any = { fileUrl: prescriptions[0].invoiceUrl, fileName: prescriptions[0].fileName };
        if (prescriptions.length > 1) {
          doc.prescriptionIds = prescriptionId;
        }
        doc.documentId = id;
        const file = doc as any;
        localStorage.setItem('file-' + id, JSON.stringify(file));
        window.open('#/main/indexing/indexed-image/' + id, '_blank');
      }
    }
  }

  exportPDF() {
    if (!this.claimManager.selectedClaim) {
      this.toast.warning('No claim loaded!');
    } else {
      this.claimManager.loading = true;
      this.http.exportPrescriptions({ claimId: this.claimManager.selectedClaim.claimId })
        .subscribe((result) => {
          this.claimManager.loading = false;
          this.ar.downloadFile(result);
        }, err => { 
          this.claimManager.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        });
    }
  }
  exportBilling() {
    if (!this.claimManager.selectedClaim) {
      this.toast.warning('No claim loaded!');
    } else {
      this.claimManager.loading = true;
      this.http.exportBillingStatement({ claimId: this.claimManager.selectedClaim.claimId })
        .subscribe((result) => {
          this.claimManager.loading = false;
          this.ar.downloadFile(result);
        }, err => { 
          this.claimManager.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        });
    }
  }

  addNote(noteText: String = '', TypeId?: String) {
    const selectedNotes = [];
    noteText = noteText.replace(/\\n/g, '&#13;');
    let claimNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.NoteTypes.forEach((note: { key: String, value: String }) => {
      claimNoteTypeIds = claimNoteTypeIds + '<option value="' + note.key + '"' + (note.value === TypeId ? 'selected' : '') + '>' + note.value + '</option>';
    });
    const width = window.innerWidth * 1.799 / 3;
    swal({
      title: 'Claim Note',
      width: width + 'px',
      html:
        `<div class="form-group" style="text-align:center">
              <label id="claimNoteTypeLabel">Note type</label>
              <select class="form-control" id="noteTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                ` + claimNoteTypeIds + `
              </select>
              <p style="font-size:11pt">Optional</p>
          </div>
          <div class="form-group">
              <label id="noteTextLabel">Note Text</label>
              <textarea class="form-control"  id="noteText" rows="5" cols="5"  style="resize: vertical;font-size:12pt;">` + noteText + `</textarea>
          </div>
        `,
      showCancelButton: true,
      showLoaderOnConfirm: true,
      confirmButtonText: 'Save',
      preConfirm: function () {
        return new Promise(function (resolve) {
          resolve([
            $('#noteTypeId').val(),
            $('#noteText').val()
          ]);
        });
      },
      onOpen: function () {
        $('#noteTypeId').focus();
      }
    }).then((results) => {
      if (!results.dismiss) {
        const result = results.value;
        if (result[1] === '') {
          this.toast.warning('Note Text is required!');
          setTimeout(() => {
            this.addNote(result[1], result[0]);
            $('#noteTextLabel').css({ 'color': 'red' });
          }, 200);
        } else {
          swal({ title: '', html: 'Saving note... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(swal.noop)
            .catch(swal.noop);
          let txt = JSON.stringify(result[1]);
          txt = txt.substring(1, txt.length - 1);
          this.http.saveClaimNote({
            claimId: this.claimManager.selectedClaim.claimId,
            noteTypeId: result[0] ? result[0] : null,
            noteText: txt
          }).subscribe(res => {
            const noteType = this.claimManager.NoteTypes.find(type => type.key === result[0]);
            if (!this.claimManager.selectedClaim.claimNote) {
              this.claimManager.selectedClaim.claimNote = new ClaimNote(txt, noteType ? noteType.value : undefined);
            } else {
              this.claimManager.selectedClaim.claimNote.noteText = txt;
              this.claimManager.selectedClaim.claimNote.noteType = noteType ? noteType.value : undefined;
            }
            this.claimManager.selectedClaim.editing = false;
            this.claimManager.loading = false; 
            swal.close();
            this.toast.success('Noted successfully saved');
          }, error => {
            const err = error.error;
            setTimeout(() => {
              this.addNote(result[1], result[0]);
              this.toast.error(err.Message);
            }, 200);
          });
        }
      }
    }).catch(swal.noop);
  }

}
