import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimPaymentComponent } from './claim-payment.component';

describe('ClaimPaymentComponent', () => {
  let component: ClaimPaymentComponent;
  let fixture: ComponentFixture<ClaimPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
