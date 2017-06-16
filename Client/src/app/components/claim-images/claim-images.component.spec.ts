import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimImagesComponent } from './claim-images.component';

describe('ClaimImagesComponent', () => {
  let component: ClaimImagesComponent;
  let fixture: ComponentFixture<ClaimImagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimImagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
