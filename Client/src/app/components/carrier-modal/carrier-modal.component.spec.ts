import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CarrierModalComponent } from './carrier-modal.component';

describe('CarrierModalComponent', () => {
  let component: CarrierModalComponent;
  let fixture: ComponentFixture<CarrierModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CarrierModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CarrierModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
