import { Component, OnInit, AfterViewInit, ViewChild, NgZone, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as d3 from 'd3';
import swal from 'sweetalert2';
import { HttpService } from '../../services/http-service';
import { DecisionTreeService} from '../../services/services.barrel';
import { ProfileManager } from '../../services/profile-manager';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
import { LocalStorageService } from 'ngx-webstorage';

declare var $: any;

let zoomX = 0, zoomY = 0, zoomZ = 1, clickOriginX = null, clickOriginY = null;

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit, OnDestroy {
  zoomLevel = 1;
  over: boolean[];
  claimId: string;
  rootNode: string;
  rootTreeId: any;
  leafText: string;
  leafTreeId: any;
  rootText: string;
  @ViewChild('episodeSwal') private episodeSwal: SwalComponent;
  constructor(
    private localSt: LocalStorageService,
    public readonly swalTargets: SwalPartialTargets, private router: Router, private route: ActivatedRoute, private zone: NgZone,
    public ds: DecisionTreeService, private profileManager: ProfileManager, private http: HttpService) {
    this.over = new Array(1);
    this.over.fill(false);
  }
  episode() {
    // $("#newEpisode").modal('show');
    this.episodeSwal.show().then((r) => {
    });
  }
  collapse(d) {
    if (d.children) {
      d._children = d.children;
      d._children.forEach(c => this.collapse(c));
      d.children = null;
    }
  }

  get isExperience(): boolean {
    const url = this.router.url;
    if (url.indexOf('experience') > -1) {
      return true;
    } else {
      return false;
    }
  }

  ngOnDestroy(): void {
    $('body, html, .wrapper').css('height', 'auto');
    this.ds.readonly = false;
  }
  ngAfterViewInit() {    
    this.ds.readonly = false;
    $('body, html, .wrapper').css('height', '100%');
    if (this.ds.parentTreeId) {
      this.ds.loading = true;
      this.http.getTree({ parentTreeId: this.ds.parentTreeId })
        .subscribe((result: any) => {
          this.ds.loading = false;
          this.ds.treeData = result;
          this.ds.root = d3.hierarchy(this.ds.treeData, (d) => d.children);
          this.ds.root.x0 = this.ds.height / 2;
          this.ds.root.y0 = 20;
          this.ds.root.y = 20;
          this.ds.svg = d3.select('#decisionTree')
            .style('width', this.ds.width)
            .style('height', this.ds.height)
            .attr('transform', 'translate('
              + this.ds.margin.left + ',' + this.ds.margin.top + ')');
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
    try { swal.clickCancel(); } catch (e) { }
    this.ds.endExperience.subscribe(async (exp) => {
      if (!this.claimId || this.claimId === 'episode') {
        this.localSt.store('treeExperienceEpisode', {time: (new Date()).getTime(), type: 'episode', value: exp});
      } else {
        this.localSt.store('treeExperienceClaim', {time: (new Date()).getTime(), type: 'claim', value: exp});
      }
    });
    this.ds.onExperienceEnd.subscribe(async (exp) => {
      this.episode();
      this.zone.run(() => {
        this.rootText = exp.root.nodeName;
        this.rootTreeId = exp.root.treeId;
        this.leafText = exp.leaf.nodeName;
        this.leafTreeId = exp.leaf.treeId;
        this.ds.episodeForm.reset();
        this.ds.episodeForm.patchValue({
          claimId: this.claimId,
          episodeText: '',
          pharmacyNabp: null,
          episodeTypeId: 1,
          rootTreeId: exp.root.treeId,
          leafTreeId: exp.leaf.treeId
        });
      });
    });
  }

  get allowed(): boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles
      instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  startDragging() {
    d3.select('#decisionTree')
      .call(
        d3.drag()
          .on('start', () => {
            clickOriginX = d3.event.x;
            clickOriginY = d3.event.y;
          })
          .on('drag',  ()=> {
            zoomX+= d3.event.dx;// - clickOriginX;
            zoomY+= d3.event.dy;// - clickOriginY;
            d3.select('#decisionTree')
              .attr('transform', () =>{
                return 'translate(' + [zoomX,zoomY] + ')' + `scale(${zoomZ})`;
              });
          })
      );
  }
  handleZoomLevel(x) {
    zoomZ = x;
    d3.select('#decisionTree').attr('transform', `translate(${zoomX},${zoomY})scale(${zoomZ})`);
  }
}
