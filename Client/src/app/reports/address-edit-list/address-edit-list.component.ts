import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { AddressEditService, AddressEditState } from '../../services/address-edit.service';

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
  public statusBar: any;
  public gridApi: any;
  public rowData: any;
  public columnDefs: any;
  public states: AddressEditState[];
  public stateNames: string[] = [];
  @ViewChild('agGrid') agGrid: AgGridNg2;

  constructor(public addressEditService: AddressEditService) {
    this.states = this.addressEditService.getStates();
    for (const st of this.states) {
      this.stateNames.push(st.stateName);
    }
    this.columnDefs = [
      // { headerName: 'ClaimId', field: 'claimId', sortable: true, filter: true, checkboxSelection: true, rowDrag: true },
      { headerName: 'PatientId', field: 'patientId', hidden: true },
      { headerName: 'Last Name', field: 'lastName', sortable: true, editable: true, rowDrag: true,
        filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'First Name', field: 'firstName', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Address 1', field: 'address1', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Address 2', field: 'address2', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'City', field: 'city', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'State', field: 'stateCode', cellRenderer: 'stateCellRenderer', sortable: true,
        editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true}, cellEditor: 'agRichSelectCellEditor',
        cellEditorParams: {
          values: this.stateNames,
          cellRenderer: 'stateCellRenderer'
        } },
      { headerName: 'Zip', field: 'postalCode', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Phone #', field: 'phoneNumber', sortable: true, editable: true, filter: 'agTextColumnFilter', filterParams: { clearButton: true} },
      { headerName: 'Email', cellEditor: 'agPopupTextCellEditor', field: 'emailAddress', sortable: true, editable: true,
        filter: 'agTextColumnFilter', filterParams: { clearButton: true} }
    ];
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
    this.rowData = this.addressEditService.getPatientAddressEdit();
  }

  onGridReady(params): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    params.api.sizeColumnsToFit();
  }

  onCellValueChanged(params) {
    const colId = params.column.getId();
    if (colId === 'patientId') {
      let selectedLastName = params.data.lastName;
      let selectedFirstName = params.data.firstName;
    }
  }
}
