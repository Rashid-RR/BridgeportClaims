import { animate, state, style, transition, trigger } from '@angular/animations';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { combineLatest, merge, Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mapTo, shareReplay, skip, startWith, switchMap, take, tap } from 'rxjs/operators';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService, PayorSearchResult } from '../../services/http-service';

declare var $: any;

@Component({
  selector: 'app-payor-search',
  templateUrl: './payor-search.component.html',
  styleUrls: ['./payor-search.component.css'],
  animations: [
    trigger('anim', [
      state('initial', style({
        width: '80px'
      })),
      state('final', style({
        width: '25rem'
      })),
      transition('initial=>final', animate('1500ms')),
      transition('final=>initial', animate('1000ms'))
    ]),
  ]
})
export class PayorSearchComponent implements OnInit {
  searchCtrl = new FormControl();
  rowSearchHits$!: Observable<PayorSearchResult[]>;
  isSearchInputExpanded = false;
  isAutocompleteOpened = false;
  isFirstSearchResultReceived = false;
  highlightedTexts: string[] = [];
  selectedType = '';
  currentPayorId = null;
  readyState = false;
  isLoading = false;
  isResult = false;
  _placeholder: string;
  set placeholder(key: string) {
    this._placeholder = key;
  }
  get placeholder() {
    return this._placeholder || 'Search Carrier';
  }
  // tslint:disable-next-line: no-output-on-prefix
  @Output() onPayorSelected = new EventEmitter();

  private autocompleteOpened$: Subject<boolean> = new Subject<boolean>();

  constructor(private http: HttpService, public claimManager: ClaimManager) {}

  ngOnInit(): void {
    // Search autocomplete.
    this.rowSearchHits$ = this.searchCtrl.valueChanges.pipe(
      debounceTime(250),
      distinctUntilChanged(),
      switchMap((val: any) => {
        if (!val) {
          val = '';
          this.isResult = false;
        }
        if (val.length > 1) {
          this.readyState = true;
          this.isLoading = true;
          const searchResult = this.http.getPayorSearch(val);
          searchResult.subscribe((payors: PayorSearchResult[]) => {
            if (payors.length > 0) { this.isLoading = false; this.isResult = false; } else { this.isLoading = false; this.isResult = true; }
          }, err => { this.isLoading = false; this.isResult = true; });
          return searchResult;
        } else {
          this.isResult = false;
          this.isLoading = false;
          this.readyState = false;
          return of([]);
        }
      }),
      tap(val => this.highlightedTexts = this.highlightPayors(val)),
      startWith([])
    );

    this.handleAutoCompleteStyles();
  }
  // tslint:disable-next-line: use-life-cycle-interface
  ngOnDestroy() { }

  private handleAutoCompleteStyles() {
    const isFirstSearchResultReceived$ = this.rowSearchHits$.pipe(
      skip(1), // skip initial state
      take(1), // unsubscribe after first result
      mapTo(true)
    );

    const isSearchCtrlValueExist$ = this.searchCtrl.valueChanges.pipe(
      startWith(null),
      map(value => !!value)
    );

    const isAutocompleteOpened$ = merge(this.autocompleteOpened$.asObservable(), isFirstSearchResultReceived$).pipe(
      shareReplay(1)
    );

    const isSearchInputExpanded$ = combineLatest([isSearchCtrlValueExist$, isAutocompleteOpened$]).pipe(
      map(([isSearchCtrlValueExist, autocompleteOpened]) => isSearchCtrlValueExist || (!isSearchCtrlValueExist && autocompleteOpened))
    );

    isFirstSearchResultReceived$.subscribe(() => this.isFirstSearchResultReceived = true);
    isSearchInputExpanded$.subscribe(x => this.isSearchInputExpanded = x);
    isAutocompleteOpened$.subscribe(x => this.isAutocompleteOpened = x);
  }

  onAutocompleteOpened() {
    this.autocompleteOpened$.next(true);
  }

  onAutocompleteClosed() {
    this.autocompleteOpened$.next(false);
  }

  private highlightPayors(rows: any[]): string[] {
    if (!rows || rows.length === 0) {
      return [];
    }

    const searchText: string = this.searchCtrl.value;
    const regexp = new RegExp(searchText, 'gi');
    // Check that prevents an error if the authentication token is expired.
    if (rows && !(rows instanceof HttpErrorResponse)) {
      return rows.map((x: any) => {
        const wholeString = x.carrier;
        return wholeString.replace(regexp, match => `<span class="highlighted">${match}</span>`);
      });
    } else {
      return [];
    }
  }

  onSearchHit(n: PayorSearchResult) {
    this.onPayorSelected.emit(n);
    this.placeholder = n.carrier;
  }
}
