import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AutoComplete } from './auto-complete';
import { Subject } from 'rxjs';
import { HttpService, AccountReceivableService } from '../services/services.barrel';
declare var $: any;
/**
 * show a selected date in monthly calendar
 * Each filteredList item has the following property in addition to data itself
 *   1. displayValue as string e.g. Allen Kim
 *   2. dataValue as any e.g.
 */
@Component({
  selector: 'auto-complete',
  template: `
  <div #autoCompleteContainer class="auto-complete">
    <!-- keyword input -->
    <input *ngIf="showInputTag"
           #autoCompleteInput class="keyword"
           [attr.autocomplete]="autocomplete ? 'null' : 'off'"
           placeholder="{{placeholder}}"
           (focus)="showDropdownList($event)"
           (blur)="blurHandler($event)"
           (keydown)="inputElKeyHandler($event)"
           (input)="reloadListInDelay($event)"
           [(ngModel)]="keyword" />
    <!-- dropdown that user can select -->
    <ul *ngIf="dropdownVisible" [class.empty]="emptyList">
      <li *ngIf="isLoading && loadingTemplate" class="loading" [innerHTML]="loadingTemplate"></li>
      <li *ngIf="isLoading && !loadingTemplate" class="loading">{{loadingText}}</li>
      <li *ngIf="minCharsEntered && !isLoading && !filteredList.length"
           (mousedown)="selectOne('')"
           class="no-match-found">{{noMatchFoundText || 'No Result Found'}}</li>
      <li *ngIf="blankOptionText && filteredList.length"
          (mousedown)="selectOne('')"
          class="blank-item">{{blankOptionText}}</li>
      <li class="item"
          *ngFor="let item of filteredList; let i=index"
          (mousedown)="selectOne(item)"
          [ngClass]="{selected: i === itemIndex}"
          [innerHtml]="autoComplete.getFormattedListItem(item)">
      </li>
    </ul>
  </div>`,
  providers: [AutoComplete, AccountReceivableService],
  styles: [`
  @keyframes slideDown {
    0% {
      transform:  translateY(-10px);
    }
    100% {
      transform: translateY(0px);
    }
  }
  auto-complete{
    max-height:55vh;
    border-bottom:1px solid #ddd;
    overflow-y:auto;
  }
  .auto-complete {
    background-color: transparent;
  }
  .auto-complete > input {
    outline: none;
    border: 0;
    padding: 2px;
    box-sizing: border-box;
    background-clip: content-box;
  }
  .auto-complete > ul {
    background-color: #fff;
    margin: 0;
    width : 100%;
    overflow-y: auto;
    list-style-type: none;
    padding: 0;
    border: 1px solid #ccc;
    box-sizing: border-box;
    animation: slideDown 0.1s;
  }
  .auto-complete > ul.empty {
    display: none;
  }
  .auto-complete > ul li {
    padding: 2px 5px;
    border-bottom: 1px solid #eee;
  }
  .auto-complete > ul li.selected {
    background-color: #ccc;
  }
  .auto-complete > ul li:last-child {
    border-bottom: none;
  }
  .auto-complete > ul li:hover {
    background-color: #ccc;
  }`
  ],
  encapsulation: ViewEncapsulation.None
})
export class AutoCompleteComponent implements OnInit {
;
;

  get emptyList(): boolean {
    return !(
      this.isLoading ||
      (this.minCharsEntered && !this.isLoading && !this.filteredList.length) ||
      (this.filteredList.length)
    );
  }

  /**
   * public input properties
   */
  @Input('autocomplete') autocomplete = false;
  @Input('list-formatter') listFormatter: (arg: any) => string;
  @Input('source') source: any;
  @Input('path-to-data') pathToData: string;
  @Input('min-chars') minChars = 0;
  @Input('placeholder') placeholder: string;
  @Input('http-method') httpMethod = 'post';
  @Input('service') service: any;
  @Input('method') method: any;
  @Input('blank-option-text') blankOptionText: string;
  @Input('no-match-found-text') noMatchFoundText: string;
  @Input('accept-user-input') acceptUserInput = true;
  @Input('loading-text') loadingText = 'Loading';
  @Input('loading-template') loadingTemplate = null;
  @Input('max-num-list') maxNumList: number;
  @Input('show-input-tag') showInputTag = true;
  @Input('exactMatch') exactMatch = true;
  @Input('show-dropdown-on-init') showDropdownOnInit = false;
  @Input('tab-to-select') tabToSelect = true;
  @Input('match-formatted') matchFormatted = false;
  @Input('auto-select-first-item') autoSelectFirstItem = false;
  @Input('select-on-blur') selectOnBlur = true;
  @Input('autocomplete-dropdown-event-emitter') showDropDown = new Subject<any>();

  @Output() valueSelected = new EventEmitter();
  @Output() customSelected = new EventEmitter();
  @Output() textEntered = new EventEmitter();
  @ViewChild('autoCompleteInput') autoCompleteInput: ElementRef;
  @ViewChild('autoCompleteContainer') autoCompleteContainer: ElementRef;

  el: HTMLElement;           // this component  element `<auto-complete>`
  dropdownVisible = false;
  isLoading = false;

  minCharsEntered = false;
  itemIndex: number = null;
  keyword: string;
  public filteredList: any[] = [];

  private delay = (function () {
    let timer:any = 0;
    return function (callback: any, ms: number) {
      clearTimeout(timer);
      timer = setTimeout(callback, ms);
    };
  })();

  isSrcArr(): boolean {
    return (this.source.constructor.name === 'Array');
  }

  /**
   * constructor
   */
  constructor(
    elementRef: ElementRef,
    public autoComplete: AutoComplete,
    public ar: AccountReceivableService,
    private http: HttpService,
  ) {
    this.el = elementRef.nativeElement;
  }

  /**
   * user enters into input el, shows list to select, then select one
   */
  ngOnInit(): void {
    this.autoComplete.source = this.source;
    this.autoComplete.pathToData = this.pathToData;
    this.autoComplete.listFormatter = this.listFormatter;
    if (this.autoSelectFirstItem) {
      this.itemIndex = 0;
    }
    setTimeout(() => {
      if (this.autoCompleteInput) {
        this.autoCompleteInput.nativeElement.focus();
      }
      if (this.showDropdownOnInit) {
        this.showDropdownList({ target: { value: '' } });
      }
    });
  }

  reloadListInDelay = (evt: any): void => {
    const delayMs = this.isSrcArr() ? 10 : 500;
    const keyword = evt.target.value;

    // executing after user stopped typing
    this.delay(() => this.reloadList(keyword), delayMs);
  }

  showDropdownList(event: any): void {
    this.dropdownVisible = true;
    this.reloadList(event.target.value);
  }

  hideDropdownList(): void {
    this.dropdownVisible = false;
  }

  findItemFromSelectValue(selectText: string): any {
    const matchingItems = this.filteredList
      .filter(item => ('' + item) === selectText);
    return matchingItems.length ? matchingItems[0] : null;
  }

  reloadList(keyword: string): void {

    this.filteredList = [];
    if (keyword.length < (this.minChars || 0)) {
      this.minCharsEntered = false;
      return;
    } else {
      this.minCharsEntered = true;
    }

    if (this.isSrcArr()) {    // local source
      this.isLoading = false;
      this.filteredList = this.autoComplete.filter(this.source, keyword, this.matchFormatted, this.httpMethod);
      if (this.maxNumList) {
        this.filteredList = this.filteredList.slice(0, this.maxNumList);
      }

    } else {                 // remote source
      this.isLoading = true;
      this.autoComplete.httpMethod = this.httpMethod;
      this.autoComplete.exactMatch = this.exactMatch;
      if (this.service && this.method) {
        this.autoComplete.getRemoteData(keyword, this.http.headers).subscribe(resp => {
          this.filteredList = resp ? (<any>resp) : [];
          if (this.maxNumList) {
            this.filteredList = this.filteredList.slice(0, this.maxNumList);
          }
          // select if only one result is returned
          if (this.filteredList.length === 1) {
            this.selectOne(this.filteredList[0]);
          }
          const wevent = document.createEvent('Event');
          if (this.source.indexOf('group-name') > -1) {
            wevent.initEvent('filteredList', true, true);
            wevent['filteredList'] = this.filteredList;
          } else if (this.source.indexOf('pharmacy-name') > -1) {
            wevent.initEvent('pharmacyList', true, true);
            wevent['pharmacyList'] = this.filteredList;
          }
          try {
            window.dispatchEvent(wevent);
          } catch (e) { }
        },
          error => null,
          () => this.isLoading = false // complete
        );

      } else if (typeof this.source === 'function') {
        // custom function that returns observable
        this.source(keyword).subscribe(
          resp => {
            if (this.pathToData) {
              const paths = this.pathToData.split('.');
              paths.forEach(prop => resp = resp[prop]);
            }

            this.filteredList = resp;
            if (this.maxNumList) {
              this.filteredList = this.filteredList.slice(0, this.maxNumList);
            }
          },
          error => null,
          () => this.isLoading = false // complete
        );
      } else {
        // remote source
        this.autoComplete.getRemoteData(keyword).subscribe(resp => {
          this.filteredList = resp ? (<any>resp) : [];
          if (this.maxNumList) {
            this.filteredList = this.filteredList.slice(0, this.maxNumList);
          }
        },
          error => null,
          () => this.isLoading = false // complete
        );
      }
    }
  }

  selectOne(data: any) {
    if (!!data || data === '') {
      this.valueSelected.emit(data);
    } else {
      this.customSelected.emit(this.keyword);
    }
  }
  enterText(data: any) {
    this.textEntered.emit(data);
  }

  blurHandler(evt: any) {
    if (this.selectOnBlur) {
      this.selectOne(this.filteredList[this.itemIndex]);
    }

    this.hideDropdownList();
  }
  inputElKeyHandler = (evt: any) => {
    const totalNumItem = this.filteredList.length;

    switch (evt.keyCode) {
      case 27: // ESC, hide auto complete
        this.selectOne(undefined);
        break;

      case 38: // UP, select the previous li el
        if (0 === totalNumItem) {
          return;
        }
        this.itemIndex = (totalNumItem + this.itemIndex - 1) % totalNumItem;
        this.scrollToView(this.itemIndex);
        break;

      case 40: // DOWN, select the next li el or the first one
        if (0 === totalNumItem) {
          return;
        }
        this.dropdownVisible = true;
        let sum = this.itemIndex;
        sum = (this.itemIndex === null) ? 0 : sum + 1;
        this.itemIndex = (totalNumItem + sum) % totalNumItem;
        this.scrollToView(this.itemIndex);
        break;

      case 13: // ENTER, choose it!!
        this.selectOne(this.filteredList[this.itemIndex]);
        evt.preventDefault();
        break;

      case 9: // TAB, choose if tab-to-select is enabled
        if (this.tabToSelect) {
          this.selectOne(this.filteredList[this.itemIndex]);
        }
        break;
    }
  }

  scrollToView(index) {
    const container = this.autoCompleteContainer.nativeElement;
    const ul = container.querySelector('ul');
    const li = ul.querySelector('li');  // just sample the first li to get height
    const liHeight = li.offsetHeight;
    const scrollTop = ul.scrollTop;
    const viewport = scrollTop + ul.offsetHeight;
    const scrollOffset = liHeight * index;
    if (scrollOffset < scrollTop || (scrollOffset + liHeight) > viewport) {
      ul.scrollTop = scrollOffset;
    }
  }

}
