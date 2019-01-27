import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferencesContainerComponent } from './references-container.component';

describe('ReferencesContainerComponent', () => {
  let component: ReferencesContainerComponent;
  let fixture: ComponentFixture<ReferencesContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferencesContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferencesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
