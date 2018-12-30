 
export interface ITreeNode {
    treePath?: string;
    treeLevel?: number;
    treeId?: number;
    nodeName: string;
    nodeDescription: string;
    parentTreeId?: number;
    children?:ITreeNode[]
}