export interface Episode {
    episodeId: number,
    owner: string;
    category: string;
    created: string;
    patientName: string;
    claimNumber: any,
    rxNumber: any,
    type: string;
    role: string;
    pharmacy: string;
    carrier: string;
    fileUrl: string;
    episodeNote:string;
    episodeNoteCount:number;
    noteCount:number;
    resolved:boolean;
  }