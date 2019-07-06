export class ImportFile {
    createdOn: Date;
    fileExtension: string;
    fileName: string;
    importFileId: number;
    fileSize: string;
    fileType: string;
    processed: any;
    constructor(createdOn: Date, fileExtension: string, importFileId: number, fileName: string, fileSize: string,
      fileType: string,
      processed: any) {
      this.createdOn = createdOn;
      this.fileExtension = fileExtension;
      this.importFileId = importFileId;
      this.fileName = fileName;
      this.fileSize = fileSize;
      this.fileType = fileType;
      this.processed = processed;
    }
  }
