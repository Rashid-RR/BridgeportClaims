import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DiaryInputComponent } from './diary-input/diary-input.component';
import { DiaryResultsComponent } from './diary-results/diary-results.component';
import { DiaryComponent } from './diary/diary.component'; 
import { DiaryRoutes } from './diaries.routing';
import { SharedModule } from '../shared';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(DiaryRoutes),
  ],
  declarations: [
    DiaryComponent, DiaryInputComponent, DiaryResultsComponent]
})
export class DiariesModule { }
