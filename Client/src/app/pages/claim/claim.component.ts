import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {EventsService} from "../../services/events-service"
import {ClaimManager} from "../../services/claim-manager";
import {PrescriptionNoteType} from "../../models/prescription-note-type";
import swal from "sweetalert2";
import {ClaimNote} from "../../models/claim-note"
import {Episode} from "../../models/episode"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {
  
  expanded:Boolean=false
  expandedBlade:Number=0;
  
  constructor(public claimManager:ClaimManager,private http:HttpService,private events:EventsService,
    private toast: ToastsManager) {
     
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
     this.events.on("edit-episode",(id:Number)=>{
       this.episode(id);
     })
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
            selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex;font-size:11pt;">'+window['jQuery']("#"+checkboxes[i].id).attr("labelName")+'</span> &nbsp; ';
            selectedNotes.push(Number(checkboxes[i].id));
        }
     }
     if(selectedNotes.length>0){
        var width   = window.innerWidth*1.799/3;
        swal({
          width: width+'px',
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
                      <textarea class="form-control"  id="noteText"  rows="5" cols="5" style="resize: vertical;">`+text+`</textarea>
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
              this.toast.warning('Please select a Note Type in order to Save your Note!');
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
              swal({title: "", html: "Saving note... <br/> <img src='assets/1.gif'>", showConfirmButton: false})              
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
                    this.toast.error('An internal system error has occurred. This will be investigated ASAP.');
                  },200)
                })
          } 
        }).catch(swal.noop)
     }else{
        this.toast.warning('Please select at least one prescription');
     }
  }

  episode(id?:Number){
    var episode:Episode;
      if(id){
          episode = this.claimManager.selectedClaim.episodes.find(episode=>episode.episodeId==id);
          console.log(episode);
      }
      swal({
          title: 'Episode Entry',
          html:
            `<div class="form-group">
                  <label id="noteTextLabel">Note Text</label>
                  <textarea class="form-control"  id="note" rows="5"  style="resize: vertical;">`+(episode !==undefined ? episode.note : '')+`</textarea>
              </div>
            `,
          showCancelButton: true,
          showLoaderOnConfirm:true,
          confirmButtonText:"Save",
          preConfirm: function () {
            return new Promise(function (resolve) {
              resolve([
                window['jQuery']('#note').val()
              ])
            })
          },
          onOpen: function () {
            window['jQuery']('#note').focus()
          }
        }).then( (result)=> {
          if(result[0]==""){
              this.toast.warning('Note Text is required!');
              setTimeout(()=>{
                this.episode(result[0]);
                window['jQuery']('#noteTextLabel').css({"color":"red"})
              },200)
          }else{
              swal({title:"",html:"Saving episode... <br/> <img src='assets/1.gif'>",showConfirmButton:false})
              this.http.saveEpisode(
                {
                  claimId: this.claimManager.selectedClaim.claimId,
                  episodeId: episode !==undefined ? episode.episodeId : null,
                  note: result[0],
                  by: episode !==undefined  ?episode.by : 'me',
                  date: episode !==undefined  ? episode.date : (new Date())
                }).single().subscribe(res=>{
                  let result = res.json()
                  swal.close();
                  this.claimManager.getClaimsDataById(this.claimManager.selectedClaim.claimId);
                  this.toast.success(result.message);
                },error=>{
                  setTimeout(()=>{
                    this.episode(id);
                    this.toast.error('An internal system error has occurred. This will be investigated ASAP.');
                  },200)
                })
          } 
        }).catch(swal.noop)
  }
  addNote(noteText:String="",TypeId?:String){
    let selectedNotes = [];
    noteText = noteText.replace(/\\n/g,'&#13;');
    let claimNoteTypeIds = '<option value="" style="color:purple">Select type</option>';
    this.claimManager.NoteTypes.forEach((note:{key:String,value:String})=>{
        claimNoteTypeIds=claimNoteTypeIds+'<option value="'+note.key+'"' +(note.value ==TypeId ? "selected": "")+'>'+note.value+'</option>';          
    }); 
      var width   = window.innerWidth*1.799/3;
    swal({
      title: 'Claim Note',
      width: width+'px',
      html:
        `<div class="form-group" style="text-align:center">
              <label id="claimNoteTypeLabel">Note type</label>
              <select class="form-control" id="noteTypeId" style="font-size:12pt;min-width:200px;width:350px;margin-left: calc(50% - 150px);">
                `+claimNoteTypeIds+`
              </select>
              <p style="font-size:11pt">Optional</p>
          </div>
          <div class="form-group">
              <label id="noteTextLabel">Note Text</label>
              <textarea class="form-control"  id="noteText" rows="5" cols="5"  style="resize: vertical;font-size:12pt;">`+noteText+`</textarea>
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
      /* if(result[0]==""){
          this.toast.warning('Please select a Note Type in order to Save your Note!');
          setTimeout(()=>{
              this.addNote(result[1],result[0]);
              window['jQuery']('#claimNoteTypeLabel').css({"color":"red"})
            },200)
      }else  */if(result[1]==""){
          this.toast.warning('Note Text is required!');
          setTimeout(()=>{
            this.addNote(result[1],result[0]);
            window['jQuery']('#noteTextLabel').css({"color":"red"})
          },200)
      }else{
          swal({title:"",html:"Saving note... <br/> <img src='assets/1.gif'>",showConfirmButton:false})
          console.log(JSON.stringify({text:result[1]}),{text:result[1]});
          var txt = JSON.stringify(result[1]);
          txt = txt.substring(1, txt.length - 1)
          this.http.saveClaimNote({
              claimId: this.claimManager.selectedClaim.claimId,
              noteTypeId:result[0] ? result[0] : null,
              noteText:txt
          }).subscribe(res => {
            let noteType = this.claimManager.NoteTypes.find(type=>type.key==result[0]);
            if(!this.claimManager.selectedClaim.claimNote){
              this.claimManager.selectedClaim.claimNote = new ClaimNote(txt,noteType.value)
            }else{
              this.claimManager.selectedClaim.claimNote.noteText=txt;
              this.claimManager.selectedClaim.claimNote.noteType=noteType.value;
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
