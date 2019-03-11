export interface Episode {
    episodeId: number;
    owner: string;
    category: string;
    created: string;
    patientName: string;
    claimNumber: string;
    claimId: number;
    rxNumber: string;
    type: string;
    role: string;
    pharmacy: string;
    carrier: string;
    fileUrl: string;
    episodeNote: string;
    episodeNoteCount: number;
    noteCount: number;
    resolved: boolean;
  }
