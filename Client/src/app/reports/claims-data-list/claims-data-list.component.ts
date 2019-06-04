import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { QueryBuilderService } from '../../services/query-builder.service';

@Component({
  selector: 'app-claims-data-list',
  templateUrl: './claims-data-list.component.html',
  styleUrls: ['./claims-data-list.component.css']
})
export class ClaimsDataListComponent implements OnInit {
  public defaultColDef: any;
  public rowSelection: string;
  public rowGroupPanelShow: string;
  public pivotPanelShow: string;
  private gridApi;
  private gridColumnApi;

  @ViewChild('agGrid') agGrid: AgGridNg2;
  columnDefs = [
    // { headerName: 'ClaimId', field: 'claimId', sortable: true, filter: true, checkboxSelection: true, rowDrag: true },
    // { headerName: 'PrescriptionId', field: 'prescriptionId', sortable: true, filter: true },
    // { headerName: 'PrescriptionPaymentId', field: 'prescriptionPaymentId', sortable: true, filter: true },
    { headerName: 'Pharmacy', field: 'pharmacy', sortable: true, filter: true, rowDrag: true },
    { headerName: 'State', field: 'stateCode', sortable: true, filter: true },
    { headerName: 'Submitted', field: 'dateSubmitted', sortable: true, filter: true },
    { headerName: 'Billed', field: 'billed', sortable: true, filter: true },
    { headerName: 'Payable', field: 'payable', sortable: true, filter: true },
    { headerName: 'Collected', field: 'collected', sortable: true, filter: true },
    { headerName: 'Prescriber', field: 'prescriber', sortable: true, filter: true },
    { headerName: 'Patient Last', field: 'patientLast', sortable: true, filter: true },
    { headerName: 'Patient First', field: 'patientFirst', sortable: true, filter: true },
    { headerName: 'Claim #', field: 'claimNumber', sortable: true, filter: true },
    { headerName: 'Attorney Managed', field: 'isAttorneyManaged', sortable: true, filter: true },
    { headerName: 'Attorney Name', field: 'attorneyName', sortable: true, filter: true }
  ];
  autoGroupColumnDef = {
    headerName: 'GroupName',
    field: 'groupName',
    cellRenderer: 'agGroupCellRenderer',
    cellRendererParams: {
        checkbox: true
    }
  };

  rowData: any;
  constructor(private queryBuilderSvc: QueryBuilderService) {
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
  }

  ngOnInit(): void {
    this.rowData = this.queryBuilderSvc.fetchQueryBuilderReport();
  }

  getSelectedRows() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const selectedData = selectedNodes.map(node => node.data);
    const selectedDataStringPresentation = selectedData.map(node => node.make + ' ' + node.model).join(', ');
    console.log(`Selected nodes: ${selectedDataStringPresentation}`);
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    this.gridApi.api.sizeColumnsToFit();
  }
}
