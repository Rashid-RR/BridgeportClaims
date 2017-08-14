export class Episode {
  date: Date;
  by: String;
  readonly note?: String;
  noteText: String;
  episodeId: Number;
  episodeTypeId: String;
  claimId: Number;
  type:String;
  constructor(date: Date, by: String, note: String, episodeId: Number, episodeTypeId: String,type?:String) {
    this.date = date;
    this.by = by;
    this.type = type;
    this.noteText = note;
    this.episodeId = episodeId;
    this.episodeTypeId = episodeTypeId;
  }
}