import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { EpisodeService } from "../../services/episode.service"
import { EventsService } from "../../services/events-service"
import { WindowInstance } from "../ng-window/WindowInstance";
import { Episode } from 'app/interfaces/episode';

declare var $: any;

@Component({
  selector: 'app-episode-note-modal-window',
  templateUrl: './episode-note-modal.component.html',
  styleUrls: ['./episode-note-modal.component.css']
})
export class EpisodeNoteModalComponent implements OnInit, AfterViewInit {

  episode: Episode;
  episodeNotes = [
  {
    "owner": "Test Test",
    "created": "2018-09-16T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Gurney, Jordan",
    "created": "2018-09-14T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "TestLastName, TestFirstName",
    "created": "2018-09-13T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-09-12T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-09-07T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Aftab, Ahmed",
    "created": "2018-08-19T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Gurney, Jordan",
    "created": "2018-08-09T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-08-09T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },

  {
    "owner": "Gurney, Jordan",
    "created": "2018-08-03T00:00:00.0000000+00:00",
    "note": 0
  },
  {
    "owner": "Test Test",
    "created": "2018-07-17T00:00:00.0000000+00:00",
    "note": 0
  },
  {
    "owner": "Mansha1, Waseem1",
    "created": "2018-07-12T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-06-29T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-06-29T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Condie, Adam",
    "created": "2018-06-11T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Aftab, Ahmed",
    "created": "2018-06-08T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Masood, Atiq",
    "created": "2018-06-06T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Mansha1, Waseem1",
    "created": "2018-05-23T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-05-22T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Sarwari, Ahmad",
    "created": "2018-04-28T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Todor, Csaba",
    "created": "2018-04-27T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Condie, Adam",
    "created": "2018-04-12T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-04-09T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-03-19T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  },
  {
    "owner": "Test Test",
    "created": "2018-03-13T00:00:00.0000000+00:00",
    "note": "plorum quartu novum nomen si parte quo esset si quantare imaginator quo, volcans et regit, brevens, fecundio"
  }]
  noteText:'';
  constructor(
    public dialog: WindowInstance,
    private episodeService: EpisodeService,
    private events: EventsService,
    private toast: ToastsManager) {

  }

  ngOnInit() {
    this.dialog.config.BlockParentUI = true;
  }
  saveNote(){
    if(!this.noteText){
      this.toast.warning("Please type in the note text")
    }else{
        console.log("Ready to send to API (",this.noteText,")");
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
