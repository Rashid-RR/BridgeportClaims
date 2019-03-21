import { Component, Inject, OnInit,OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpService } from '../../services/http-service';
import { ReferenceManagerService } from '../../services/reference-manager.service';
import { ClaimManager } from '../../services/claim-manager';
import { DecisionTreeService } from '../../services/services.barrel';
import { RootDecisionTreeModal, DecisionTreeChoiceModalHeader, DecisionTreeChoiceModalPath } from '../../interfaces/decision-tree-choice';
import * as d3 from 'd3';
import { ITreeNode } from '../../decision-tree/tree-node';

@Component({
  selector: 'app-decesiontree-modal',
  templateUrl: './decesiontree-modal.component.html',
  styleUrls: ['./decesiontree-modal.component.css']
})
export class DecesiontreeModalComponent implements OnInit,OnDestroy {

  treeChoiceHeader: DecisionTreeChoiceModalHeader
  treeChoicePaths: DecisionTreeChoiceModalPath[];
  loading: boolean = false;
  constructor(public dialogRef: MatDialogRef<DecesiontreeModalComponent>,
    private http: HttpService,
    public ds: DecisionTreeService,
    public rs: ReferenceManagerService,
    public claimManager: ClaimManager,
    @Inject(MAT_DIALOG_DATA) private data) {
      
  }

  ngOnInit() {
    this.loading = true;
    this.http.getTreeChoice(this.data.episodeId).subscribe((tree: RootDecisionTreeModal) => {
      this.loading = false;
      this.treeChoiceHeader = tree.decisionTreeChoiceModalHeader
      this.treeChoicePaths = tree.decisionTreeChoiceModalPaths
      this.treeChoicePaths.map(t=>{t['picked']=true; return t;})//for the selected color setup
      this.ds.treeData = this.treeChoicePaths[0] as ITreeNode;
      this.attachChildren(this.ds.treeData, this.treeChoicePaths[1], 1, this.treeChoicePaths.length);
    }, () => {
      this.loading = false;
    })
  }
  drawTree() {
    this.ds.root = d3.hierarchy(this.ds.treeData, (d) => d.children);
    this.ds.height = 200;
    this.ds.root.x0 = 50;
    this.ds.root.y0 = 20;
    this.ds.root.y = 20;
    this.ds.tree = d3.tree().size([this.ds.height, this.ds.width]);
    this.ds.svg = d3.select('#decisionTree')
      .style('width', this.ds.width)
      .style('height', this.ds.height)
      .attr('transform', 'translate('
        + this.ds.margin.left + ',' + this.ds.margin.top + ')');
    this.ds.update(this.ds.root);
  }
  attachChildren(tree: ITreeNode, child: DecisionTreeChoiceModalPath, pos: number, length: number) {
    tree['children'] = [child as ITreeNode];
    let next = pos + 1
    if (this.treeChoicePaths[next]) {
      this.attachChildren(tree['children'][0], this.treeChoicePaths[next], next, length);
    } else {
      this.drawTree();
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnDestroy(){
    this.ds.height = 500 - this.ds.margin.top - this.ds.margin.bottom;
    this.ds.root.x0 = this.ds.height/2;
    this.ds.tree = d3.tree().size([this.ds.height, this.ds.width]);
  }


}
