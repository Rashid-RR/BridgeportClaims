export class PrescriptionStatuses {
    prescriptionStatusId: String;
    statusName:  String;
    constructor(prescriptionStatusId: String, statusName: String) {
        this.prescriptionStatusId = prescriptionStatusId;
        this.statusName = statusName;
    }
}
