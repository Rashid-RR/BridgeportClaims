import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClaimScriptNoteComponent } from './claim-script-note.component';

describe('ClaimScriptNoteComponent', () => {
  let component: ClaimScriptNoteComponent;
  let fixture: ComponentFixture<ClaimScriptNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClaimScriptNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClaimScriptNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
