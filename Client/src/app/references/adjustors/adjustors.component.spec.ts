import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustorsComponent } from './adjustors.component';

describe('AdjustorsComponent', () => {
  let component: AdjustorsComponent;
  let fixture: ComponentFixture<AdjustorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdjustorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdjustorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
