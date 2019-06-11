import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgPhoneNumberMaskComponent } from './ag-phone-number-mask.component';

describe('AgPhoneNumberMaskComponent', () => {
  let component: AgPhoneNumberMaskComponent;
  let fixture: ComponentFixture<AgPhoneNumberMaskComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgPhoneNumberMaskComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgPhoneNumberMaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
