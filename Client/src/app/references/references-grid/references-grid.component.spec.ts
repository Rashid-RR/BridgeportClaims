import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferencesGridComponent } from './references-grid.component';

describe('ReferencesGridComponent', () => {
  let component: ReferencesGridComponent;
  let fixture: ComponentFixture<ReferencesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferencesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferencesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
