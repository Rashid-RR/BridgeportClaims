import { CommonModule } from '@angular/common';
import { NgModule, Pipe, PipeTransform } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ShContextMenuModule } from 'ng2-right-click-menu';
import { AutoCompleteModule } from '../auto-complete';
// tslint:disable-next-line: max-line-length
import { EpisodeNoteModalComponent, InvoiceSearchComponent, NewEpisodeComponent, ScriptNoteWindowComponent, UnindexedImageFilterComponent, UnindexedImageListComponent, UnpaidScriptResultsComponent, UnpaidScriptSearchComponent } from '../components/components-barrel';
import { ConfirmComponent } from '../components/confirm.component';
import { EpisodesFilterPipe } from '../components/episode-results/episode-filter.pipe';
import { BootstrapWindowContainer, WindowBackdrop, WindowsInjetor } from '../components/ng-window';
import { TreeListFilterComponent } from '../decision-tree//tree-list-filter/tree-list-filter.component';
import { TreeListGridComponent } from '../decision-tree/tree-list-grid/tree-list-grid.component';
import { TreeListComponent } from '../decision-tree/tree-list/tree-list.component';
import { DiariesFilterPipe } from '../diaries/diary-results/diary-filter.pipe';
import { ColumnSortDirective } from '../directives/column-sort.directive';
import { TableSortDirective } from '../directives/table-sort.directive';
import { TextSelectDirective } from '../directives/text-select.directive';
import { MaterialComponentsModule } from '../material/material-components.module';
import { DeleteIndexConfirmationComponent } from '../payment/delete-index-confirmation.component';
import { BridgeportDatePipe } from '../pipes/date.pipe';
import { DisplayRolesPipe } from '../pipes/display-roles.pipe';
import { PhonePipe } from '../pipes/phone-pipe';
import { ArraySortPipe } from '../pipes/sort.pipe';
import { FilterUserPipe } from '../user/list/filter-user.pipe';
import { MaterialModule } from './material.module';
import { NotificationComponent } from '../dashboard/notification/notification.component';
import { NotificationDetailsComponent } from '../dashboard/notification-details/notification-details.component';
import { PayorSearchComponent } from '../components/payor-search/payor-search.component';
import { RouterModule } from '@angular/router';
import {NgxMaskModule } from 'ngx-mask';
import { AgDateFilterComponent } from '../components/ag-date-filter/ag-date-filter.component';
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
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    ShContextMenuModule,
    MaterialComponentsModule,
    MaterialModule,
    SweetAlert2Module.forRoot(),
    NgxMaskModule.forRoot()
  ],
  declarations: [
    ConfirmComponent, BootstrapWindowContainer, WindowBackdrop, ScriptNoteWindowComponent, EpisodeNoteModalComponent,
    TableSortDirective, ColumnSortDirective, TextSelectDirective, DeleteIndexConfirmationComponent,
    DisplayRolesPipe, BridgeportDatePipe, ArraySortPipe, PhonePipe, EpisodesFilterPipe, SafeStylePipe, SafeUrlPipe, FilterUserPipe, DiariesFilterPipe,
    UnpaidScriptResultsComponent, UnpaidScriptSearchComponent, InvoiceSearchComponent,
     EpisodeNoteModalComponent, UnindexedImageFilterComponent, UnindexedImageListComponent, ScriptNoteWindowComponent,
     TreeListGridComponent, TreeListFilterComponent, TreeListComponent, NewEpisodeComponent,
     NotificationComponent, NotificationDetailsComponent, PayorSearchComponent, AgDateFilterComponent
  ],
  providers: [
    WindowsInjetor,
    DisplayRolesPipe, BridgeportDatePipe, ArraySortPipe, SafeStylePipe, PhonePipe, EpisodesFilterPipe, SafeUrlPipe, FilterUserPipe, DiariesFilterPipe
  ],
  exports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    ShContextMenuModule,
    SweetAlert2Module, NgxMaskModule,
    TableSortDirective, ColumnSortDirective, TextSelectDirective,
    DisplayRolesPipe, BridgeportDatePipe, ArraySortPipe, PhonePipe, EpisodesFilterPipe, SafeUrlPipe, SafeStylePipe, FilterUserPipe, DiariesFilterPipe,
    UnpaidScriptResultsComponent, UnpaidScriptSearchComponent,
    EpisodeNoteModalComponent, InvoiceSearchComponent, NewEpisodeComponent,
    UnindexedImageFilterComponent, UnindexedImageListComponent, ScriptNoteWindowComponent,
    TreeListGridComponent, TreeListFilterComponent, TreeListComponent, MaterialComponentsModule,
    NotificationComponent, NotificationDetailsComponent, PayorSearchComponent, AgDateFilterComponent
  ],
  entryComponents: [
    ConfirmComponent, BootstrapWindowContainer, WindowBackdrop, ScriptNoteWindowComponent,
    EpisodeNoteModalComponent, DeleteIndexConfirmationComponent, NewEpisodeComponent,
    TreeListGridComponent, TreeListFilterComponent, TreeListComponent
  ]
})
export class SharedModule { }
