import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimResultComponent } from './claim-result.component';

describe('ClaimResultComponent', () => {
  let component: ClaimResultComponent;
  let fixture: ComponentFixture<ClaimResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
