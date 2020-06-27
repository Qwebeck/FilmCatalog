import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteNotFoundViewComponent } from './route-not-found-view.component';

describe('RouteNotFoundViewComponent', () => {
  let component: RouteNotFoundViewComponent;
  let fixture: ComponentFixture<RouteNotFoundViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RouteNotFoundViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RouteNotFoundViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
