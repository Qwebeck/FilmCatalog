import { Component, OnInit, Input } from '@angular/core';
import { FilmComment } from 'src/app/interfaces/film-comment';
import { CommentsService } from '../../services/comments.service';
import { Film } from 'src/app/interfaces/film';

@Component({
  selector: 'app-comment-editing-block',
  templateUrl: './comment-editing-block.component.html',
  styleUrls: ['./comment-editing-block.component.scss']
})
export class CommentEditingBlockComponent {

  @Input()
  currentFilm: Film;
  
  content: string;

  constructor(
    private comments: CommentsService
   ) { }

  
  publish(): void {
    let msg = this.content;
    this.content = "";
    this.comments.publish(msg, this.currentFilm).subscribe(
      comment => this.currentFilm.comments.push(comment)
    );
  }
}
