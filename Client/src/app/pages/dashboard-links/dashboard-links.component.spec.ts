import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardLinksComponent } from './dashboard-links.component';

describe('PrivateComponent', () => {
  let component: DashboardLinksComponent;
  let fixture: ComponentFixture<DashboardLinksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardLinksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
