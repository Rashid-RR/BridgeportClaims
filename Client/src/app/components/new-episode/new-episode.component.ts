import { Subject } from 'rxjs';
import { Component, Input, OnInit, NgZone } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ClaimManager } from '../../services/claim-manager';
import { EpisodeService } from '../../services/episode.service';
import swal from 'sweetalert2';

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
  @Input() episode;

  dropdownVisible = false;
  loading = false;
  showDropDown = new Subject<any>();
  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    public es: EpisodeService
  ) {

  }
  closeModal() {
    swal.clickCancel();
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
