import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkippedPaymentSearchComponent } from './skipped-payment-search.component';

describe('SkippedPaymentSearchComponent', () => {
  let component: SkippedPaymentSearchComponent;
  let fixture: ComponentFixture<SkippedPaymentSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkippedPaymentSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkippedPaymentSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
