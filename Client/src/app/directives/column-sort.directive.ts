import { Directive, ViewContainerRef, OnInit, OnChanges, TemplateRef,
  AfterViewInit, Input, Output, EventEmitter, SimpleChange, SimpleChanges,
  HostListener, HostBinding } from '@angular/core';
import { TableSortDirective } from './table-sort.directive';

const SORT_DIRS_LOOP = ['asc', 'desc'];
type SortDirs = 'asc' | 'desc' | null;
@Directive({
  selector: '[columnSort]',
})
export class ColumnSortDirective implements OnInit, AfterViewInit {
  template: any;
  @Input('columnSort') name: string;
  @Input() sortDir: SortDirs = null;
  @Input('sort-table') table: TableSortDirective;
  headElement: HTMLTableHeaderCellElement;

  constructor(private viewContainerRef: ViewContainerRef) { }
  
  ngOnInit() {
    if (this.template && this.viewContainerRef && this.template.elementRef) {
      this.viewContainerRef.createEmbeddedView(this.template, { data: this });
    }
  }

  ngAfterViewInit() {
    this.headElement = this.viewContainerRef.element.nativeElement;
  }

  @HostBinding('attr.class') get style(): string {
    const results: string[] = ['fa'];
    if (this.sortDir == null) {
      results.push('fa-sort');
    } else if (this.sortDir === 'asc') {
      results.push('fa-sort-asc');
    } else {
      results.push('fa-sort-desc');
    }
    return results.join(' ');
  }

  @HostListener('click', ['$event']) onClick(e: Event) {
    let new_dir: SortDirs;
    const index = SORT_DIRS_LOOP.indexOf(this.sortDir);
    new_dir = SORT_DIRS_LOOP[(index + 1) % SORT_DIRS_LOOP.length] as SortDirs;
    this.sortDir = new_dir;
    if (this.table) {
      this.table.onColumnSorted(this);
    } else {
      
    }
  }

}
