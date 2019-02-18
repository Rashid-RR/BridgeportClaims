import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as d3 from 'd3';
import swal from 'sweetalert2';
import { HttpService } from '../../services/http-service';
import { DecisionTreeService } from '../../services/services.barrel';
import { ProfileManager } from '../../services/profile-manager';
import { ToastrService } from 'ngx-toastr';

declare var $: any;

var zoomX = 0, zoomY = 0, zoomZ = 1;

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit {
  zoomLevel = 1;
  over: boolean[];
  claimId: string;
  constructor(private toast:ToastrService,private route: ActivatedRoute, public ds: DecisionTreeService, private profileManager: ProfileManager, private http: HttpService) {
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
    $('body, html, .wrapper').css('height', '100%');
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
      setTimeout(() => {
        this.startDragging();
      }, 1000);
    }
  }
  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.treeId) {
        this.ds.parentTreeId = params.treeId;
      }
      if (params.claimId) {
        this.ds.claimId = params.claimId;
        this.claimId = params.claimId;
        this.ds.loading = true;
        this.http.selectTree(params.treeId,params.claimId)
        .subscribe((resp: any) => {
          this.toast.success(resp.message)
          this.ds.sessionId = resp.sessionId;
          this.ds.loading = false;
          d3.select('svg').attr('transform', `translate(${zoomX},${zoomY})scale(${zoomZ})`);
        }, err => {
          this.ds.loading = false
            try {
              const error = err.error;
            } catch (e) { }
          });
      }
    });
    try { swal.clickCancel() } catch (e) { };
  }
  get allowed(): boolean {
    return  (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  startDragging() {
    d3
    .select('svg')
    .call(
      d3.zoom()
      // .extent([[this.ds.ma`rgin.left, this.ds.margin.top], [this.ds.width, this.ds.height]])
      // .scaleExtent([1, 1])
      .on('zoom', () => {
          zoomX = d3.event.transform.x + (this.ds.margin.left || 0);
          // console.log('zoomX', zoomX);
          zoomY = d3.event.transform.y + (this.ds.margin.top || 0);
          // console.log('zoomY', zoomY);
          d3.select('svg').attr('transform', `translate(${zoomX},${zoomY})scale(${zoomZ})`);
      })
    );
    // d3.select('#slider8').call(d3.slider().value(50).orientation('vertical'));
  }
  handleZoomLevel(x) {
    // alert(x);
    zoomZ = x;
    d3.select('svg').attr('transform', `translate(${zoomX},${zoomY})scale(${zoomZ})`);
  }
}
