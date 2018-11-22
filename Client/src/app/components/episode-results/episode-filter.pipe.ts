import { Pipe, PipeTransform } from '@angular/core';
import {Episode} from '../../models/episode';
declare var $: any;
@Pipe({
  name: 'episodePipe'
})
export class EpisodesFilterPipe implements PipeTransform {

  transform(episodes: Array<Episode>, searchText: any, owner: any): any {

    if (searchText == null && (owner == null || !owner)) {
      return episodes;
    }
    return episodes.filter((episode) => {
      if (owner && searchText == null) {
        return (episode.owner.toLowerCase().includes(owner.toLowerCase()));
      } else if (owner && searchText != null) {
        return (episode.owner.toLowerCase().includes(owner.toLowerCase())) &&
          (episode.episodeNote.toLowerCase().includes(searchText.toLowerCase()));
      } else {
        return episode.episodeNote.toLowerCase().includes(searchText.toLowerCase());
      }
    });
  }
}
