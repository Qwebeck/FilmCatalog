import { Component, OnInit, Input } from '@angular/core';
import { Film } from '../../film';



@Component({
  selector: 'app-film-description',
  templateUrl: './film-description.component.html',
  styleUrls: ['./film-description.component.scss']
})
export class FilmDescriptionComponent implements OnInit {

  @Input() 
  film?: Film;
  
  @Input()
  readMode: boolean; 

  constructor() { }

  ngOnInit(): void { 
    if ( !this.film ) {
      this.film = { title: '', filmID: 0, addedBy:'', genre: '', description: ''};
    }
  }
}
