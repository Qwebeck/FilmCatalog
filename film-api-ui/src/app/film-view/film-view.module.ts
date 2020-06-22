import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilmDescriptionComponent } from './film-description/film-description.component';
import { SharedModule } from '../shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MainViewComponent } from './main-view/main-view.component';
import { FilmCommentComponent } from './film-comment/film-comment.component';
import { CommentEditingBlockComponent } from './comment-editing-block/comment-editing-block.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [
    FilmDescriptionComponent,
    MainViewComponent,
    FilmCommentComponent,
    CommentEditingBlockComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MatButtonModule,
    MatGridListModule,
    MatInputModule
  ],
  exports: [
    FilmDescriptionComponent
  ]
})
export class FilmViewModule { }
