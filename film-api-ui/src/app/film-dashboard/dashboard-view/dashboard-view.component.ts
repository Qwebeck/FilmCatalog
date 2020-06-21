import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FilmService } from '../../film.service';
import { OktaAuthService } from '@okta/okta-angular';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { FilterComponent } from '../filter/filter.component';
import { Film } from '../../film';
import { FilmSearchComponent } from '../film-search/film-search.component';

@Component({
  selector: 'app-dashboard-view',
  templateUrl: './dashboard-view.component.html',
  styleUrls: ['./dashboard-view.component.scss']
})
export class DashboardViewComponent implements OnInit, OnDestroy {
  
  @ViewChild(DashboardComponent)
  private dashboardComponent: DashboardComponent;

  @ViewChild(FilterComponent)
  private filterComponent: FilterComponent;
  
  @ViewChild(FilmSearchComponent)
  private filmSearchComponent: FilmSearchComponent; 

  filmSubscription;

  constructor(
    private filmService: FilmService,
    private auth: OktaAuthService
  ) { }
  
  ngOnInit(): void {
    this.getFilms();
  }

  ngOnDestroy(): void {
    this.filmSubscription.unsubscribe();
  }

  private update(films: Film[]): void {
    this.dashboardComponent.update(films);
    this.filterComponent.update(films);
    this.filmSearchComponent.update(films);
  }

  getFilms() {
    this.auth.getAccessToken()
    .then( 
      (token) => this.filmSubscription = this.filmService.getFilms(token)
                                              .subscribe( films => this.update(films))
    );
  }
  
  findByTitle(title: string) {
    this.filmService.findByTitle(title).subscribe(
      (films) => this.update(films)
    );
  }

  findByGenres(genres: string[]) {
    this.filmService.findByGenres(genres).subscribe(
      (films) => this.update(films)
    )
  }
}
