import { Subject } from 'rxjs/Subject';
import { Component, Input, OnInit, NgZone } from '@angular/core';
import { HttpService } from "../../services/http-service"
import { ClaimManager } from "../../services/claim-manager";
import { EpisodeService } from "../../services/episode.service";
import swal from "sweetalert2";

declare var $: any

@Component({
  selector: 'app-new-episode',
  templateUrl: './new-episode.component.html',
  styleUrls: ['./new-episode.component.css']
})
export class NewEpisodeComponent implements OnInit {
 
  payorId: any = '';
  submitted: boolean = false;
  searchText: string = '';
  @Input() episode;

  dropdownVisible: boolean = false; 
  loading: boolean = false
  showDropDown = new Subject<any>();
  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    public es:EpisodeService
  ) {
  }
  closeModal() {
    swal.clickCancel();
  }

  get autoCompletePharmacyName(): string {
    return this.http.baseUrl + "/reports/pharmacy-name/?pharmacyName=:keyword";
  } 
  pharmacySelected($event) {
    if (this.episode && this.es.pharmacyName && $event.nabp) {
      this.es.episodeForm.controls['pharmacyNabp'].setValue($event.nabp)
      setTimeout(() => {
        this.dropdownVisible = false
      }, 100);
    }else if (!this.episode && this.claimManager.pharmacyName && $event.nabp) {
      this.claimManager.episodeForm.controls['pharmacyNabp'].setValue($event.nabp)
      setTimeout(() => {
        this.dropdownVisible = false
      }, 100);
    }
  }
  ngOnInit() {
    this.es.pharmacyName='';
    this.claimManager.pharmacyName='';
  }

}
