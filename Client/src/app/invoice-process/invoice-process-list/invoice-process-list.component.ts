import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { GridApi } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { HttpService, InvoicesService, InvoiceProcessService } from '../../services/services.barrel';
import { AgDateFilterComponent } from '../../components/ag-date-filter/ag-date-filter.component';
import { AgPhoneNumberMaskComponent } from '../../components/ag-phone-number-mask/ag-phone-number-mask.component';
import { BridgeportDateService } from '../../services/bridgeport-date.service';
import { InvoiceProcessStateCellRendererComponent } from './invoice-process-states-cell-renderer.component';

@Component({
  selector: 'app-invoice-process-list',
  templateUrl: './invoice-process-list.component.html',
})
export class InvoiceProcessListComponent implements OnInit {
  private gridColumnApi: any;
  public defaultColDef: any;
  public rowSelection: string;
  public rowGroupPanelShow: string;
  public pivotPanelShow: string;
  public sideBar: any;
  public editType: any;
  public statusBar: any;
  public gridApi: GridApi;
  public rowData: any;
  public columnDefs: any;
  public frameworkComponents: any;
  @ViewChild('agGrid') agGrid: AgGridNg2;

  constructor(private bpDate: BridgeportDateService, public invoiceProcessService: InvoiceProcessService, private http: HttpService, private toast: ToastrService) {
    this.editType = 'fullRow';
    this.frameworkComponents = {
      stateCellRenderer: InvoiceProcessStateCellRendererComponent,
      agPhoneNumberMaskComponent: AgPhoneNumberMaskComponent,
      agDateInput: AgDateFilterComponent
    };
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

    this.rowData = this.invoiceProcessService.getInvoiceProcess().pipe(
      map((invoices: any) => {
        const inv = invoices.map((invoice) => {
          invoice['dateSubmitted'] = (invoice['dateSubmitted'] || '').substr(0, 10);
          return invoice;
        });
        inv.sort((a, b) => parseInt(this.bpDate.formatDateRaw(b.invoiceDate)) - parseInt(this.bpDate.formatDateRaw(a.invoiceDate)));
        return inv;
      })
    );

    this.columnDefs = [
      {
        headerName: 'Submitted Date',
        field: 'dateSubmitted',
        // editable: true, sortable: false,
        // filter: 'agTextColumnFilter',
        // filterParams: { clearButton: true},
        rowGroup: true,
        width: 150,
        filter: 'agDateColumnFilter',
        filterParams: {
          comparator: function(filterLocalDateAtMidnight, cellValue) {
            let dateAsString = cellValue;
            let dateParts = dateAsString.split('/');
            let cellDate = new Date(Number(dateParts[2]), Number(dateParts[1]) - 1, Number(dateParts[0]));
            if (filterLocalDateAtMidnight.getTime() === cellDate.getTime()) {
              return 0;
            }
            if (cellDate < filterLocalDateAtMidnight) {
              return -1;
            }
            if (cellDate > filterLocalDateAtMidnight) {
              return 1;
            }
          }
        }
      },
      { headerName: 'Carrier', field: 'carrier', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, rowGroup: true, width: 90, },
      { headerName: 'Patient Name', field: 'patientName', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, width: 90, },
      { headerName: 'Claim #', field: 'claimNumber', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, width: 90, },
      { headerName: 'In Queue', field: 'inQueue', editable: true, sortable: true, filter: 'agNumberColumnFilter', filterParams: { clearButton: true}, aggFunc: 'sum' },
    ];
    this.invoiceProcessService.refreshList$.subscribe(this.refreshList);
  }

  onGridReady(params): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }

  onFirstDataRendered (params): void {
    const allColumnIds = [];
    this.gridColumnApi.getAllColumns().forEach(function(column) {
      allColumnIds.push(column.colId);
    });
    this.gridColumnApi.autoSizeColumns(allColumnIds);
  }

  refreshGrid(): void {
    this.gridApi.refreshCells({});
  }

  onCellValueChanged(params: any) {
    const valueActuallyChanged = (params.oldValue !== params.newValue);
    if ( valueActuallyChanged ) {
      const colId = params.column.getId();
      this.http.editPatient(params.data).subscribe(res => { this.toast.success(res.message); }, err => this.toast.error(err.message));
    }
  }

  onBtWhich() {
    const cellDefs = this.gridApi.getEditingCells();
    if (cellDefs.length > 0) {
      const cellDef = cellDefs[0];
      console.log(
        'editing cell is: row = ' +
          cellDef.rowIndex +
          ', col = ' +
          cellDef.column.getId() +
          ', floating = ' +
          cellDef.floating
      );
    } else {
      console.log('no cells are editing');
    }
  }

  refreshList = (action) => {
    if (!action) {return; }
    this.gridApi.setFilterModel(null);
    this.gridApi.onFilterChanged();
    this.gridColumnApi.resetColumnState();
  }
}
