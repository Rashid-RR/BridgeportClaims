import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {PaymentPosting} from "../../models/payment-posting";
import {PaymentPostingPrescription} from "../../models/payment-posting-prescription";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';
import {DecimalPipe } from '@angular/common';
import {LocalStorageService} from 'ng2-webstorage';
import {DialogService } from "ng2-bootstrap-modal";
import {ConfirmComponent } from '../../components/confirm.component';
import swal from "sweetalert2";

@Component({
  selector: 'app-payment-input',
  templateUrl: './payment-input.component.html',
  styleUrls: ['./payment-input.component.css']
})
export class PaymentInputComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  disableCheckEntry: boolean = false;
  checkAmount:number = 0;
  constructor(private decimalPipe:DecimalPipe,public paymentService:PaymentService,
    private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService,private toast: ToastsManager,
    private localSt:LocalStorageService,private dialogService: DialogService) {
    this.form = this.formBuilder.group({
      checkNumber: [null],
      checkAmount: [null],
      amountSelected: [null],
      amountToPost: [null],
      amountRemaining: [null]
    });
    this.events.on("payment-suspense",a=>{
        this.form.patchValue({
          checkNumber: null,
          checkAmount: Number(0).toFixed(2),
          amountSelected: Number(0).toFixed(2),
          amountToPost: null,
          amountRemaining: null
        });
        this.paymentService.paymentPosting = new PaymentPosting();
    });
    this.events.on("payment-closed",a=>{
        this.form.patchValue({
          checkNumber: null,
          checkAmount: Number(0).toFixed(2),
          amountSelected: Number(0).toFixed(2),
          amountToPost: null,
          amountRemaining: null
        });
        this.paymentService.paymentPosting = new PaymentPosting();
    });
    this.events.on("payment-amountRemaining",a=>{
         this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(a.amountRemaining),"1.2-2"));
         let c=Number(this.form.get('checkAmount').value.replace(",","")).toFixed(2);
         let checkAmount  = Number(c);
         var form: any={};
         form = this.form.value;
         form.checkAmount = Number(form.checkAmount.replace(",","")).toFixed(2);          
         this.paymentService.paymentPosting.sessionId=a.sessionId;
         this.paymentService.paymentPosting.checkAmount=form.checkAmount;
         this.paymentService.paymentPosting.checkNumber=form.checkNumber;
         console.log(checkAmount-(a.amountRemaining as number));
         this.paymentService.paymentPosting.lastAmountRemaining = a.amountRemaining
         form.paymentPostings = this.paymentService.paymentPosting.paymentPostings;
         form.lastAmountRemaining = a.amountRemaining;
         form.sessionId = this.paymentService.paymentPosting.sessionId;
         /* form.checkAmount = this.paymentService.paymentPosting.checkAmount;
         form.checkNumber = this.paymentService.paymentPosting.checkNumber; */
         form.amountSelected = this.paymentService.paymentPosting.amountSelected;
         console.log(form);
         if (a.amountRemaining == 0) {
          this.finalizePosting();
         }else if (a.amountRemaining <= 0) {
          this.events.broadcast("disable-links",false);
          this.paymentService.prescriptionSelected = false; 
        }else if(checkAmount-(a.amountRemaining as number)>0){
           this.disableCheckEntry = true;
           this.events.broadcast("disable-links",true);
         }
         this.paymentService.unSelectAllPrescriptions();
         this.form.get('amountToPost').setValue(null);         
    })
  }
  ngOnInit() {
    this.paymentService.paymentPosting = new PaymentPosting();
  }

  finalizePosting(){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Permanently Save Payment",
      message: "Your payments are ready for saving. Would you like to permanently save now?"
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {  
          this.paymentService.finalizePosting({sessionId:this.paymentService.paymentPosting.sessionId});
        }
        else {
           
        }
      });
  }
  cancel(){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Cancel posting",
      message: "Are you sure you want to cancel "+this.paymentService.paymentPosting.paymentPostings.length+" posting"+(this.paymentService.paymentPosting.paymentPostings.length>1 ? 's':'')+"?"
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {  
          this.paymentService.prescriptionSelected=false;
          this.events.broadcast("disable-links",false);           
        }
        else {
           
        }
      });
  }
  search(){
 
  }

  textChange(controlName:string){
    if(this.form.get(controlName).value ==='undefined' || this.form.get(controlName).value ===''){
      this.form.get(controlName).setValue(null);
    }else{
      switch(controlName){
        case 'checkAmount':
        case 'amountToPost':
          var val = this.form.get(controlName).value.replace(",",'');
          this.form.get(controlName).setValue(this.decimalPipe.transform(val,"1.2-2"));
        break;
        default:
        break;

      }
    }
  }
  checkNumber($event){
    $event = ($event) ? $event : window.event;
    var charCode = ($event.which) ? $event.which : $event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode!=44 && charCode!=46) {
        return false;
    }
    return true;
  }
  post(){
    var payments = [];
    var form  = this.form.value;
    if (this.form.get('checkNumber').value == null){
      this.toast.warning('The Check # field is mandatory in order to save the payment posting.');
    }else if (form.checkAmount == null || form.checkAmount == 0){
          this.toast.warning('The Check Amount field is mandatory in order to save the payment posting.');
    }else{
      form.amountRemaining = undefined;
      form.amountToPost = form.amountToPost !==null ? Number((form.amountToPost || 0).replace(",","")).toFixed(2) :(0).toFixed(2);
      
      this.paymentService.detailedClaimsData.forEach(p=>{
          if(p.selected){
            this.paymentService.paymentPosting.payments = this.paymentService.paymentPosting.payments.set(p.prescriptionId, new PaymentPostingPrescription(p.patientName,p.rxDate,p.invoicedAmount,p.prescriptionId))       
            payments.push({
              patientName: p.patientName,
              rxDate: p.rxDate,
              amountPosted: form.amountToPost,
              prescriptionId: p.prescriptionId,
              invoiceAmount:p.invoicedAmount
            });
          }
      });

      form.paymentPostings = payments;
      //this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(this.amountRemaining),"1.2-2"));
      if(this.paymentService.paymentPosting.lastAmountRemaining == 0){
        this.finalizePosting();
      }else{
        form.amountSelected = Number((form.amountSelected || "0").replace(",","")).toFixed(2);
        form.checkAmount = Number(form.checkAmount.replace(",","")).toFixed(2); 
        this.paymentService.paymentPosting.checkAmount = Number(Number((form.checkAmount || "0").replace(",","")).toFixed(2));
        form.amountSelected = this.paymentService.paymentPosting.amountSelected ;
        this.paymentService.paymentPosting.checkNumber = form.checkNumber;
        form.lastAmountRemaining=this.paymentService.paymentPosting.lastAmountRemaining;
        form.sessionId=this.paymentService.paymentPosting.sessionId;
        if(this.paymentService.selected.length>1 && form.amountToPost!=form.amountSelected){
          this.toast.warning('Multi-line payments are not permitted for posting unless the "Amount Selected" is equal to the "Amount To Post"');
        }/* else if(form.checkAmount > form.amountToPost){      
          this.localSt.store("partial-payment",payments);
          this.toast.info("Posting has been saved. Please continue posting until the Check Amount is posted in full before it is saved to the database");
        } */
        else if(Number(form.amountToPost) > Number(form.checkAmount)){
          console.log(Number(form.amountToPost) > Number(form.checkAmount));
          this.toast.warning("The amount to post you specified is greater than the check amount. Please correct to proceed");
        }else if(form.amountToPost==0 || form.amountToPost==null){
          this.toast.warning("You need to specify amount to post");
        }else {
          this.paymentService.post(form);
        }
      }
    }
  }
  get amountRemaining():Number{
    var checkAmount = Number(this.form.get('checkAmount').value.replace(",",""));
    var amountSelected = Number(this.form.get('amountSelected').value.replace(",",""));
    var amount =  Number((checkAmount ? checkAmount : 0)  - amountSelected);
    return amount;
  }
  tosuspense(noteText: String = ""){
    var form  = this.form.value;
    form.checkAmount = Number(form.checkAmount.replace(",","")).toFixed(2); 
    var amountSelected = Number(this.form.get('amountSelected').value.replace(",",""));
    form.lastAmountRemaining=this.paymentService.paymentPosting.lastAmountRemaining;
    if (this.form.get('checkNumber').value == null)
      this.toast.warning('The Check # field is mandatory in order to conclude the payment posting process.');
    else if (form.checkAmount == null || form.checkAmount == 0)
          this.toast.warning('The Check Amount field is mandatory in order to conclude the payment posting process.');
    else if (this.paymentService.selected.length>0 || amountSelected > 0)
          this.toast.warning('You have selected '+this.paymentService.selected.length+' row'+(this.paymentService.selected.length>1 ? 's' : '')+' prescriptions. You cannot associate any prescriptions to an Amount that you are suspending. Please uncheck all prescriptions and retry.');
    else{
      var width = window.innerWidth * 1.799 / 3;
      let amountToSuspend = (form.amountToPost != null && form.amountToPost > 0) ? form.amountToPost : form.checkAmount;
      
      if(form.lastAmountRemaining==0){
        this.confirmSuspense(amountToSuspend,'');  
      }else{
        swal({
          title: 'Claim Note',
          width: width + 'px',
          html:
          `<div class="form-group">
                  <label id="noteTextLabel">Suspense Note (Optional):</label>
                  <textarea class="form-control"  id="noteText" rows="5" cols="5"  style="resize: vertical;font-size:12pt;">`+ noteText + `</textarea>
              </div>
            `,
          showCancelButton: true,
          showLoaderOnConfirm: true,
          confirmButtonText: "Save",
          preConfirm: function () {
            return new Promise(function (resolve) {
              resolve([
                window['jQuery']('#noteText').val()
              ])
            })
          },
          onOpen: function () {
            window['jQuery']('#noteText').focus()
          }
        }).then((result) => {
          this.confirmSuspense(amountToSuspend,result[0]);    
        });
      }      
    }
    
  }
  confirmSuspense(amountToSuspend:Number,text:String){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Suspense postings",
      message: "Are you sure you are ready to post "+this.paymentService.paymentPosting.paymentPostings.length+" posting"+(this.paymentService.paymentPosting.paymentPostings.length>1 ? 's':'')+" and an amount of "+amountToSuspend+" to suspense?"
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {  
          this.paymentService.paymentToSuspense({sessionId:this.paymentService.paymentPosting.sessionId,amountToSuspense:amountToSuspend,noteText:text});             
        }
        else {
           
        }
      });
  }

}
