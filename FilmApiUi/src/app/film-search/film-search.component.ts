import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import {
   debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators'
import { Film } from '../film';
import { FilmService } from '../film.service';

@Component({
  selector: 'app-film-search',
  templateUrl: './film-search.component.html',
  styleUrls: ['./film-search.component.scss']
})
export class FilmSearchComponent implements OnInit {

  films$: Observable<Film[]>
  private searchTerms = new Subject<string>();

  constructor(private filmService: FilmService) { }

  ngOnInit(): void {
    this.films$ = this.searchTerms.pipe(
      debounceTime(300),

      distinctUntilChanged(),
      switchMap((term: string) => this.filmService.searchFilms(term)),
    );
  }

  search( term: string): void {
    this.searchTerms.next(term);
  }

}
