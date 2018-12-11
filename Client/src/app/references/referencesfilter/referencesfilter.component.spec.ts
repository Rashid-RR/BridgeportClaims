import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferencesfilterComponent } from './referencesfilter.component';

describe('ReferencesfilterComponent', () => {
  let component: ReferencesfilterComponent;
  let fixture: ComponentFixture<ReferencesfilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferencesfilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferencesfilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
