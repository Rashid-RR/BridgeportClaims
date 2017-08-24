import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPaymentInputComponent } from './new-payment-input.component';

describe('NewPaymentInputComponent', () => {
  let component: NewPaymentInputComponent;
  let fixture: ComponentFixture<NewPaymentInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewPaymentInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPaymentInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
