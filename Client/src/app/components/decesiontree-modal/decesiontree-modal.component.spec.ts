import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DecesiontreeModalComponent } from './decesiontree-modal.component';

describe('DecesiontreeModalComponent', () => {
  let component: DecesiontreeModalComponent;
  let fixture: ComponentFixture<DecesiontreeModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DecesiontreeModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DecesiontreeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
