export interface DocumentItem{
    documentId: any;
    fileName: string;
    extension: string;
    fileSize: string;
    creationTimeLocal: Date;
    lastAccessTimeLocal: Date;
    lastWriteTimeLocal: Date,
    fullFilePath: string;
    fileUrl: string;
}