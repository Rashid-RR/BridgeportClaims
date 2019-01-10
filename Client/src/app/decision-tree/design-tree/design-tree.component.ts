import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import * as d3 from 'd3';
import { ITreeNode } from '../tree-node';
import { HttpService } from '../../services/http-service';
import swal from 'sweetalert2';
import { ToastsManager } from 'ng2-toastr';
import { DialogService } from 'ng2-bootstrap-modal';

declare var $: any;

@Component({
  selector: 'app-design-tree',
  templateUrl: './design-tree.component.html',
  styleUrls: ['./design-tree.component.css']
})
export class DesignTreeComponent implements OnInit, AfterViewInit {

  decisionTreeSVG: any;
  margin = { top: 20, right: 90, bottom: 30, left: 30 };
  get width(): number {
    return (this.treeDepth + 1) + 60;
  };
  duration: number = 750
  i: number = 0
  height = 500 - this.margin.top - this.margin.bottom;
  treeData: ITreeNode =
    {
      "nodeName": "Top Level",
      "nodeDescription": "",
      "parentTreeId": 0,
      treeId: 1,
    };
  dx: number = 35;
  dy: number = 120;
  svg: any;
  tree = d3.tree().size([this.height, this.width]);
  root: any;
  loading: boolean = false;
  rootTreeId: number = 2;
  depth: number = 930;
  get treeDepth() {
    return this.depth || 0;
  }
  constructor(private toast: ToastsManager, private dialogService: DialogService, private http: HttpService) { }
  collapse(d) {
    if (d.children) {
      d._children = d.children
      d._children.forEach(c => this.collapse(c));
      d.children = null
    }
  }
  ngAfterViewInit() {
    this.loading = true;
    this.http.getTree({ rootTreeId: this.rootTreeId })
      .subscribe((result: any) => {
        this.loading = false;
        this.treeData = result;
        this.root = d3.hierarchy(this.treeData, (d) => { return d.children; });
        this.root.x0 = this.height / 2;
        this.root.y0 = 20;
        this.root.y = 20;
        this.svg = d3.select('svg')
          .style("width", this.width)
          .style("height", this.height)
          .attr("transform", "translate("
            + this.margin.left + "," + this.margin.top + ")");
        this.root.children.forEach(c => this.collapse(c));
        this.update(this.root);
      }, err => {
        this.loading = false;
        try {
          const error = err.error;

        } catch (e) { }
      });
  }
  ngOnInit() {

  }
  diagonal(s, d) {
    return `M ${s.y} ${s.x}
            C ${(s.y + d.y) / 2} ${s.x},
              ${(s.y + d.y) / 2} ${d.x},
              ${d.y} ${d.x}`;
  }
  treeNodeItems(n): any {
    let items: any = {}
    let expandName = `${n._children ? 'Expand' : 'Collapse'} Node`;
    let expandIcon = n._children ? "fa-expand" : "fa-minus";
    if (n.data.children && n.data.children.length > 0) items.expand = {
      name: expandName,
      icon: expandIcon,
      callback: () => {
        this.toggleExpand(n);
        expandName = `${n._children ? 'Expand' : 'Collapse'} Node`;
        expandIcon = n._children ? "fa-expand" : "fa-minus";
        return true;
      }
    };
    items.add = {
      name: "Add Node",
      icon: "add",
      callback: () => {
        return true;
      }
    };
    items.edit = {
      name: "Edit Node",
      icon: "edit",
      callback: () => {
        return true;
      }
    };
    items.delete = {
      icon: 'delete',
      name: "Delete Node",
      callback: () => {
        this.deleteNode(n)
      }
    };
    return items;
  }
  deleteNode(n) {
    let title = `Delete this node - ${n.data.nodeName}`,
      msg = `Are you sure you want to delete this node - ${n.data.nodeName}. This will delete all the decendant trees/nodes`;
    swal({
      title: title,
      text: msg,
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.value) {
        this.removeNode(n);
      }
    }).catch(swal.noop)
  }
  removeNode(d) {
    //this is the links target node which you want to remove
    console.log(d);
    //make new set of children
    var children = [];
    //iterate through the children 
    d.parent.children.forEach(function (child) {
      if (child.id != d.id) {
        //add to the child list if target id is not same 
        //so that the node target is removed.
        children.push(child);
      }
    });
    //set the target parent with new set of children sans the one which is removed
    d.parent.children = children;
    //redraw the parent since one of its children is removed
    this.update(d.parent)
  }

  update(source) {

    // Assigns the x and y position for the nodes
    var treeData = this.tree(this.root);

    // Compute the new tree layout.
    var nodes = treeData.descendants(),
      links = treeData.descendants().slice(1);
    // Normalize for fixed-depth.
    nodes.forEach((d) => { d.y = d.depth * 180 + 70 });

    // ****************** Nodes section ***************************

    // Update the nodes...
    var node = this.svg.selectAll('g.node')
      .data(nodes, (d) => { return d.id || (d.id = ++this.i); });

    // Enter any new modes at the parent's previous position.
    var nodeEnter = node.enter().append('g')
      .attr('class', 'node')
      .attr('id', (n: any) => {
        let id = `tree_node${n.id}`;
        return id;
      })
      .attr("transform", (d) => {
        return "translate(" + source.y0 + "," + source.x0 + ")";
      })
      .on('click', (n) => {
        var _offset = $(`#tree_node${n.id}`).offset(),
          position = {
            x: _offset.left + 10,
            y: _offset.top + 10
          }
        $.contextMenu('destroy');
        $.contextMenu({
          selector: `#tree_node${n.id}`,
          trigger: 'none',
          callback: (key, options) => { },
          items: this.treeNodeItems(n)
        });
        setTimeout(function () { $(`#tree_node${n.id}`).contextMenu(position); }, 10);
      });

    // Add Circle for the nodes
    nodeEnter.append('circle')
      .attr('class', 'node')
      .attr('r', 1e-6)
      .style("fill", (d) => {
        return d._children ? "lightsteelblue" : "#fff";
      });

    // Add labels for the nodes
    nodeEnter.append('text')
      .attr("dy", ".35em")
      .attr("x", (d) => {
        return d.children || d._children ? -13 : 13;
      })
      .attr("text-anchor", (d) => {
        return d.children || d._children ? "end" : "start";
      })
      .text((d) => { return d.data.nodeName; });

    // UPDATE
    var nodeUpdate = nodeEnter.merge(node);

    // Transition to the proper position for the node
    nodeUpdate.transition()
      .duration(this.duration)
      .attr("transform", (d) => {
        this.depth = (180 * d.data.treeLevel);
        $("svg:last-child").css("width", this.width + "px");
        return "translate(" + d.y + "," + d.x + ")";
      });

    // Update the node attributes and style
    nodeUpdate.select('circle.node')
      .attr('r', 10)
      .style("fill", (d) => {
        return d._children ? "lightsteelblue" : "#fff";
      })
      .attr('cursor', 'pointer');


    // Remove any exiting nodes
    var nodeExit = node.exit().transition()
      .duration(this.duration)
      .attr("transform", (d) => {
        return "translate(" + source.y + "," + source.x + ")";
      })
      .remove();

    // On exit reduce the node circles size to 0
    nodeExit.select('circle')
      .attr('r', 1e-6);

    // On exit reduce the opacity of text labels
    nodeExit.select('text')
      .style('fill-opacity', 1e-6);

    // ****************** links section ***************************

    // Update the links...
    var link = this.svg.selectAll('path.link')
      .data(links, (d) => { return d.id; });

    // Enter any new links at the parent's previous position.
    var linkEnter = link.enter().insert('path', "g")
      .attr("class", "link")
      .attr('d', (d) => {
        var o = { x: source.x0, y: source.y0 }
        return this.diagonal(o, o)
      });

    // UPDATE
    var linkUpdate = linkEnter.merge(link);

    // Transition back to the parent element position
    linkUpdate.transition()
      .duration(this.duration)
      .attr('d', (d) => { return this.diagonal(d, d.parent) });

    // Remove any exiting links
    link.exit().transition()
      .duration(this.duration)
      .attr('d', (d) => {
        var o = { x: source.x, y: source.y }
        return this.diagonal(o, o)
      })
      .remove();

    // Store the old positions for transition.
    nodes.forEach((d) => {
      d.x0 = d.x;
      d.y0 = d.y;
    });
  }
  toggleExpand(d) {
    if (d.children) {
      d._children = d.children;
      d.children = null;
    } else {
      d.children = d._children;
      d._children = null;
    }
    this.update(d);
  }


}
