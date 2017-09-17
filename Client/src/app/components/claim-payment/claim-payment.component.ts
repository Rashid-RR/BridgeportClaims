import { SortColumnInfo } from "../../directives/table-sort.directive";
import { Component, OnInit, Renderer2, AfterViewInit, NgZone, HostListener, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import { HttpService } from "../../services/http-service";
import { Payment } from "../../models/payment";
import { EventsService } from "../../services/events-service";
import { DatePipe } from '@angular/common';
import { ConfirmComponent } from '../../components/confirm.component';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { DialogService } from "ng2-bootstrap-modal";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-claim-payment',
  templateUrl: './claim-payment.component.html',
  styleUrls: ['./claim-payment.component.css']
})
export class ClaimPaymentComponent implements OnInit {

  sortColumn:Array<SortColumnInfo>=[];
  payments:Array<Payment>=[];
  editing:Boolean=false;
  editingPaymentId:any;
  form:FormGroup;
  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    private formBuilder: FormBuilder,
    public claimManager: ClaimManager,
    private events: EventsService,
    private dialogService: DialogService,
    private toast: ToastsManager,
    private http: HttpService
  ) { 
    this.fetchData();
    this.form = this.formBuilder.group({
      checkAmt:[null],
      checkNumber:[null],
      rxDate:[null],
      prescriptionPaymentId:[null],
      prescriptionId:[null],
      postedDate:[null],
      rxNumber:[null],
      invoiceNumber:[null]
  });
    this.claimManager.onClaimIdChanged.subscribe(() => {
      this.fetchData();
    });
    
  }

  ngOnInit() {
     
  }

  onSortColumn(info: SortColumnInfo) {
    if(this.sortColumn.length==2 && this.sortColumn[1].column==info.column){
      this.sortColumn[1].dir=info.dir;
    }else if(this.sortColumn.length==1 && this.sortColumn[0].column==info.column){
      this.sortColumn[0].dir=info.dir;
    }else {
      this.sortColumn.push(info);
    }
    if(this.sortColumn.length>2){
      this.sortColumn=[this.sortColumn[this.sortColumn.length-2],this.sortColumn[this.sortColumn.length-1]];
    }
    this.fetchData();
  }

  update(payment:Payment){
    this.editing=true;
    this.editingPaymentId = payment.prescriptionPaymentId
    this.form = this.formBuilder.group({
        checkAmt:[payment.checkAmt],
        checkNumber:[payment.checkNumber],
        rxDate:[payment.rxDate],
        prescriptionPaymentId:[payment.prescriptionPaymentId],
        prescriptionId:[payment.prescriptionId],
        postedDate:[payment.postedDate],
        rxNumber:[payment.rxNumber],
        invoiceNumber:[payment.invoiceNumber]
    });
  }

  savePayment(){

  }
  checkNumber($event){

  }
  textChange(controlName:string){
    /* if(this.form.get(controlName).value ==='undefined' || this.form.get(controlName).value ===''){
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
    } */
  }
  cancel(){
    this.editing = false;
    this.editingPaymentId = undefined;
    this.form.patchValue({
      checkAmt:[null],
      checkNumber:[null],
      rxDate:[null],
      prescriptionPaymentId:[null],
      prescriptionId:[null],
      postedDate:[null],
      rxNumber:[null],
      invoiceNumber:[null]
    })
  }

  del(payment:Payment){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Delete payment",
      message: ""
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true
          this.http.deletePrescriptionPayment(payment.prescriptionPaymentId).map(r=>{return r.json()}).single().subscribe(res=>{              
              this.toast.success(res.message);
              this.removePayment(payment);
              this.claimManager.loading = false;
          },error=>{                          
            this.toast.error(error.message);
            this.claimManager.loading = false;
          });
        }
        else {}
      });
  }

  removePayment(payment)
  {
    for(var i=0;i<this.payments.length;i++){
      if(payment.prescriptionPaymentId == this.payments[i].prescriptionPaymentId && this.payments[i].prescriptionId == payment.prescriptionId){
        this.payments.splice( i, 1 );
      }
    }    
  } 
  fetchData() {
    let page = 1;
    let page_size = 1000;
    let sort: string = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc'; 
    let sort2: string = 'RxNumber';
    let sort_dir2: 'asc' | 'desc' = 'asc';
    if (this.sortColumn[0]) {
      sort = this.sortColumn[0].column;
      sort_dir = this.sortColumn[0].dir;
    }
    if (this.sortColumn[1]) {
      sort2 = this.sortColumn[1].column;
      sort_dir2 = this.sortColumn[1].dir;
    }
    this.http.getPayments(this.claimManager.selectedClaim.claimId, sort, sort_dir,sort2, sort_dir2,
      page, page_size).map(p => p.json())
      .subscribe(results => {
        this.payments = results;
      });
  }

}
