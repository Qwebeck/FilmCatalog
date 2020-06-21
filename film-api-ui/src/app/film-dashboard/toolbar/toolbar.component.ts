import { Component, OnInit } from '@angular/core';
import { FilmService } from '../../film.service';
// import { Observable } from 'rxjs';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  filmName: string;
  filmDirector: string;
  filmGenres;

  constructor(private filmService: FilmService) { }

  ngOnInit(): void {
    this.getGenres()
  }

  getGenres(): void {
    this.filmService.getFilms().subscribe(
      ( films ) => this.filmGenres = films.map( f => f.genre )
    );
  }


}
