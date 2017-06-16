import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimPrescriptionsComponent } from './claim-prescriptions.component';

describe('ClaimPrescriptionsComponent', () => {
  let component: ClaimPrescriptionsComponent;
  let fixture: ComponentFixture<ClaimPrescriptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimPrescriptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimPrescriptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
