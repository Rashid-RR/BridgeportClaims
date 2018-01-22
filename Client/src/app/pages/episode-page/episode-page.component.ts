import { Component, OnInit } from '@angular/core';
import { EpisodeService } from 'app/services/episode.service';

@Component({
  selector: 'app-episode-page',
  templateUrl: './episode-page.component.html',
  styleUrls: ['./episode-page.component.css']
})
export class EpisodePageComponent implements OnInit {

  constructor(
    public ds: EpisodeService) { }

  ngOnInit() {
  }

}
