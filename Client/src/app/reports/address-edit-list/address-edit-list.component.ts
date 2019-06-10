import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { AddressEditService } from '../../services/address-edit.service';
import { HttpService } from '../../services/http-service';
import { StateCellRendererComponent } from '../address-edit/states-cell-renderer.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-address-edit-list',
  templateUrl: './address-edit-list.component.html'
})
export class AddressEditListComponent implements OnInit {
  private gridColumnApi: any;
  public defaultColDef: any;
  public rowSelection: string;
  public rowGroupPanelShow: string;
  public pivotPanelShow: string;
  public sideBar: any;
  public editType: any;
  public statusBar: any;
  public gridApi: any;
  public rowData: any;
  public columnDefs: any;
  public frameworkComponents: any;
  @ViewChild('agGrid') agGrid: AgGridNg2;

  constructor(public addressEditService: AddressEditService, private http: HttpService, private toast: ToastrService) {
    this.editType = 'fullRow';
    this.frameworkComponents = { stateCellRenderer: StateCellRendererComponent };
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
    /*this.http.getStates({}).pipe(take(1)).subscribe(data => {
        this.addressEditService.states = data.map(a => a.stateName);
        console.log(this.addressEditService.states);
      }, error => {
        console.error(error);
    });*/
    this.rowData = this.addressEditService.getPatientAddressEdit();
    this.columnDefs = [
      // { headerName: 'ClaimId', field: 'claimId', sortable: true, filter: true, checkboxSelection: true, rowDrag: true },
      { headerName: 'Patient ID', field: 'patientId', hide: true },
      { headerName: 'Last Name', field: 'lastName', sortable: true, editable: true, rowDrag: true,
        filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'First Name', field: 'firstName', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Address 1', field: 'address1', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Address 2', field: 'address2', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'City', field: 'city', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'State', field: 'stateName', cellRenderer: 'stateCellRenderer', sortable: true,
        editable: true,
        // filter: 'agTextColumnFilter',
        // filterParams: { clearButton: true},
        cellEditor: 'agRichSelectCellEditor',
        cellEditorParams: {
          values: ['ALABAMA', 'ALASKA', 'ARIZONA', 'ARKANSAS', 'CALIFORNIA', 'COLORADO', 'CONNECTICUT', 'DELAWARE', 'FLORIDA', 'GEORGIA', 'HAWAII', 'IDAHO', 'ILLINOIS', 'INDIANA', 'IOWA', 'KANSAS', 'KENTUCKY', 'LOUISIANA', 'MAINE', 'MARYLAND', 'MASSACHUSETTS', 'MICHIGAN', 'MINNESOTA', 'MISSISSIPPI', 'MISSOURI', 'MONTANA', 'NEBRASKA', 'NEVADA', 'NEW HAMPSHIRE', 'NEW JERSEY', 'NEW MEXICO', 'NEW YORK', 'NORTH CAROLINA', 'NORTH DAKOTA', 'OHIO', 'OKLAHOMA', 'OREGON', 'PENNSYLVANIA', 'RHODE ISLAND', 'SOUTH CAROLINA', 'SOUTH DAKOTA', 'TENNESSEE', 'TEXAS', 'UTAH', 'VERMONT', 'VIRGINIA', 'WASHINGTON', 'WEST VIRGINIA', 'WISCONSIN', 'WYOMING'],
          cellRenderer: 'stateCellRenderer'
        } },
      { headerName: 'Zip', field: 'postalCode', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Phone #', field: 'phoneNumber', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Email', cellEditor: 'agPopupTextCellEditor', field: 'emailAddress', sortable: true, editable: true,
        filter: 'agTextColumnFilter', filterParams: { clearButton: true} }
    ];
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
    const colId = params.column.getId();
    this.http.editPatient(params.data).subscribe(res => { this.toast.success(res.message); }, err => this.toast.error(err.message));
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
}
