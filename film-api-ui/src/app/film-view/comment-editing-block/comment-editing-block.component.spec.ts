import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentEditingBlockComponent } from './comment-editing-block.component';

describe('CommentEditingBlockComponent', () => {
  let component: CommentEditingBlockComponent;
  let fixture: ComponentFixture<CommentEditingBlockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentEditingBlockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentEditingBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
