import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilmEditorComponent } from './film-editor.component';

describe('FilmEditorComponent', () => {
  let component: FilmEditorComponent;
  let fixture: ComponentFixture<FilmEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilmEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilmEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
