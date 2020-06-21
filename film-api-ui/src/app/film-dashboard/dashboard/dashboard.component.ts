import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { FilmService } from '../../film.service';
import { Film } from '../../film';
import { OktaAuthService } from '@okta/okta-angular';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  // userAuthorized: boolean = true;
  // check if really needed
  filmSubscription;
  cards;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private filmService: FilmService,
    private auth: OktaAuthService )
  { }

  // Ask if there is any other
  update(films: Film[]): void {
    this.cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
      map( ({ matches }) => {
        let col: number;
        if (matches) col = 3;
        else col = 1;
        return films.map(film => {return {...film, cols: col, rows: 1}}) 
      })
    )
  } 

  removeFilm(id: number): void {
    alert(`Removing film with id=${id}`);
  }
}
