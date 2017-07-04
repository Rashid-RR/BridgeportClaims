import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {ClaimManager} from "../../services/claim-manager";
import {PrescriptionNoteType} from "../../models/prescription-note-type";
import swal from "sweetalert2";
import {warn,success} from "../../models/notification"

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {
  
  expanded:Boolean=false
  expandedBlade:Number=0;
  
  constructor(public claimManager:ClaimManager,private http:HttpService) {
     
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
     this.claimManager.selectedClaim.prescriptions.forEach(prescription=>{
        if(prescription.selected){
            selectedPrecriptions = selectedPrecriptions + '<span class="label label-info"  style="margin:2px;display:inline-flex">'+prescription.labelName+'</span> &nbsp; ';
            selectedNotes.push(Number(prescription.rxNumber));
        }
    });
     
    swal({
      title: 'New Prescription Note',
      html:
        `
              <div class="form-group">
                  <label id="prescriptionNoteTypeLabel">Prescription Note type</label>
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
          warn('Please select one type!');
          setTimeout(()=>{
              this.addPrescriptionNote(result[1],result[0]);
              window['jQuery']('#prescriptionNoteTypeLabel').css({"color":"red"})
            },200)
      }else if(result[1]==""){
          warn('Note Text is required!');
          setTimeout(()=>{
            this.addPrescriptionNote(result[1],result[0]);
            window['jQuery']('#noteTextLabel').css({"color":"red"})
          },200)
      }else{
          swal({title:"",html:"Saving note... <br/> <i class='fa fa-refresh fa-2x fa-spin'></i>",showConfirmButton:false})
          this.http.savePrescriptionNote(
            {
              claimId: this.claimManager.selectedClaim.claimId,
              noteText: result[1],
              prescriptionNoteTypeId: Number(result[0]),
              prescriptions: selectedNotes,
              prescriptionNoteId: prescriptionNoteId
            }).single().subscribe(result=>{
              console.log(result);
              swal.close();
              success("Noted successfully saved");
            },error=>{
              setTimeout(()=>{
                this.addPrescriptionNote(result[1],result[0]);
                warn('Server error!');
              },200)
            })
      } 
    }).catch(swal.noop)
  }

}
