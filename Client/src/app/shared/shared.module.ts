import { NgModule, Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ConfirmComponent } from '../components/confirm.component';
import { ShContextMenuModule } from 'ng2-right-click-menu';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { AutoCompleteModule } from '../auto-complete';
import { WindowsInjetor, WindowBackdrop, BootstrapWindowContainer } from '../components/ng-window';
import { FilterUserPipe } from '../user/list/filter-user.pipe';
import { DiariesFilterPipe } from '../diaries/diary-results/diary-filter.pipe';
import { EpisodesFilterPipe } from '../components/episode-results/episode-filter.pipe';
import {
  UnpaidScriptResultsComponent, UnpaidScriptSearchComponent,InvoiceSearchComponent,
   EpisodeNoteModalComponent,UnindexedImageFilterComponent, UnindexedImageListComponent, ScriptNoteWindowComponent
} from '../components/components-barrel';
import { ColumnSortDirective } from '../directives/column-sort.directive';
import { TableSortDirective } from '../directives/table-sort.directive';
import { TextSelectDirective } from '../directives/text-select.directive';
import { PhonePipe } from '../pipes/phone-pipe';
import { DisplayRolesPipe } from '../pipes/display-roles.pipe';
import { ArraySortPipe } from '../pipes/sort.pipe';
import { DeleteIndexConfirmationComponent } from '../payment/delete-index-confirmation.component';

@Pipe({ name: 'safeStyle' })
export class SafeStylePipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }

  transform(value: any) {
    return this.sanitized.bypassSecurityTrustStyle(value);
  }
}
@Pipe({ name: 'safeURL' })
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }
  transform(value: any) {
    return this.sanitized.bypassSecurityTrustUrl(value);
  }
}

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    ShContextMenuModule,
    SweetAlert2Module.forRoot(),
  ],
  declarations: [
    ConfirmComponent,BootstrapWindowContainer, WindowBackdrop, ScriptNoteWindowComponent, EpisodeNoteModalComponent,
    TableSortDirective, ColumnSortDirective,TextSelectDirective,DeleteIndexConfirmationComponent,
    DisplayRolesPipe, ArraySortPipe, PhonePipe, EpisodesFilterPipe,SafeStylePipe, SafeUrlPipe,FilterUserPipe,DiariesFilterPipe,
    UnpaidScriptResultsComponent, UnpaidScriptSearchComponent,InvoiceSearchComponent,
     EpisodeNoteModalComponent,UnindexedImageFilterComponent, UnindexedImageListComponent, ScriptNoteWindowComponent
  ],
  providers:[
    WindowsInjetor,
    DisplayRolesPipe,ArraySortPipe,SafeStylePipe, PhonePipe, EpisodesFilterPipe, SafeUrlPipe,FilterUserPipe,DiariesFilterPipe
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    ShContextMenuModule,
    SweetAlert2Module,
    TableSortDirective, ColumnSortDirective,TextSelectDirective,
    DisplayRolesPipe,ArraySortPipe, PhonePipe, EpisodesFilterPipe, SafeUrlPipe,SafeStylePipe,FilterUserPipe,DiariesFilterPipe,
    UnpaidScriptResultsComponent, UnpaidScriptSearchComponent,
    EpisodeNoteModalComponent,InvoiceSearchComponent,
    UnindexedImageFilterComponent, UnindexedImageListComponent, ScriptNoteWindowComponent
  ],
  entryComponents: [
    ConfirmComponent, BootstrapWindowContainer, WindowBackdrop, ScriptNoteWindowComponent, EpisodeNoteModalComponent,DeleteIndexConfirmationComponent
  ],
})
export class SharedModule { }
