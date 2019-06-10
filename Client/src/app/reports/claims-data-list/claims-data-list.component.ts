import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { QueryBuilderService } from '../../services/query-builder.service';

@Component({
  selector: 'app-claims-data-list',
  templateUrl: './claims-data-list.component.html'
})
export class ClaimsDataListComponent implements OnInit {
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
    { headerName: 'Group Name', field: 'groupName', sortable: true, rowDrag: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Pharmacy', field: 'pharmacy', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'State', field: 'stateCode', sortable: true, filter: true, filterParams: { clearButton: true} },
    { headerName: 'Submitted', field: 'dateSubmitted', sortable: true, filter: 'agDateColumnFilter',
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
    { headerName: 'Billed', field: 'billed', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Payable', field: 'payable', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Collected', field: 'collected', sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Prescriber', field: 'prescriber', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Patient Last', field: 'patientLast', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Patient First', field: 'patientFirst', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Claim #', field: 'claimNumber', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Attorney Managed', field: 'isAttorneyManaged', sortable: true, filter: true, filterParams: { clearButton: true} },
    { headerName: 'Attorney Name', field: 'attorneyName', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Label', field: 'labelName', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Rx #', field: 'rxNumber', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Rx Date', field: 'dateFilled', sortable: true, filter: 'agDateColumnFilter',
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
    { headerName: 'NDC', field: 'ndc', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Invoice #', field: 'invoiceNumber', sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    { headerName: 'Invoice Date', field: 'invoiceDate', sortable: true, filter: 'agDateColumnFilter',
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
  /*autoGroupColumnDef = {
    headerName: 'GroupName',
    field: 'groupName',
    cellRenderer: 'agGroupCellRenderer',
    cellRendererParams: {
        checkbox: true
    }
  };*/

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
    this.queryBuilderSvc.refreshList$.subscribe(this.refreshList);
  }

  /*getSelectedRows() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const selectedData = selectedNodes.map(node => node.data);
    const selectedDataStringPresentation = selectedData.map(node => node.make + ' ' + node.model).join(', ');
    console.log(`Selected nodes: ${selectedDataStringPresentation}`);
  }*/

  onGridReady(params): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }

  refreshList = (action) => {
    if(!action){return;}
    this.gridApi.setFilterModel(null);
    this.gridApi.onFilterChanged();
    this.gridColumnApi.resetColumnState();
  }
}
