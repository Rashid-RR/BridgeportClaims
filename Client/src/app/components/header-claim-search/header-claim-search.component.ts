import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { combineLatest, merge, Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mapTo, shareReplay, skip, startWith, switchMap, take, tap, filter } from 'rxjs/operators';
import { HttpService, GlobalSearchResult } from '../../services/http-service';
import { ClaimManager } from '../../services/claim-manager';
import { trigger, state, style, animate, transition } from '@angular/animations';
declare var $: any;


@Component({
  selector: 'app-header-claim-search',
  templateUrl: './header-claim-search.component.html',
  styleUrls: ['./header-claim-search.component.css'],
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
export class HeaderClaimSearchComponent implements OnInit, OnDestroy {
    searchCtrl = new FormControl();
    rowSearchHits$!: Observable<GlobalSearchResult[]>;
    isSearchInputExpanded = false;
    isAutocompleteOpened = false;
    isFirstSearchResultReceived = false;
    highlightedTexts: string[] = [];
    selectedType = '';
    placeholder: string = '';
    currentCustomerId = null;
    readyState = false;




    private autocompleteOpened$: Subject<boolean> = new Subject<boolean>();

    constructor(
      private router: Router,
      private http: HttpService,
      public claimManager: ClaimManager,
    ) {}

    ngOnInit(): void {
        // Search autocomplete.
        this.rowSearchHits$ = this.searchCtrl.valueChanges.pipe(
            // delay emits
            debounceTime(300),
            // filter((val:string) => (val.length > 2)),
            distinctUntilChanged(),
            // use switch map so as to cancel previous subscribed events, before creating new ones
            switchMap((val: any) => {
                // TODO: if the "val" comes in as a GlobalSearchResult object, hit the API again with the FullName.
                // preferably, I would like to avoid doing this to avoid hitting the API at all if it isn't necessary.
                // if (val) {
                //     return this.filterCustomerSearchList(val.FullName);
                // }
                if(!val) {
                  val = '';
                }
                if (val.length > 2) {
                    // lookup value.
                    // return this.filterCustomerSearchList(val);
                    this.readyState = true;
                    $('body').addClass('search-completed');
                    return this.http.getGlobalSearch(val, this.selectedType);
                  } else {
                    // if no value is present, return null
                    // TODO: Looking to return of(null) or something here, but that throws an error.
                    // return this.filterCustomerSearchList('');
                    $('body').removeClass('search-completed');
                    this.readyState = false;
                    return of([]);
                }
            }),
            tap(val => this.highlightedTexts = this.highlightCustomers(val)),
            startWith([])
        );

        this.handleAutoCompleteStyles();
        this.selectItem('LastName', true);
    }

    ngOnDestroy() { }

    // onSearchHit(customerId: string): void {
    //     this.searchCtrl.reset();
    //     const url = `/customer/${customerId}`;
    //     this.router.navigateByUrl(url);
    // }



    private filterCustomerSearchList(val: string): Observable<GlobalSearchResult[]> {
        return this.getSearchHits(val)
            .pipe(
                map(response => {
                    if (response instanceof Error) {
                        throw response;
                    } else {
                        const hit = response as GlobalSearchResult[];
                        if (hit) {
                            return hit;
                        } else {
                            throw new Error('Could not extract GlobalSearchResult objects from API.');
                        }
                    }
                })
            );
    }

    // Function to resolve the two possible return types "GlobalSearchResult[] | SpotError" into just the former.
    private getSearchHits(val: string): Observable<GlobalSearchResult[] | Error> {
        if (val) {
            // const action = {
            //     type: ECustomerSearchActions.SearchCustomer,
            //     payload: { lookupValue: val }
            // };
            // this.customerStore.dispatch(action);
        }
        // const hits$ = this.customerStore.pipe(select(selectSearchCustomers), untilDestroyed(this));
        // if (hits$) {
        //     return hits$;
        // }
        return of(new Error('Could not retrieve customer hits.'));
    }

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

    displayFullName(fullName?: string): string | undefined {
        return fullName ? fullName : undefined;
    }

    // private highlightCustomers(rows: GlobalSearchResult[]): string[] {
    //     if (!rows || rows.length === 0) {
    //         return [];
    //     }

    //     const searchText: string = this.searchCtrl.value;
    //     const regexp = new RegExp(searchText, 'gi');
    //     // Check that prevents an error if the authentication token is expired.
    //     if (rows && !(rows instanceof HttpErrorResponse)) {
    //         return rows.map(x => x.FullName.replace(regexp, match => `<span class="highlighted">${match}</span>`));
    //     } else {
    //         return [];
    //     }
    // }
    private highlightCustomers(rows: any[]): string[] {
      if (!rows || rows.length === 0) {
          return [];
      }

      const searchText: string = this.searchCtrl.value;
      const regexp = new RegExp(searchText, 'gi');
      // Check that prevents an error if the authentication token is expired.
      if (rows && !(rows instanceof HttpErrorResponse)) {
          return rows.map((x: any) => {
              const wholeString = x.firstName + ' ' + x.lastName + ' - ' + x.claimNumber
              return wholeString.replace(regexp, match => `<span class="highlighted">${match}</span>`);
          });
      } else {
          return [];
      }
    }


    selectItem(txt, init = false) {
      this.selectedType = txt;
      switch (this.selectedType) {
        case 'FirstName':
        this.placeholder = 'Search by First Name...';
        break;
        case 'LastName':
        this.placeholder = 'Search by Last Name...';
        break;
        case 'ClaimNumber':
        this.placeholder = 'Search by Claim Number...';
        break;
        default:
          this.placeholder = '';
          break;
        }
        if(!init) {
          this.searchCtrl.reset();
          // this.cleanSearch = false;
          // this.myControl.setValue('');
          // this.prepareSearchStream();
        }
    }

    goToClaim(id: number) {
        this.claimManager.getClaimsDataById(id);
        // this.claimManager.search({
        //   claimNumber: null, firstName: null, lastName: null,
        //   rxNumber: null, invoiceNumber: null, claimId: id
        // }, false);
        if (this.router.url !== '/main/claims') {
          this.router.navigate(['/main/claims']);
        }
    }

    onSearchHit(id) {
      this.currentCustomerId = parseInt(id) || null;
      this.searchCtrl.reset();
      this.selectItem('LastName', true);
      this.currentCustomerId && this.goToClaim(this.currentCustomerId);

      // this.myControl.setValue('');
      // this.cleanSearch = false;
      // this.prepareSearchStream();
    }
}
