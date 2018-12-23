export class Episode {
  date: Date;
  by: String;
  readonly note?: String;
  episodeNote: String;
  episodeId: Number;
  episodeTypeId: String;
  claimId: Number;
  owner: string;
  type: String;
  diaryId?: number;
  constructor(date: Date, by: String, note: String, episodeId: Number, episodeTypeId: String, type?: String,
    diaryId?: number, owner?: string) {
    this.date = date;
    this.by = by;
    this.type = type;
    this.episodeNote = note;
    this.episodeId = episodeId;
    this.episodeTypeId = episodeTypeId;
    this.diaryId = diaryId;
    this.owner = owner;
  }
}
