export interface Episode {
    episodeId: number,
    owner: string;
    created: string;
    patientName: string;
    claimNumber: any,
    type: string;
    pharmacy: string;
    carrier: string;
    fileUrl: string;
    episodeNote:string;
    episodeNoteCount:number;
  }