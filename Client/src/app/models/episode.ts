export class Episode{
  date:Date;
  by:String;
  note:String;
  episodeId:Number;
  constructor(date:Date,by:String,note:String,episodeId:Number){
      this.date = date;
      this.by = by;
      this.note = note;
      this.episodeId = episodeId;
  }
  
}