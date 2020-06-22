import { Component, OnInit } from '@angular/core';
import { FilmService } from '../film.service';
import { MessageService } from '../message.service';
import { Film } from '../film';

@Component({
  selector: 'app-film-creator',
  templateUrl: './film-creator.component.html',
  styleUrls: ['./film-creator.component.scss']
})
export class FilmCreatorComponent implements OnInit {

  selectedFilm: Film;
  films: Film [];
  
  constructor(private filmService: FilmService) { }

  getFilms(): void {
    this.filmService.getFilms().subscribe( films => this.films = films);
  }

  ngOnInit(): void {
    this.getFilms();
  }
  
  delete( film: Film) {
    this.films = this.films.filter(f => f.id != film.id)
    this.filmService.deleteFilm(film).subscribe();
  }

  add(name: string): void {
    name = name.trim()
    if (!name) return;
    this.films.push({ title: name } as Film)
    this.filmService.addFilm({title: name} as Film);
  }

}

