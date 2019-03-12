import { Subject } from 'rxjs';
import { Component, Input, OnInit, NgZone } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ClaimManager } from '../../services/claim-manager';
import { EpisodeService } from '../../services/episode.service';
import { DecisionTreeService } from '../../services/decision-tree.service';
import {ConfirmComponent} from '../../components/confirm.component';
import {DialogService} from 'ng2-bootstrap-modal';
import swal from 'sweetalert2';

declare var treeWin: any;
declare var $: any;

@Component({
  selector: 'app-new-episode',
  templateUrl: './new-episode.component.html',
  styleUrls: ['./new-episode.component.css']
})
export class NewEpisodeComponent implements OnInit {

  payorId: any = '';
  submitted = false;
  searchText = '';
  @Input() episode:any;
  @Input() claimId:any;
  @Input() rootTreeId:any;
  @Input() leafTreeId:any;
  @Input() rootText:any;
  @Input() leafText:any;

  dropdownVisible = false;
  loading = false;
  showDropDown = new Subject<any>();
  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private ds: DecisionTreeService,
    private dialogService: DialogService,
    public es: EpisodeService
  ) {

  }
  closeModal() {
    if(this.claimId){
      this.dialogService.addDialog(ConfirmComponent, {
        title: 'Cancel Episode Progress and Experience',
        buttonText:'I understand',
        message: 'By cancelling, you will lose the episode entry and progress on your tree experience'
      }).subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
            swal.clickCancel();
            
        }
      });
    }else{
      swal.clickCancel();
      
    }
  }

  get autoCompletePharmacyName(): string {
    return this.http.baseUrl + '/reports/pharmacy-name/?pharmacyName=:keyword';
  }
  pharmacySelected($event) {
    if (this.episode && this.es.pharmacyName && $event.nabp) {
      this.es.episodeForm.controls['pharmacyNabp'].setValue($event.nabp);
      setTimeout(() => {
        this.dropdownVisible = false;
      }, 100);
    } else if (this.claimId && this.ds.pharmacyName && $event.nabp) {
      this.ds.episodeForm.controls['pharmacyNabp'].setValue($event.nabp);
      setTimeout(() => {
        this.dropdownVisible = false;
      }, 100);
    } else if (!this.episode && this.claimManager.pharmacyName && $event.nabp) {
      this.claimManager.episodeForm.controls['pharmacyNabp'].setValue($event.nabp);
      setTimeout(() => {
        this.dropdownVisible = false;
      }, 100);
    }
  }
  get auth() {
    const user = localStorage.getItem('user');
    try {
      const us = JSON.parse(user);
      return  `Bearer ${us.access_token}`;
    } catch (error) {

    }
    return null;
  }
  ngOnInit() {
    this.es.pharmacyName = '';
    this.claimManager.pharmacyName = '';
    /* this.es.payorListReady.subscribe(() => {
      $("#ePayorsSelection").select2();
    }) */
  }
  ngAfterViewInit() {
    // if(this.es.payors && this.es.payors.length>0){
      $('#ePayorsSelection').select2({
        ajax: {
          headers: {'Authorization': this.auth},
          url: function (params) {
            return '/api/reports/pharmacy-name/?pharmacyName=' + (params.term || '');
          },
          type: 'POST',
          processResults: function (data) {
            data.forEach(d => {
                d.id = d.nabp,
                d.text = d.pharmacyName;
            });
            return {
              results: (data || [])
            };
          }
        }
      });
    // }

  }

}
