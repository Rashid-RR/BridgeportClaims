import { map } from 'rxjs/operators';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { InvoicesService, HttpService } from '../../services/services.barrel';
import { StateCellRendererComponent } from '../address-edit/states-cell-renderer.component';
import { ToastrService } from 'ngx-toastr';
import { GridApi } from 'ag-grid-community';
import { AgPhoneNumberMaskComponent } from './../../components/ag-phone-number-mask/ag-phone-number-mask.component';
import { InvoiceScreen } from './../../models/invoice.model';

@Component({
  selector: 'app-invoices-list',
  templateUrl: './invoices-list.component.html',
})
export class InvoicesListComponent implements OnInit {
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

  constructor(public invoicesService: InvoicesService, private http: HttpService, private toast: ToastrService) {
    this.editType = 'fullRow';
    this.frameworkComponents = { stateCellRenderer: StateCellRendererComponent, agPhoneNumberMaskComponent: AgPhoneNumberMaskComponent };
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

    this.rowData = this.invoicesService.getInvoices().pipe(
      map((invoices: any) => {
        return invoices.map((invoice) => {
          invoice['invoiceDate'] = (invoice['invoiceDate']||'').substr(0, 8);
          return invoice;
        });
      })
    );

    this.columnDefs = [
      { headerName: 'Inv Date', field: 'invoiceDate', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, rowGroup: true, width: 120, },
      { headerName: 'Carrier', field: 'carrier', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, rowGroup: true, width: 90, },
      { headerName: 'Patient Name', field: 'patientName', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, width: 90, },
      { headerName: 'Claim #', field: 'claimNumber', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, width: 90, },
      { headerName: 'Invoice Count', field: 'invoiceCount', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Script Count', field: 'scriptCount', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Printed', field: 'printed', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Total Printed', field: 'totalToPrint', editable: true, sortable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
    ];
    this.invoicesService.refreshList$.subscribe(this.refreshList);
  }

  onGridReady(params): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    params.api.sizeColumnsToFit();
  }

  refreshGrid(): void {
    this.gridApi.refreshCells({});
  }

  onCellValueChanged(params: any) {
    const valueActuallyChanged = (params.oldValue !== params.newValue);
    if( valueActuallyChanged ) {
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
    if(!action){return;}
    this.gridApi.setFilterModel(null);
    this.gridApi.onFilterChanged();
    this.gridColumnApi.resetColumnState();
  }
}
