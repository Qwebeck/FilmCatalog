import { 
  Component, OnInit, Input
} from '@angular/core';
import { FilmComment } from '../../interfaces/film-comment'; 

@Component({
  selector: 'app-film-comment',
  templateUrl: './film-comment.component.html',
  styleUrls: ['./film-comment.component.scss']
})
export class FilmCommentComponent implements OnInit {

  @Input()
  comment: FilmComment;

  constructor() { }

  ngOnInit(): void {
  
   }

}
