export class ImportFile {
    createdOn: Date;
    fileExtension: String;
    fileName: String;
    importFileId: Number; 
    fileSize:String;                
    fileType:String;
    processed:any;
    constructor(createdOn: Date, fileExtension: String, importFileId: Number, fileName: String,fileSize:String,
      fileType:String,
      processed:any) {
      this.createdOn = createdOn;
      this.fileExtension = fileExtension;
      this.importFileId = importFileId;
      this.fileName = fileName;
      this.fileSize = fileSize;                
      this.fileType = fileType;
      this.processed = processed;
    }
  }