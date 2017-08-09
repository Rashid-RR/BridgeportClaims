import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentClaimResultComponent } from './payment-claim-result.component';

describe('PaymentClaimResultComponent', () => {
  let component: PaymentClaimResultComponent;
  let fixture: ComponentFixture<PaymentClaimResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentClaimResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentClaimResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
