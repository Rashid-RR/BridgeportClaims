import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { EpisodeService } from "../../services/episode.service"
import { EventsService } from "../../services/events-service"
import { WindowInstance } from "../ng-window/WindowInstance";
import { Episode } from 'app/interfaces/episode';
import { HttpService } from 'app/services/services.barrel';

declare var $: any;

@Component({
  selector: 'app-episode-note-modal-window',
  templateUrl: './episode-note-modal.component.html',
  styleUrls: ['./episode-note-modal.component.css']
})
export class EpisodeNoteModalComponent implements OnInit, AfterViewInit {

  episode: Episode;
  episodeNotes:Array<{writtenBy:string,noteCreated:any,noteText:string}>=[];
  noteText: '';
  loading: boolean = false;
  constructor(
    public dialog: WindowInstance,
    private episodeService: EpisodeService,
    private events: EventsService,
    private http: HttpService,
    private toast: ToastsManager) {

  }

  ngOnInit() {
    this.dialog.config.BlockParentUI = true;
    this.loading = true;
    this.http.getEpisodeNotes(this.episode.episodeId).map(r => r.json()).single().subscribe(r => {
      let result = Object.prototype.toString.call(r) === '[object Array]' ? r[0] : r;
      this.episodeNotes = result.episodeNotes;  
      this.loading = false;
    }, err => { 
      this.loading = false;
    });
  }
  saveNote() {
    if (!this.noteText) {
      this.toast.warning("Please type in the note text")
    } else {
      console.log("Ready to send to API (", this.noteText, ")");
    }

  }
  formatDate(input: String) {
    if (!input) return null;
    if (input.indexOf("-") > -1) {
      let date = input.split("T");
      let d = date[0].split("-");
      return d[1] + "/" + d[2] + "/" + d[0];
    } else {
      return input;
    }
  }
  ngAfterViewInit() {

  }

  showNote(episode: Episode) {
    this.episode = episode;
  }
}
