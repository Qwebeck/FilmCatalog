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
import { MatDividerModule } from '@angular/material/divider';
import { FormsModule } from '@angular/forms';
import { MatProgressBarModule } from '@angular/material/progress-bar';
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
    MatInputModule,
    MatDividerModule,
    FormsModule,
    MatProgressBarModule    
  ],
  exports: [
    FilmDescriptionComponent
  ]
})
export class FilmViewModule { }
