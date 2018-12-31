import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import * as d3 from 'd3';
import { ITreeNode } from '../tree-node';
import { HttpService } from '../../services/http-service';

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit {

  decisionTreeSVG: any;
  margin = { top: 10, right: 120, bottom: 10, left: 40 };
  get width(): number {
    return (this.treeDepth+1) * this.dy;
  };
  height = 300 - this.margin.top - this.margin.bottom;
  treeData: ITreeNode =
    {
      "nodeName": "Top Level",
      "nodeDescription": "",
      "parentTreeId": 0,
      treeId: 1,
    };
  dx: number = 35;
  dy: number = 120;
  tree = data => {
    const root = d3.hierarchy(data);
    this.depth = root.height;
    root.dx = this.dx;
    root.dy = this.dy;//this.width / (root.height + 1);
    return d3.tree().nodeSize([root.dx, root.dy])(root);
  }
  loading: boolean = false;
  rootTreeId: number = 2;
  depth: number = 930;
  get treeDepth() {
    return this.depth;
  }
  constructor(private http: HttpService) { }

  ngAfterViewInit() {
    this.loading = true;
    this.http.getTree({ rootTreeId: this.rootTreeId })
      .subscribe((result: any) => {
        this.loading = false;
        this.treeData = result;
        this.chart();
      }, err => {
        this.loading = false;
        try {
          const error = err.error;

        } catch (e) { }
      });
  }
  ngOnInit() {

  }

  /*  update(source) {
     const duration = d3.event && d3.event.altKey ? 2500 : 250;
     const nodes = this.root.descendants().reverse();
     const links = this.root.links();
     // Compute the new tree layout.
     this.tree(this.root);
 
     let left = this.root;
     let right = this.root;
     this.root.eachBefore(node => {
       if (node.x < left.x) left = node;
       if (node.x > right.x) right = node;
     });
 
     console.log(right.x, right.y)
     const height = right.x - left.x + this.margin.top + this.margin.bottom;
 
     const transition = this.decisionTreeSVG.transition()
       .duration(duration)
       .attr("height", height)
       .attr("viewBox", [-this.margin.left, left.x - this.margin.top, this.width, height])
       .tween("resize", () => () => this.decisionTreeSVG.dispatch("toggle"));
 
     // Update the nodes…
     const node = this.gNode.selectAll("g")
       .data(nodes, d => d.id);
 
     // Enter any new nodes at the parent's previous position.
     const nodeEnter = node.enter().append("g")
       .attr("transform", d => `translate(${source.y0},${source.x0})`)
       .attr("fill-opacity", 0)
       .attr("stroke-opacity", 0)
       .on("click", d => {
         d.children = d.children ? null : d._children;
         this.update(d);
       });
 
     nodeEnter.append("circle")
       .attr("r", 2.5)
       .attr("fill", d => d._children ? "#555" : "#999");
 
     nodeEnter.append("text")
       .attr("dy", "0.31em")
       .attr("x", d => d._children ? -6 : 6)
       .attr("text-anchor", d => d._children ? "end" : "start")
       .text(d => d.data.name)
       .clone(true).lower()
       .attr("stroke-linejoin", "round")
       .attr("stroke-width", 3)
       .attr("stroke", "white");
 
     // Transition nodes to their new position.
     node.merge(nodeEnter).transition(transition)
       .attr("transform", d => `translate(${d.y},${d.x})`)
       .attr("fill-opacity", 1)
       .attr("stroke-opacity", 1);
 
     // Transition exiting nodes to the parent's new position.
     node.exit().transition(transition).remove()
       .attr("transform", d => `translate(${source.y},${source.x})`)
       .attr("fill-opacity", 0)
       .attr("stroke-opacity", 0);
 
     // Update the links…
     const link = this.gLink.selectAll("path")
       .data(links, d => d.target.id);
 
     // Enter any new links at the parent's previous position.
     const linkEnter = link.enter().append("path")
       .attr("d", d => {
         const o = { x: source.x0, y: source.y0 };
         return this.diagonal({ source: o, target: o });
       });
 
     // Transition links to their new position.
     link.merge(linkEnter).transition(transition)
       .attr("d", this.diagonal);
 
     // Transition exiting nodes to the parent's new position.
     link.exit().transition(transition).remove()
       .attr("d", d => {
         const o = { x: source.x, y: source.y };
         return this.diagonal({ source: o, target: o });
       });
 
     // Stash the old positions for transition.
     this.root.eachBefore(d => {
       d.x0 = d.x;
       d.y0 = d.y;
     });
     return this.decisionTreeSVG.node();
   } */
  chart = () => {
    const root = this.tree(this.treeData);

    let x0 = Infinity;
    let x1 = -x0;
    root.each(d => {
      if (d.x > x1) x1 = d.x;
      if (d.x < x0) x0 = d.x;
    });

    const svg = d3.select('svg')
      .style("width", this.width)
      .style("height", this.height);

    const g = svg.append("g")
      .attr("font-family", "sans-serif")
      .attr("font-size", 10)
      .attr("transform", `translate(${root.dy / 3},${root.dx - x0})`);

    const link = g.append("g")
      .attr("fill", "none")
      .attr("stroke", "#555")
      .attr("stroke-opacity", 0.4)
      .attr("stroke-width", 1.5)
      .selectAll("path")
      .data(root.links())
      .enter().append("path")
      .attr("d", d3.linkHorizontal()
        .x(d => d.y)
        .y(d => d.x));

    const node = g.append("g")
      .attr("class", "pointer")
      .attr("stroke-linejoin", "round")
      .attr("stroke-width", 3)
      .selectAll("g")
      .data(root.descendants().reverse())
      .enter().append("g")
      .attr("transform", d => `translate(${d.y},${d.x})`)
      .on("click", this.nodeClick);;

    node.append("circle")
    .attr("class", "pointer")
      .attr("fill", d => d.children ? "#555" : "#999")
      .attr("r", 10);

    node.append("text")
      .attr("dy", "-0.8em")
      .attr("x", d => d.children ? 0 :13)
      /* .attr("dy", "0.31em")
      .attr("x", d => d.children ? -6 : 6) */
      .attr("text-anchor", d => d.children ? "end" : "start")
      .text(d => d.data.nodeName)
      .clone(true).lower()
      .attr("stroke", "white");

    return svg.node();
  }
  nodeClick(d) {
    console.log(d);
    if (d.children) {
      d._children = d.children;
      d.children = null;
    } else {
      d.children = d._children;
      d._children = null;
    }
   // update(d);
  }

}
