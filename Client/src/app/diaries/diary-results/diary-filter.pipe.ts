import { Pipe, PipeTransform } from '@angular/core';
import {Diary} from '../../models/diary';
declare var $: any;
@Pipe({
  name: 'filterDiary'
})
export class DiariesFilterPipe implements PipeTransform {

  transform(diaries: Array<Diary>, searchText: any, owner: any): any {

    if (searchText == null && (owner == null || !owner)) { return diaries; }
    return diaries.filter((diary) => {
      if (owner && searchText == null) {
        return (diary.owner.toLowerCase().includes(owner.toLowerCase()));
      } else if (owner && searchText != null) {
        return (diary.owner.toLowerCase().includes(owner.toLowerCase())) && (diary.diaryNote.toLowerCase().includes(searchText.toLowerCase()));
      } else {
        return diary.diaryNote.toLowerCase().includes(searchText.toLowerCase());
      }
    });
  }
}
