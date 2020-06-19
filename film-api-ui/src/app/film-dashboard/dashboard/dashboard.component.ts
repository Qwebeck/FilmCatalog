import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { FilmService } from '../../film.service';
import { Film } from '../../film';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  films: Film[]; 
  userAuthorized: boolean = true;
  
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
              map( ({ matches }) => {
                let col: number;
                if (matches) col = 3;
                else col = 1;
                return this.films.map(film => {return {...film, cols: col, rows: 1}}) 
              })
            )
  constructor(
    private breakpointObserver: BreakpointObserver,
    private filmService: FilmService ) { }

  ngOnInit () {
    this.getFilms();
  }

  getFilms(): void {
    this.filmService.getFilms()
        .subscribe( films => this.films = films);
  }

  removeFilm(id: number): void {
    alert(`Removing film with id=${id}`);
  }
}
