import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentInvoiceComponent } from './payment-invoice.component';

describe('PaymentInvoiceComponent', () => {
  let component: PaymentInvoiceComponent;
  let fixture: ComponentFixture<PaymentInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
