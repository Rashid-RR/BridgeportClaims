import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridNg2 } from 'ag-grid-angular/dist/agGridNg2';
import { QueryBuilderService } from '../../services/query-builder.service';

@Component({
  selector: 'app-claims-data-list',
  templateUrl: './claims-data-list.component.html',
  styleUrls: ['./claims-data-list.component.css']
})
export class ClaimsDataListComponent implements OnInit {
  @ViewChild('agGrid') agGrid: AgGridNg2;
  columnDefs = [
    { headerName: 'Make', field: 'make', sortable: true, filter: true, rowGroup: true, checkboxSelection: true },
    { headerName: 'Model', field: 'model', sortable: true, filter: true },
    { headerName: 'Price', field: 'price', sortable: true, filter: true }
  ];
  autoGroupColumnDef = {
    headerName: 'Model',
    field: 'model',
    cellRenderer: 'agGroupCellRenderer',
    cellRendererParams: {
        checkbox: true
    }
};

  rowData: any;
  constructor(private queryBuilderSvc: QueryBuilderService) {}

  ngOnInit(): void {
    this.rowData = this.queryBuilderSvc.fetchQueryBuilderReport();
  }

  getSelectedRows() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const selectedData = selectedNodes.map(node => node.data);
    const selectedDataStringPresentation = selectedData.map(node => node.make + ' ' + node.model).join(', ');
    console.log(`Selected nodes: ${selectedDataStringPresentation}`);
  }
}
