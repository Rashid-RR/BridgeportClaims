import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentInputComponent } from './payment-input.component';

describe('PaymentInputComponent', () => {
  let component: PaymentInputComponent;
  let fixture: ComponentFixture<PaymentInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
