import { 
  Component, Output, EventEmitter
 } from '@angular/core';
import { Film } from '../../interfaces/film';

@Component({
  selector: 'app-film-search',
  templateUrl: './film-search.component.html',
  styleUrls: ['./film-search.component.scss']
})
export class FilmSearchComponent  {

  @Output()
  find  = new EventEmitter<string>();

  constructor() { }

  private fetchedFims: Film[] = [];
  previousSubtitle: string;
  films: Film[] = [];
  title: string;

  update(films: Film[]): void {
    this.fetchedFims = films;
  }

  searchInFetched(subtitle: string) {
    if (subtitle === this.previousSubtitle ) return;
    this.films = this.fetchedFims.filter( f => f.title.includes(subtitle));
    this.previousSubtitle = subtitle;
  }

  fetchFromServer() {
    this.find.emit(this.title);
  }

}
