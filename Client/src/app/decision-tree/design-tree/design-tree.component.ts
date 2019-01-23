import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as d3 from 'd3';
import { HttpService } from '../../services/http-service';
import { ToastrService } from 'ngx-toastr';
import { DialogService } from 'ng2-bootstrap-modal';
import { DecisionTreeService } from 'app/services/services.barrel';

declare var $: any;

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit {
  over: boolean[];
  constructor(private route: ActivatedRoute, private ds: DecisionTreeService, private toast: ToastrService, private dialogService: DialogService, private http: HttpService) {
    this.over = new Array(1);
    this.over.fill(false);

  }
  collapse(d) {
    if (d.children) {
      d._children = d.children
      d._children.forEach(c => this.collapse(c));
      d.children = null
    }
  }
  ngAfterViewInit() {
    if (this.ds.parentTreeId) {
      this.ds.loading = true;
      this.http.getTree({ parentTreeId: this.ds.parentTreeId })
        .subscribe((result: any) => {
          this.ds.loading = false;
          this.ds.treeData = result;
          this.ds.root = d3.hierarchy(this.ds.treeData, (d) => { return d.children; });
          this.ds.root.x0 = this.ds.height / 2;
          this.ds.root.y0 = 20;
          this.ds.root.y = 20;
          this.ds.svg = d3.select('svg')
            .style("width", this.ds.width)
            .style("height", this.ds.height)
            .attr("transform", "translate("
              + this.ds.margin.left + "," + this.ds.margin.top + ")");
          (this.ds.root.children || []).forEach(c => this.collapse(c));
          this.ds.update(this.ds.root);
        }, err => {
          this.ds.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        });
    }
  }
  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.treeId) {
        this.ds.parentTreeId = params.treeId;
      }
    });
  }

}
