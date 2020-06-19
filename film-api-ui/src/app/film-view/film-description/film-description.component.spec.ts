import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilmDescriptionComponent } from './film-description.component';

describe('FilmDescriptionComponent', () => {
  let component: FilmDescriptionComponent;
  let fixture: ComponentFixture<FilmDescriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilmDescriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilmDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
