import { Injectable } from '@angular/core';

@Injectable()
export class ReportLoaderService {
  loading: Boolean = false; 
  current:String='List';
  currentURL:String='List';
  constructor() { }

}
