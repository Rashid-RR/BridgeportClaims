export class Episode {
  date: Date;
  by: String;
  readonly note?: String;
  noteText: String;
  episodeId: Number;
  episodeTypeId: String;
  claimId: Number;
  constructor(date: Date, by: String, note: String, episodeId: Number, episodeTypeId: String) {
    this.date = date;
    this.by = by;
    this.noteText = note;
    this.episodeId = episodeId;
    this.episodeTypeId = episodeTypeId;
  }
}