import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {ClaimManager} from "../../services/claim-manager";
import {PrescriptionNoteType} from "../../models/prescription-note-type";
import swal from "sweetalert2";
import {ClaimNote} from "../../models/claim-note"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {
  
  expanded:Boolean=false
  expandedBlade:Number=0;
  
  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private toast: ToastsManager
  ) {
     
  }

  expand(expanded:Boolean,expandedBlade:Number){
    this.expanded = expanded;
    this.expandedBlade = expandedBlade;
  }
  minimize(){
    this.expanded = false;
    this.expandedBlade = 0;
  }
  ngOnInit() {
     window['jQuery']('body').addClass('sidebar-collapse');
  }

  addPrescriptionNote(text:String="",TypeId?:String,prescriptionNoteId:any=null){
    let selectedNotes = [];
    let prescriptionNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
      this.claimManager.PrescriptionNoteTypes.forEach((note:PrescriptionNoteType)=>{
          prescriptionNoteTypeIds=prescriptionNoteTypeIds+'<option value="'+note.prescriptionNoteTypeId+'"' +(note.prescriptionNoteTypeId ==TypeId ? "selected": "")+'>'+note.typeName+'</option>';          
      });
     var selectedPrecriptions = '';
     var checkboxes = window['jQuery']('.pescriptionCheck');
     for(var i=0;i<checkboxes.length;i++){
         if(window['jQuery']("#"+checkboxes[i].id).is(':checked')){
            selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex">'+window['jQuery']("#"+checkboxes[i].id).attr("labelName")+'</span> &nbsp; ';
            selectedNotes.push(Number(checkboxes[i].id));
        }
     }
     if(selectedNotes.length>0){
        swal({
          title: 'New Prescription Note',
          html:
            `
                  <div class="form-group">
                      <label id="claimNoteTypeLabel">Prescription Note type</label>
                      <select class="form-control" id="prescriptionNoteTypeId">
                        `+prescriptionNoteTypeIds+`
                      </select>
                  </div>
                  <div class="form-group">
                      <label id="noteTextLabel">Note Text</label>
                      <textarea class="form-control"  id="noteText" rows="3">`+text+`</textarea>
                  </div>
                  <div style="text-align:left">
                      <h4 class="text-green">Prescriptions</h4>
                      `+selectedPrecriptions+`              
                  </div>
            `,
          showCancelButton: true,
          showLoaderOnConfirm:true,
          confirmButtonText:"Save",
          preConfirm: function () {
            return new Promise(function (resolve) {
              resolve([
                window['jQuery']('#prescriptionNoteTypeId').val(),
                window['jQuery']('#noteText').val()
              ])
            })
          },
          onOpen: function () {
            window['jQuery']('#prescriptionNoteTypeId').focus()
          }
        }).then( (result)=> {
          if(result[0]==""){
              this.toast.warning('Please select one type!');
              setTimeout(()=>{
                  this.addPrescriptionNote(result[1],result[0]);
                  window['jQuery']('#claimNoteTypeLabel').css({"color":"red"})
                },200)
          }else if(result[1]==""){
              this.toast.warning('Note Text is required!');
              setTimeout(()=>{
                this.addPrescriptionNote(result[1],result[0]);
                window['jQuery']('#noteTextLabel').css({"color":"red"})
              },200)
          }else{
              swal({title: "", html: "Saving note... <br/> <i class='fa fa-refresh fa-2x fa-spin'></i>", showConfirmButton: false})
              this.http.savePrescriptionNote(
                {
                  claimId: this.claimManager.selectedClaim.claimId,
                  noteText: result[1],
                  prescriptionNoteTypeId: Number(result[0]),
                  prescriptions: selectedNotes,
                  prescriptionNoteId: prescriptionNoteId
                }).single().subscribe(res=>{
                  let result = res.json()
                  swal.close();
                  this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
                  this.toast.success(result.message);
                },error=>{
                  setTimeout(()=>{
                    this.addPrescriptionNote(result[1],result[0]);
                    this.toast.warning('Server error!');
                  },200)
                })
          } 
        }).catch(swal.noop)
     }else{
        this.toast.warning('Please select at least one prescription');
     }
  }

  addNote(noteText:String="",TypeId?:String){
    let selectedNotes = [];
    let claimNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.NoteTypes.forEach((note:{key:String,value:String})=>{
        claimNoteTypeIds=claimNoteTypeIds+'<option value="'+note.key+'"' +(note.value ==TypeId ? "selected": "")+'>'+note.value+'</option>';          
    });     
    swal({
      title: 'Claim Note',
      html:
        `
              <div class="form-group">
                  <label id="claimNoteTypeLabel">Note type</label>
                  <select class="form-control" id="noteTypeId">
                    `+claimNoteTypeIds+`
                  </select>
              </div>
              <div class="form-group">
                  <label id="noteTextLabel">Note Text</label>
                  <textarea class="form-control"  id="noteText" rows="3">`+noteText+`</textarea>
              </div>
        `,
      showCancelButton: true,
      showLoaderOnConfirm:true,
      confirmButtonText:"Save",
      preConfirm: function () {
        return new Promise(function (resolve) {
          resolve([
            window['jQuery']('#noteTypeId').val(),
            window['jQuery']('#noteText').val()
          ])
        })
      },
      onOpen: function () {
        window['jQuery']('#noteTypeId').focus()
      }
    }).then( (result)=> {
      if(result[0]==""){
          this.toast.warning('Please select one type!');
          setTimeout(()=>{
              this.addNote(result[1],result[0]);
              window['jQuery']('#claimNoteTypeLabel').css({"color":"red"})
            },200)
      }else if(result[1]==""){
          this.toast.warning('Note Text is required!');
          setTimeout(()=>{
            this.addNote(result[1],result[0]);
            window['jQuery']('#noteTextLabel').css({"color":"red"})
          },200)
      }else{
          swal({title:"",html:"Saving note... <br/> <i class='fa fa-refresh fa-2x fa-spin'></i>",showConfirmButton:false})
          this.http.saveClaimNote({
              claimId: this.claimManager.selectedClaim.claimId,
              noteTypeId:result[0],
              noteText:result[1]
          }).subscribe(res => {
            if(!this.claimManager.selectedClaim.claimNote){
              this.claimManager.selectedClaim.claimNote = new ClaimNote(result[1],result[0])
            }else{
              this.claimManager.selectedClaim.claimNote.noteText=result[1];
            } 
            this.claimManager.selectedClaim.editing = false;
            this.claimManager.loading = false;              
              //console.log(res);
              swal.close();
              this.toast.success("Noted successfully saved");
            },error=>{
              let err = error.json();
              setTimeout(()=>{
                this.addNote(result[1],result[0]);
                this.toast.warning(err.error_description);
              },200)
            })
      } 
    }).catch(swal.noop)
  }

}
