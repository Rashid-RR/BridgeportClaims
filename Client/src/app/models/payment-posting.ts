import {PaymentPostingPrescription} from './payment-posting-prescription';
import * as Immutable from 'immutable';
import { UUID } from 'angular2-uuid';

export class PaymentPosting {
    checkNumber: any;
	checkAmount: Number;
	lastAmountRemaining: Number;
	sessionId: UUID;
    payments: Immutable.OrderedMap<Number, PaymentPostingPrescription> = Immutable.OrderedMap<Number, PaymentPostingPrescription>();
    constructor() {
        this.lastAmountRemaining = null;
        this.sessionId = null;
    }

    get paymentPostings(): Array<PaymentPostingPrescription> {
        return this.payments.asImmutable().toArray();
    }

    get amountSelected(): Number {
        let amount = 0;
        this.paymentPostings.forEach(prescription => {
            amount = amount + Number(prescription.amountPosted);
        });
        return amount;
    }

}
