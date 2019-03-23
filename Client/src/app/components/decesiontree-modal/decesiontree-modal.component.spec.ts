import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DecisionTreeModalComponent } from './decesiontree-modal.component';

describe('DecisionTreeModalComponent', () => {
  let component: DecisionTreeModalComponent;
  let fixture: ComponentFixture<DecisionTreeModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DecisionTreeModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DecisionTreeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
