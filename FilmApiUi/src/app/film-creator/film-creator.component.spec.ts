import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilmCreatorComponent } from './film-creator.component';

describe('FilmCreatorComponent', () => {
  let component: FilmCreatorComponent;
  let fixture: ComponentFixture<FilmCreatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilmCreatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilmCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
