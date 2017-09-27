import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiaryResultsComponent } from './diary-results.component';

describe('DiaryResultsComponent', () => {
  let component: DiaryResultsComponent;
  let fixture: ComponentFixture<DiaryResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiaryResultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiaryResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
