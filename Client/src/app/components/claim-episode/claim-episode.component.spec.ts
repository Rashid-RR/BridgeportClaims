import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimEpisodeComponent } from './claim-episode.component';

describe('ClaimEpisodeComponent', () => {
  let component: ClaimEpisodeComponent;
  let fixture: ComponentFixture<ClaimEpisodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimEpisodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimEpisodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
