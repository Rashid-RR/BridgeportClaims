import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewFeaturesComponent } from './new-features.component';

describe('NewFeaturesComponent', () => {
  let component: NewFeaturesComponent;
  let fixture: ComponentFixture<NewFeaturesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewFeaturesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewFeaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
