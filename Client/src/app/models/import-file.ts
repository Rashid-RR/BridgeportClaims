export class ImportFile {
    createdOn: Date;
    fileExtension: String;
    fileName: String;
    importFileId: Number; 
    constructor(createdOn: Date, fileExtension: String, importFileId: Number, fileName: String) {
      this.createdOn = createdOn;
      this.fileExtension = fileExtension;
       this.importFileId = importFileId;
      this.fileName = fileName;
    }
  }