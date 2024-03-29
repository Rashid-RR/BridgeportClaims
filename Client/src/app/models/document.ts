export interface DocumentItem {
    documentId: any;
    fileName: string;
    extension: string;
    fileSize: string;
    creationTimeLocal: Date;
    lastAccessTimeLocal: Date;
    lastWriteTimeLocal: Date;
    fullFilePath: string;
    fileUrl: string;
    rxNumber?: number;
    rxDate?: Date;
    added?: boolean;
    edited?: boolean;
    deleted?: boolean;
}
