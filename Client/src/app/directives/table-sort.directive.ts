import { Directive, ViewChildren, OnChanges, Input, Output,
  SimpleChange, SimpleChanges, QueryList, EventEmitter } from '@angular/core';

import { ColumnSortDirective } from './column-sort.directive';
export class SortColumnInfo {
  constructor(public column: string, public dir: 'asc' | 'desc') { }
}

@Directive({
  selector: '[tableSort]',
  exportAs: 'table-sort'
})
export class TableSortDirective {

  @Output('tableSort')onSort = new EventEmitter<SortColumnInfo>();
  private _sortedColumn: ColumnSortDirective;

  constructor() { }

  public get sortedColumn(): ColumnSortDirective {
    return this._sortedColumn;
  }

  public onColumnSorted(column: ColumnSortDirective) {
    if (this._sortedColumn !== column) {
      if (this._sortedColumn) {
        this._sortedColumn.sortDir = null;
      }
      this._sortedColumn = column;
    }
    this.onSort.emit(new SortColumnInfo(column.name, column.sortDir));
  }

}
