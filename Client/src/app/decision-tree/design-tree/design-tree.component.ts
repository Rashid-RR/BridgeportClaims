import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as d3 from 'd3';
import swal from 'sweetalert2';
import { HttpService } from '../../services/http-service';
import { DecisionTreeService } from '../../services/services.barrel';
import { ProfileManager } from '../../services/profile-manager';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';

declare var $: any;

var zoomX = 0, zoomY = 0, zoomZ = 1, clickOriginX = null, clickOriginY = null;

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit {
  zoomLevel = 1;
  over: boolean[];
  claimId: string;
  rootNode: string;
  rootTreeId: any;
  leafText: string;
  leafTreeId: any;
  rootText: string
  @ViewChild('episodeSwal') private episodeSwal: SwalComponent;
  constructor(
    public readonly swalTargets: SwalPartialTargets,private route: ActivatedRoute, public ds: DecisionTreeService, private profileManager: ProfileManager, private http: HttpService) {
    this.over = new Array(1);
    this.over.fill(false);


  }
  episode() {
    this.episodeSwal.show().then((r) => {

    });
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
          this.ds.svg = d3.select('#decisionTree')
            .style("width", this.ds.width)
            .style("height", this.ds.height)
            .attr("transform", "translate("
              + this.ds.margin.left + "," + this.ds.margin.top + ")");
          (this.ds.root.children || []).forEach(c => this.collapse(c));
          if (this.claimId) {
            this.ds.treeData.picked = true;
          }
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
      }
    });
    try { swal.clickCancel() } catch (e) { };
    this.ds.onExperienceEnd.subscribe(exp => {
      this.rootText = exp.root.nodeName;
      this.rootTreeId = exp.root.treeId;
      this.leafText = exp.leaf.nodeName;
      this.leafTreeId = exp.leaf.treeId;
      this.ds.episodeForm.reset();
      this.ds.episodeForm.patchValue({
        claimId: this.claimId,
        episodeText: '',
        pharmacyNabp: null,
        episodeTypeId: null,
        rootTreeId : exp.root.treeId,
        leafTreeId : exp.leaf.treeId
      });
      this.episode();
    });
  }
  get allowed(): boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  startDragging() {
    d3.select('#decisionTree')
      .call(
        d3.drag()
          .on('start', () => {
            clickOriginX = d3.event.x;
            clickOriginY = d3.event.y;
          })
          .on('drag', function (d) {
            zoomX = d3.event.x - clickOriginX;
            zoomY = d3.event.y - clickOriginY;
            d3.select('#decisionTree')
              .attr('transform', function (d) {
                console.log('d', d);
                return 'translate(' + [zoomX, zoomY] + ')' + `scale(${zoomZ})`;
              });
          })
      )
  }
  handleZoomLevel(x) {
    zoomZ = x;
    d3.select('#decisionTree').attr('transform', `translate(${zoomX},${zoomY})scale(${zoomZ})`);
  }
}
