export interface ITreeNode {
  treePath?: string;
  treeLevel?: number;
  picked?: boolean;
  treeId?: number;
  nodeName: string;
  nodeDescription?: string;
  parentTreeId?: number;
  children?: ITreeNode[]
}
