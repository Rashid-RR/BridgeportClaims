import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortPaySearchComponent } from './short-pay-search.component';

describe('ShortPaySearchComponent', () => {
  let component: ShortPaySearchComponent;
  let fixture: ComponentFixture<ShortPaySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShortPaySearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShortPaySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
