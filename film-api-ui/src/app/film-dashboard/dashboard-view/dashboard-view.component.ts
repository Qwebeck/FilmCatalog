import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FilmService } from '../../services/film.service';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { FilterComponent } from '../filter/filter.component';
import { Film } from '../../interfaces/film';
import { FilmSearchComponent } from '../film-search/film-search.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard-view',
  templateUrl: './dashboard-view.component.html',
  styleUrls: ['./dashboard-view.component.scss']
})
export class DashboardViewComponent implements OnInit, OnDestroy {
  loaded: boolean = false;

  @ViewChild(DashboardComponent)
  private dashboardComponent: DashboardComponent;

  @ViewChild(FilterComponent)
  private filterComponent: FilterComponent;
  
  @ViewChild(FilmSearchComponent)
  private filmSearchComponent: FilmSearchComponent; 

  filmSubscription;
  currentOffset: number = 0;
  films:Film[] = []; 
  filmSelector: (offset: number) => Observable<Film[]> =  (offset) => this.filmService.getFilms(false, offset);

  constructor(
    private filmService: FilmService,
  ) { }
  
  ngOnInit(): void {
    this.getFilms();
  }

  ngOnDestroy(): void {
    this.filmSubscription.unsubscribe();
  }

  getFilms() {
      this.loaded = false;
      this.filmSelector = (offset) => this.filmService.getFilms(false, offset);
      this.currentOffset = 0;
      this.filmSubscription = this.filmService.getFilms(false, this.currentOffset)
                                              .subscribe( films => {
                                                this.update(films)
                                              });
  }
  
  findByTitle(title: string) {
    this.loaded = false;
    this.filmSelector = (offset) => this.filmService.findByTitle(title, offset);
    this.currentOffset = 0;
    this.filmService.findByTitle(title, this.currentOffset).subscribe(
      (films) => this.update(films)
    );
  }

  findByGenres(genres: string[]) {
    this.loaded = false;
    this.filmSelector = (offset) => this.filmService.findByGenres(genres, offset);
    this.currentOffset = 0;
    this.filmService.findByGenres(genres, this.currentOffset ).subscribe(
      (films) => this.update(films)
    )
  }

  previousPage() {
    this.loaded = false;
    this.currentOffset -= 1;
    this.filmSelector(this.currentOffset).subscribe(
      (films) => this.update(films)
    )
  }

  nextPage() {
    this.loaded = false;
    this.currentOffset += 1;
    this.filmSelector(this.currentOffset).subscribe(
      (films) => this.update(films)
    )
  }

  clearFilters() {
    this.filmSelector =  (offset) => this.filmService.getFilms(false, offset);
    this.currentOffset = 0;
    this.getFilms();
  }
  
  private update(films: Film[]): void {
    this.films = films;
    this.updateDashboard(films);
    this.filmSearchComponent.update(films);
    this.loaded = true;
  }

  private updateDashboard(films: Film[]): void {
    this.dashboardComponent.update(films);
    this.filmService.assignImages(films)
      .subscribe(films => this.dashboardComponent.update(films));
  }
}
