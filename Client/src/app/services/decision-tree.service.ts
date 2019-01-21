import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { ToastsManager } from 'ng2-toastr';
import { Router } from '@angular/router';
import * as Immutable from 'immutable';
import { SortColumnInfo } from '../directives/table-sort.directive';
import { ITreeNode } from '../decision-tree/tree-node';
import * as d3 from 'd3';
import swal from 'sweetalert2';

declare var $: any;

@Injectable()
export class DecisionTreeService {

  loading: Boolean = false;
  treeList: Immutable.OrderedMap<Number, ITreeNode> = Immutable.OrderedMap<Number, ITreeNode>();
  data: any = {};
  decisionTreeSVG: any;
  margin = { top: 20, right: 90, bottom: 30, left: 30 };
  get width(): number {
    return (this.treeDepth + 1) + 60;
  };
  duration: number = 750
  i: number = 0
  height = 500 - this.margin.top - this.margin.bottom;
  treeData: ITreeNode;
  dx: number = 35;
  dy: number = 120;
  svg: any;
  tree = d3.tree().size([this.height, this.width]);
  root: any;
  parentTreeId: number;
  depth: number = 930;
  totalRowCount: number;
  display: string = 'list';
  constructor(private router: Router, private http: HttpService, private toast: ToastsManager) {
    this.data = {
      "searchText": null,
      "sort": "treeLevel",
      "sortDirection": "ASC",
      "page": 1,
      "pageSize": 30
    };

  }
  get treeDepth() {
    return this.depth || 0;
  }
  refresh() {

  }
  get totalPages() {
    return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
  }
  get treeArray(): Array<ITreeNode> {
    return this.treeList.toArray();
  }

  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }

  search(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      const data = JSON.parse(JSON.stringify(this.data)); // copy data instead of memory referencing

      if (next) {
        data.page++;
      }
      if (prev && data.page > 1) {
        data.page--;
      }
      if (page) {
        data.page = page;
      }
      this.http.getTreeList(data)
        .subscribe((resp: any) => {
          this.loading = false;
          this.totalRowCount = resp.totalRowCount;
          this.treeList = Immutable.OrderedMap<Number, ITreeNode>();
          resp.results.forEach((tree: ITreeNode) => {
            try {
              this.treeList = this.treeList.set(tree.treeId, tree);
            } catch (e) { }
          });
          if (next) {
            this.data.page++;
          }
          if (prev && this.data.page !== data.page) {
            this.data.page--;
          }
          if (page) {
            this.data.page = page;
          }
          setTimeout(() => {
            // this.events.broadcast('payment-amountRemaining',result)
          }, 200);
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        });
    }
  }
  get pages(): Array<any> {
    return new Array(this.data.page);
  }
  get pageStart() {
    return this.treeArray.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
  }
  get pageEnd() {
    return this.treeArray.length > 1 ? (this.data.pageSize > this.treeArray.length ? ((this.data.page - 1) * this.data.pageSize) + this.treeArray.length : (this.data.page) * this.data.pageSize) : null;
  }

  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.treeArray.length;
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
        this.createUpdateNode(n);
        return true;
      }
    };
    /* 
    No node editing 
    items.edit = {
       name: "Edit Node",
       icon: "edit",
       callback: () => {
         this.createUpdateNode(n, n.data.nodeName);
         return true;
       }
     }; */
    items.delete = {
      icon: 'delete',
      name: "Delete Node",
      callback: () => {
        this.deleteNode(n);
        return true;
      }
    };
    return items;
  }

  saveRoot(name: string, newTree?: boolean) {
    this.loading = true;
    this.http.saveTreeNode({ nodeName: name })
      .subscribe((result: any) => {
        this.loading = false;
        if (newTree) {
          this.router.navigate(['/main/decision-tree/construct', result.treeId]);
        } else {
          result.children = [];
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
          this.update(this.root);
        }
      }, err => {
        this.loading = false;
        try {
          const error = err.error;

        } catch (e) { }
      });

  }
  makeD3Node(data, parent) {
    //I have to use d3.hierarchy because d3 Node constructor is not public
    if (!data.treeLevel) {
      data.treeLevel = parent.data.treeLevel + 1
    }
    var node = d3.hierarchy(data);
    node.parent = parent;
    node.depth = parent.depth + 1;
    node.children = null;
    node.data = data;
    node.height = 0;
    return node;
  }
  saveChild(n: any, name: string) {
    this.loading = true;
    this.http.saveTreeNode({ parentTreeId: n.data.treeId, nodeName: name })
      .subscribe((result: any) => {
        this.loading = false;
        result.children = [];
        n.data.children.push(result);
        let node = this.makeD3Node(result, n);
        if (!n.children) {
          n.children = n._children;
          n._children = null;
        }
        n.children ? n.children.push(node) : n.children = [node];
        this.update(n);
      }, err => {
        this.loading = false;
        try {
          const error = err.error;

        } catch (e) { }
      });
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
        this.loading = true;
        this.http.deleteTreeNode(n.data.treeId)
          .subscribe((_: any) => {
            this.loading = false;
            this.removeNode(n);
          }, err => {
            this.loading = false;
            try {
              const error = err.error;
              if ([401, 406, 500].indexOf(err.status) == -1) {
                this.toast.warning(error.message);
              }
            } catch (e) { }
          });
      }
    }).catch(swal.noop)
  }
  removeNode(d) {
    //this is the links target node which you want to remove
    //make new set of children
    var children = [];
    //check if it was the root node deleted
    if (!d.parent) {
      this.toast.success("You deleted the whole tree, you can create a new tree on go to the list")
      this.router.navigate(['/main/decision-tree/construct']);
    } else {
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
      .attr('data-toggle', "tooltip")
      .attr('data-placement', 'left')
      .attr('title', (n: any) => {
        return n.data.nodeDescription;
      })
      .attr('id', (n: any) => {
        let id = `tree_node${n.id}`;
        $(`#${id}`).tooltip();
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
  createUpdateNode(n?: any, title?: any, newTree?: boolean) {
    swal({
      title: `New ${n ? 'Tree Node' : 'New Tree'}`,
      html: `<div class="form-group">
                <label id="treeNodeNameLabel">Name</label>
                <input class="form-control"  type="text" id="treeNodeName" value="${title || ''}">
            </div> 
            <div class="row">
                <div class="col-sm-6 text-right">
                    <button class="btn btn-flat btn-primary save-tree-node" type="button" style="color:white;background-color: rgb(48, 133, 214);">Save</button>
                </div>
                <div class="col-sm-6 text-left">
                  <div class="form-group">
                  <button class="btn btn-flat btn-default cancel-tree-note" type="button" style="color:white;background-color: rgb(170, 170, 170);">Cancel</button>
                  </div>
                </div>
              </div>`,
      showConfirmButton: false,
      showCancelButton: false,
      showLoaderOnConfirm: true,
      confirmButtonText: 'Save',
      cancelButtonText: 'Cancel',
      customClass: 'prescription-modal',
      preConfirm: function () {
        return new Promise(function (resolve) {
          resolve([
            $('#treeNodeName').val()
          ]);
        });
      },
      onOpen: function () {
        $('#treeNodeName').focus();
      }
    }).catch(swal.noop);
    $('#treeNodeName').on('keypress', function (e) {
      if (e.which == 13) {
        $('button.save-tree-node').click();
      }
    });
    $('button.save-tree-node').click(() => {
      let name = $('#treeNodeName').val();
      if (!name) {
        this.toast.warning('Please provide a name for this node');
        setTimeout(() => { $('#treeNodeNameLabel').css({ 'color': '#545454' }); }, 150);
      } else {
        $('#treeNodeNameLabel').css({ 'color': '#000' });
        if (!n) {
          this.saveRoot(name,newTree);
        } else if (title) {
          n.data.nodeName = name;
        } else {
          this.saveChild(n, name);
        }
        swal.close();
      }
    });
    $('button.cancel-tree-note').click(() => {
      swal.close();
    });
  }

}
