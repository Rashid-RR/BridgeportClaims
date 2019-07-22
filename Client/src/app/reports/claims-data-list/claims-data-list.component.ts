import { Subscription } from 'rxjs';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { QueryBuilderService } from '../../services/query-builder.service';

@Component({
  selector: 'app-claims-data-list',
  templateUrl: './claims-data-list.component.html'
})
export class ClaimsDataListComponent implements OnInit, OnDestroy {
  private sub!: Subscription;
  public defaultColDef: any;
  public rowSelection: string;
  public rowGroupPanelShow: string;
  public pivotPanelShow: string;
  public sideBar: any;
  public statusBar: any;
  public gridApi: any;
  private gridColumnApi: any;

  @ViewChild('agGrid') agGrid: AgGridNg2;
  columnDefs = [
    // { headerName: 'ClaimId', field: 'claimId', sortable: true, filter: true, checkboxSelection: true, rowDrag: true },
    // { headerName: 'PrescriptionId', field: 'prescriptionId', sortable: true, filter: true },
    { headerName: 'Group Name', field: 'groupName', editable: false, sortable: true, rowDrag: true,
     filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Pharmacy', editable: false, field: 'pharmacy', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'State', editable: false, field: 'stateCode', sortable: true, filter: true, filterParams: { clearButton: true} },
    { headerName: 'Submitted', editable: false, field: 'dateSubmitted', sortable: true, filter: 'agDateColumnFilter',
      filterParams: {
        clearButton: true,
        comparator: function(filterLocalDateAtMidnight, cellValue) {
          const dateAsString = cellValue;
          if (dateAsString == null) {
            return -1;
          }
          const dateParts = dateAsString.split('/');
          const cellDate = new Date(Number(dateParts[2]), Number(dateParts[0]) - 1, Number(dateParts[1]));
          if (filterLocalDateAtMidnight.toString() === cellDate.toString()) {
            return 0;
          }
          if (cellDate < filterLocalDateAtMidnight) {
            return -1;
          }
          if (cellDate > filterLocalDateAtMidnight) {
            return 1;
          }
        },
        browserDatePicker: true
      }
    },
    { headerName: 'Billed', editable: false, field: 'billed', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Payable', editable: false, field: 'payable', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Collected', editable: false, field: 'collected', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Prescriber', editable: false, field: 'prescriber', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Patient Last', editable: false, field: 'patientLast', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Patient First', editable: false, field: 'patientFirst', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Claim #', field: 'claimNumber', editable: false, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Attorney Managed', field: 'isAttorneyManaged', editable: false, sortable: true, filter: true, filterParams: { clearButton: true} },
    { headerName: 'Attorney Name', field: 'attorneyName', sortable: true, editable: false, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Label', field: 'labelName', sortable: true, filter: 'agTextColumnFilter', editable: false, filterParams: { clearButton: true} },
    { headerName: 'Rx #', field: 'rxNumber', editable: false, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Rx Date', field: 'dateFilled', editable: false, sortable: true, filter: 'agDateColumnFilter',
      filterParams: {
        clearButton: true,
        comparator: function(filterLocalDateAtMidnight, cellValue) {
          const dateAsString = cellValue;
          if (dateAsString == null) {
            return -1;
          }
          const dateParts = dateAsString.split('/');
          const cellDate = new Date(Number(dateParts[2]), Number(dateParts[0]) - 1, Number(dateParts[1]));
          if (filterLocalDateAtMidnight.toString() === cellDate.toString()) {
            return 0;
          }
          if (cellDate < filterLocalDateAtMidnight) {
            return -1;
          }
          if (cellDate > filterLocalDateAtMidnight) {
            return 1;
          }
        },
        browserDatePicker: true
      }
    },
    { headerName: 'NDC', field: 'ndc', editable: false, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Invoice #', field: 'invoiceNumber', editable: false, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Invoice Date', field: 'invoiceDate', sortable: true, editable: false, filter: 'agDateColumnFilter',
      filterParams: {
        clearButton: true,
        comparator: function(filterLocalDateAtMidnight, cellValue) {
          const dateAsString = cellValue;
          if (dateAsString == null) {
            return -1;
          }
          const dateParts = dateAsString.split('/');
          const cellDate = new Date(Number(dateParts[2]), Number(dateParts[0]) - 1, Number(dateParts[1]));
          if (filterLocalDateAtMidnight.toString() === cellDate.toString()) {
            return 0;
          }
          if (cellDate < filterLocalDateAtMidnight) {
            return -1;
          }
          if (cellDate > filterLocalDateAtMidnight) {
            return 1;
          }
        },
        browserDatePicker: true
      }
    }
  ];

  rowData: any;
  constructor(public queryBuilderSvc: QueryBuilderService) {
    this.defaultColDef = {
      editable: true,
      enableRowGroup: true,
      enablePivot: true,
      enableValue: true,
      sortable: true,
      resizable: true,
      filter: true
    };
    this.rowSelection = 'multiple';
    this.rowGroupPanelShow = 'always';
    this.pivotPanelShow = 'always';
    this.sideBar = {
      toolPanels: [
          {
              id: 'columns',
              labelDefault: 'Columns',
              labelKey: 'columns',
              iconKey: 'columns',
              toolPanel: 'agColumnsToolPanel',
          },
          {
              id: 'filters',
              labelDefault: 'Filters',
              labelKey: 'filters',
              iconKey: 'filter',
              toolPanel: 'agFiltersToolPanel',
          }
      ],
      defaultToolPanel: 'columns'
    };

    this.statusBar = {
      statusPanels: [
        { statusPanel: 'agTotalRowCountComponent', align: 'left' },
        { statusPanel: 'agFilteredRowCountComponent', align: 'left' },
        { statusPanel: 'agSelectedRowCountComponent', align: 'left' },
        { statusPanel: 'agAggregationComponent', align: 'left' }
      ]
    };
  }

  ngOnInit(): void {
    this.rowData = this.queryBuilderSvc.fetchQueryBuilderReport();
    this.sub = this.queryBuilderSvc.refreshList$.subscribe(this.refreshList);
  }

  ngOnDestroy(): void {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onFirstDataRendered(params: any): void {
    const allColumnIds = [];
    this.gridColumnApi.getAllColumns().forEach((column: any) => {
      allColumnIds.push(column.colId);
    });
    this.gridColumnApi.autoSizeColumns(allColumnIds);
  }

  onGridReady(params): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }

  refreshList = (action) => {
    if (!action) {
      return;
    }
    this.gridApi.setFilterModel(null);
    this.gridApi.onFilterChanged();
    this.gridColumnApi.resetColumnState();
  }
}
