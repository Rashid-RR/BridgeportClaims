import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Toast, ToastrService } from 'ngx-toastr';
import { EpisodeService } from '../../services/episode.service';
import { EventsService } from '../../services/events-service';
import { WindowInstance } from '../ng-window/WindowInstance';
import { Episode } from '../../interfaces/episode';
import { HttpService } from '../../services/services.barrel';
import swal from 'sweetalert2';
import { TextSelectEvent } from '../../directives/text-select.directive';

declare var $: any;

interface SelectionRectangle {
  left: number;
  top: number;
  width: number;
  height: number;
}

@Component({
  selector: 'app-episode-note-modal-window',
  templateUrl: './episode-note-modal.component.html',
  styleUrls: ['./episode-note-modal.component.css']
})
export class EpisodeNoteModalComponent implements OnInit, AfterViewInit {

  episode: Episode;
  episodeNotes: Array<{ episodeId: any, writtenBy: string, noteCreated: any, noteText: string }> = [];
  noteText: '';
  loading = false;
  higlighted: any;

  public hostRectangle: SelectionRectangle | null;
  private selectedText: string;

  constructor(
    public dialog: WindowInstance,
    private episodeService: EpisodeService,
    private events: EventsService,
    private http: HttpService,
    private toast: ToastrService) {
    this.hostRectangle = null;
    this.selectedText = '';
  }

  ngOnInit() {
    this.dialog.config.BlockParentUI = true;
    this.loading = true;
    this.http.getEpisodeNotes(this.episode.episodeId).subscribe(r => {
      const result = Object.prototype.toString.call(r) === '[object Array]' ? r[0] : r;
      if (result && result.episodeNotes) {
        this.episodeNotes = result.episodeNotes;
      }
      this.loading = false;
    }, err => {
      this.loading = false;
    });
    window.scrollTo(0, 0);
  }
  saveNote() {
    if (!this.noteText) {
      this.toast.warning('Please type in the note text');
    } else {
      swal.fire({
        title: '',
        html: 'Saving note... <br/> <img src=\'assets/1.gif\'>',
        showConfirmButton: false
      }).then(_ => {

      }).catch(() => {});
      this.http.saveEpisodeNote({ episodeId: this.episode.episodeId, note: this.noteText }).subscribe(r => {
        const result = Object.prototype.toString.call(r) === '[object Array]' ? r[0] : r;
        this.episodeNotes.splice(0, 0, { episodeId: this.episode.episodeId, writtenBy: result.owner, noteCreated: result.created, noteText: this.noteText });
        const episode = this.episodeService.episodes.get(this.episode.episodeId);
        if (episode) {
          episode.episodeNoteCount++;
        }
        this.events.broadcast('episode-note-updated', { episodeId: this.episode.episodeId, episodeNoteCount: this.episodeNotes.length });
        this.toast.success(result.message);
        this.episodeService.episodes = this.episodeService.episodes.set(this.episode.episodeId, episode);
        this.loading = false;
        this.noteText = '';
        swal.clickCancel();
      }, err => {
        this.loading = false;
        this.toast.error(err.message);
        swal.clickCancel();
      });
    }

  }
  formatText(noteText: string = '') {
    return noteText.replace(/\\n/g, '<br/>');
  }
  formatDate(input: String) {
    if (!input) { return null; }
    if (input.indexOf('-') > -1) {
      const date = input.split('T');
      const d = date[0].split('-');
      return d[1] + '/' + d[2] + '/' + d[0];
    } else {
      return input;
    }
  }
  ngAfterViewInit() {

  }
  getSelectedText() {
    if (window.getSelection) {
      return window.getSelection().toString();
    } else if (document['selection']) {
      return document['selection'].createRange().text;
    }
    return '';
  }
  isHighlighted(text) {
    if (window.getSelection) {
      $('#highlighter').html(text);
      const selection = window.getSelection() || document['selection'];
      selection.selectAllChildren(document.getElementById('highlighter'));
    }
    document.execCommand('copy');
    this.toast.success('Note copied to clipboard');
  }

  showNote(episode: Episode) {
    this.episode = episode;
  }
  public renderRectangles(event: TextSelectEvent): void {
    // If a new selection has been created, the viewport and host rectangles will
    // exist. Or, if a selection is being removed, the rectangles will be null.
    if (event.hostRectangle) {
      this.hostRectangle = event.hostRectangle;
      this.selectedText = event.text;
    } else {
      this.hostRectangle = null;
      this.selectedText = '';
    }
  }


  // I share the selected text with friends :)
  public shareSelection(): void {
    // Now that we've shared the text, let's clear the current selection.
    document.getSelection().removeAllRanges();
    // CAUTION: In modern browsers, the above call triggers a "selectionchange"
    // event, which implicitly calls our renderRectangles() callback. However,
    // in IE, the above call doesn't appear to trigger the "selectionchange"
    // event. As such, we need to remove the host rectangle explicitly.
    this.hostRectangle = null;
    this.selectedText = '';
  }
}
