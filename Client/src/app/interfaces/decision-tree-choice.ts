export interface DecisionTreeChoiceModalHeader {
    episodeId: number;
    createdBy: string;
    episodeNote: string;
    created: Date | string;
}

export interface DecisionTreeChoiceModalPath {
    treeLevel: number;
    nodeName: string;
}

export interface RootDecisionTreeModal {
    decisionTreeChoiceModalHeader: DecisionTreeChoiceModalHeader;
    decisionTreeChoiceModalPaths: DecisionTreeChoiceModalPath[];
}