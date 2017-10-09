import { Component, OnInit, HostListener } from '@angular/core';
import { HttpService } from "../../services/http-service"
import { EventsService } from "../../services/events-service"
import { ClaimManager } from "../../services/claim-manager";
import { PrescriptionNoteType } from "../../models/prescription-note-type";
import swal from "sweetalert2";
import { ClaimNote } from "../../models/claim-note"
import { Episode } from "../../models/episode"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
declare var $:any

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {

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

  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private events: EventsService,
    private toast: ToastsManager
  ) { }

  expand(expanded: Boolean, expandedBlade: Number, table: string) {
    this.expanded = expanded;
    this.expandedBlade = expandedBlade;
    this.initializeExpandedTableBooleanValue(table);

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
      let fixedBoxHeader = document.getElementById('pres-box-header');
      if(fixedBoxHeader){
        fixedBoxHeader.style.position = 'relative';
        fixedBoxHeader.style.width = '100%'; 
      }
    } else if (table === 'script-notes') {
      this.claimManager.isScriptNotesExpanded = !this.claimManager.isScriptNotesExpanded;
    } else if (table === 'payments') {
      this.claimManager.isPaymentsExpanded = !this.claimManager.isPaymentsExpanded;
    } else if (table === 'images') {
      this.claimManager.isImagesExpanded = !this.claimManager.isImagesExpanded;
    }
  }

  ngOnInit() {
    //$('body').addClass('sidebar-collapse');
    this.events.on("edit-episode", (id: Number,type:String) => {
      this.episode(id,type);
    })
    this.events.on("minimize", (...args) => {
      this.minimize(args[0]);
    })
    this.events.on("expand", (...args) => {
      this.expand(args[0], args[1], args[2]);
    })
  }

  addPrescriptionNote(text: String = "", TypeId?: String, prescriptionNoteId: any = null) {
    let selectedNotes = [];
    let prescriptionNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.PrescriptionNoteTypes.forEach((note: PrescriptionNoteType) => {
      prescriptionNoteTypeIds = prescriptionNoteTypeIds + '<option value="' + note.prescriptionNoteTypeId + '"' + (note.prescriptionNoteTypeId == TypeId ? "selected" : "") + '>' + note.typeName + '</option>';
    });
    var selectedPrecriptions = '';
    var checkboxes = $('.pescriptionCheck');
    for (var i = 0; i < checkboxes.length; i++) {
      if ($("#" + checkboxes[i].id).is(':checked')) {
        selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex;font-size:11pt;">' + $("#" + checkboxes[i].id).attr("labelName") + '</span> &nbsp; ';
        selectedNotes.push(Number(checkboxes[i].id));
      }
    }
    if (selectedNotes.length > 0) {
      var width = window.innerWidth * 1.799 / 3;
      swal({
        width: width + 'px',
        title: 'New Prescription Note',
        html:
        `
                  <div class="form-group">
                      <label id="claimNoteTypeLabel">Prescription Note type</label>
                      <select class="form-control" id="prescriptionNoteTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                        `+ prescriptionNoteTypeIds + `
                      </select>
                  </div>
                  <div class="form-group">
                      <label id="noteTextLabel">Note Text</label>
                      <textarea class="form-control"  id="noteText"  rows="5" cols="5" style="resize: vertical;">`+ text + `</textarea>
                  </div>
                  <div style="text-align:left">
                      <h4 class="text-green">Prescriptions</h4>
                      `+ selectedPrecriptions + `              
                  </div>
                  <div class="calendar">
                    <div class="row">
                      <div class="col-sm-8 col-sm-offset-2" style="padding-left:0px;padding-right:0px;">
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
                      <div class="col-sm-12">
                        <div class="form-group">
                            <label>&nbsp;</label><br/>
                          <button class="btn bg-primary btn-flat btn-small add-to-diary" type="button" style="color:white">Add to Diary</button>
                          <button class="btn btn-default btn-flat btn-small remove-from-diary" type="button">Remove from Diary</button>                      
                        </div>
                      </div>
                    </div>
                  </div>
            `,
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonText: "Save",
        customClass:'prescription-modal',
        preConfirm: function () {
          return new Promise(function (resolve) {
            resolve([
              $('#prescriptionNoteTypeId').val(),
              $('#noteText').val(),
              $('#datepicker').val(),
            ])
          })
        },
        onOpen: function () {
          $('#prescriptionNoteTypeId').focus()
        }
      }).then((result) => {
        if (result[0] == "") {
          this.toast.warning('Please select a note type in order to save your note.');
          setTimeout(() => {
            this.addPrescriptionNote(result[1], result[0]);
            $('#claimNoteTypeLabel').css({ "color": "red" })
          }, 200)
        } else if (result[1] == "") {
          this.toast.warning('A blank note cannot be saved.');
          setTimeout(() => {
            this.addPrescriptionNote(result[1], result[0]);
            $('#noteTextLabel').css({ "color": "red" })
          }, 200)
        } else {
          swal({ title: "", html: "Saving note... <br/> <img src='assets/1.gif'>", showConfirmButton: false })
          this.http.savePrescriptionNote(
            {
              claimId: this.claimManager.selectedClaim.claimId,
              noteText: result[1],
              prescriptionNoteTypeId: Number(result[0]),
              prescriptions: selectedNotes,
              prescriptionNoteId: prescriptionNoteId
            }).single().subscribe(res => {
              let result = res.json()
              swal.close();
              this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
              this.toast.success(result.message);
            }, error => {
              setTimeout(() => {
                this.addPrescriptionNote(result[1], result[0]);
                this.toast.error('A server error has occurred. Please contact your system administrator.');
              }, 200)
            })
        }
      }).catch(swal.noop);
      $('#datepicker').datepicker({
          autoclose: true
      });
      $("#datepicker").inputmask("mm/dd/yyyy", {"placeholder": "mm/dd/yyyy"});
      $("[inputs-mask]").inputmask();
      $("[data-mask]").inputmask();
      $(".add-to-diary").click(()=>{ 
        if (!$('#datepicker').val()) {
            this.toast.warning('Please add a Follow-up Date before adding to the Diary');
        }else if ($('#prescriptionNoteTypeId').val() == "") {
          this.toast.warning('Please select a note type in order to save your note.');
          setTimeout(() => {
            //this.addPrescriptionNote($('#noteText').val(),$('#prescriptionNoteTypeId').val());
            $('#claimNoteTypeLabel').css({ "color": "red" })
          }, 200)
        } else if ($('#noteText').val() == "") {
          this.toast.warning('A blank note cannot be saved.');
          setTimeout(() => {
            //this.addPrescriptionNote($('#noteText').val(), $('#prescriptionNoteTypeId').val());
            $('#noteTextLabel').css({ "color": "red" })
          }, 200)
        } else {
          swal.close();
          setTimeout(() => {
            swal({ title: "", html: "Saving note... <br/> <img src='assets/1.gif'>", showConfirmButton: false });
          }, 200)
          let followUpDate = $("#datepicker").val();
          this.http.savePrescriptionNote(
            {
              claimId: this.claimManager.selectedClaim.claimId,
              noteText: $('#noteText').val(),
              followUpDate:followUpDate,
              prescriptionNoteTypeId: Number($('#noteText').val()),
              prescriptions: selectedNotes,
              prescriptionNoteId: prescriptionNoteId
            }).single().subscribe(res => {
              let result = res.json()
              swal.close();
              this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
              this.toast.success(result.message);
            }, error => {
              setTimeout(() => {
                this.addPrescriptionNote($('#noteText').val(), $('#prescriptionNoteTypeId').val());
                this.toast.error('A server error has occurred. Please contact your system administrator.');
              }, 200)
            })
        }
      });
      $(".remove-from-diary").click(()=>{ 
         //console.log("Awaiting API to remove");
      });
    } else {
      this.claimManager.selectedClaim.prescriptions && this.claimManager.selectedClaim.prescriptions.length>0 ? 
        this.toast.warning('Please select at least one prescription.'):
        this.toast.warning('No prescriptions are present to save a prescription note.');
    }
  }

  savePrescriptionNote(result:Array<any>,followUpDate?:any){}

  episode(id?: Number, TypeId?: String) {
    var episode: Episode;

    // console.log(id);

    if (id) {
      episode = this.claimManager.selectedClaim.episodes.find(episode => episode.episodeId == id);
      // console.log('Episode ID: ', episode.episodeId);
      // console.log(episode);
    }

    let episodeTypeId = '<option value="" style="color:purple">Select Episode Type</option>';
    this.claimManager.EpisodeNoteTypes.forEach((note: { episodeTypeId: String, episodeTypeName: String }) => {
      episodeTypeId = episodeTypeId + '<option value="' + note.episodeTypeId + '"' + (note.episodeTypeId == TypeId || note.episodeTypeName == TypeId ? "selected" : "") + '>' + note.episodeTypeName + '</option>';
    });
    let note_text: String = '';
    if (episode) {
      note_text = episode.note ? episode.note : episode.noteText;
    }

    swal({
      width: window.innerWidth * 1.799 / 3,
      title: 'Episode Entry',
      html:
      `<div class="form-group">
              <label id="episodeNoteTypeLabel">Note type</label>
              <select class="form-control" id="episodeTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                `+ episodeTypeId + `
              </select>
              <p style="font-size:11pt" id="selected">Optional</p>
                  <label id="noteTextLabel">Episode Text</label>
                  <textarea class="form-control"  id="note" rows="5"  style="resize: vertical;">`+ note_text + `</textarea>
              </div>
            `,
      showCancelButton: true,
      showLoaderOnConfirm: true,
      confirmButtonText: "Save",
      preConfirm: function () {
        return new Promise(function (resolve) {
          resolve([
            $('#note').val(),
            $('#episodeTypeId').val()
          ])
        })
      },
      onOpen: function () {
        $('#note').focus()
      }
    }).then((result) => {
      if (result[0] == "") {
        this.toast.warning('A blank note cannot be saved.');
        setTimeout(() => {
          this.episode(result[0]);
          $('#noteTextLabel').css({ "color": "red" })
        }, 200)
      } else {
        swal({ title: "", html: "Saving episode... <br/> <img src='assets/1.gif'>", showConfirmButton: false });
        let TypeId = result[1];
        this.http.saveEpisode(
          {
            episodeId: episode !== undefined ? episode.episodeId : null, // only send on episode edit
            claimId: this.claimManager.selectedClaim.claimId,
            noteText: result[0],
            episodeTypeId: TypeId ? +TypeId : null,
            // by: episode !== undefined ? episode.by : 'me',
            // date: episode !== undefined ? episode.date : (new Date())
          }).single().subscribe(res => {
            let result = res.json()
            swal.close();
            this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
            this.toast.success(result.message);
          }, error => {
            setTimeout(() => {
              this.episode(id);
              this.toast.error('An internal system error has occurred. This will be investigated ASAP.');
            }, 200)
          })
      }
    }).catch(swal.noop)
  }


  addNote(noteText: String = "", TypeId?: String) {
    console.log(TypeId)
    let selectedNotes = [];
    noteText = noteText.replace(/\\n/g, '&#13;');
    let claimNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.NoteTypes.forEach((note: { key: String, value: String }) => {
      claimNoteTypeIds = claimNoteTypeIds + '<option value="' + note.key + '"' + (note.value == TypeId ? "selected" : "") + '>' + note.value + '</option>';
    });
    var width = window.innerWidth * 1.799 / 3;
    swal({
      title: 'Claim Note',
      width: width + 'px',
      html:
      `<div class="form-group" style="text-align:center">
              <label id="claimNoteTypeLabel">Note type</label>
              <select class="form-control" id="noteTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                `+ claimNoteTypeIds + `
              </select>
              <p style="font-size:11pt">Optional</p>
          </div>
          <div class="form-group">
              <label id="noteTextLabel">Note Text</label>
              <textarea class="form-control"  id="noteText" rows="5" cols="5"  style="resize: vertical;font-size:12pt;">`+ noteText + `</textarea>
          </div>
        `,
      showCancelButton: true,
      showLoaderOnConfirm: true,
      confirmButtonText: "Save",
      preConfirm: function () {
        return new Promise(function (resolve) {
          resolve([
            $('#noteTypeId').val(),
            $('#noteText').val()
          ])
        })
      },
      onOpen: function () {
        $('#noteTypeId').focus()
      }
    }).then((result) => {
      if (result[1] == "") {
        this.toast.warning('Note Text is required!');
        setTimeout(() => {
          this.addNote(result[1], result[0]);
          $('#noteTextLabel').css({ "color": "red" })
        }, 200)
      } else {
        swal({ title: "", html: "Saving note... <br/> <img src='assets/1.gif'>", showConfirmButton: false })

        console.log({
          claimId: this.claimManager.selectedClaim.claimId,
          noteTypeId: result[0] ? result[0] : null, // SIMILAR TO EPISODES TYPE
          noteText: txt
        });

        console.log(JSON.stringify({ text: result[1] }), { text: result[1] });
        var txt = JSON.stringify(result[1]);
        txt = txt.substring(1, txt.length - 1)
        this.http.saveClaimNote({
          claimId: this.claimManager.selectedClaim.claimId,
          noteTypeId: result[0] ? result[0] : null,
          noteText: txt
        }).subscribe(res => {
          let noteType = this.claimManager.NoteTypes.find(type => type.key == result[0]);
          if (!this.claimManager.selectedClaim.claimNote) {
            this.claimManager.selectedClaim.claimNote = new ClaimNote(txt, noteType ? noteType.value : undefined)
          } else {
            this.claimManager.selectedClaim.claimNote.noteText = txt;
            this.claimManager.selectedClaim.claimNote.noteType = noteType ? noteType.value : undefined;
          }
          this.claimManager.selectedClaim.editing = false;
          this.claimManager.loading = false;
          //console.log(res);
          swal.close();
          this.toast.success("Noted successfully saved");
        }, error => {
          let err = error.json();
          setTimeout(() => {
            this.addNote(result[1], result[0]);
            this.toast.warning(err.error_description);
          }, 200)
        })
      }
    }).catch(swal.noop)
  }

}
