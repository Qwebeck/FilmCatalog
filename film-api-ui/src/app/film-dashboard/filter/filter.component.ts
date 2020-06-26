import { 
  Component, Output, EventEmitter
} from '@angular/core';
import { Film } from '../../interfaces/film';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Output()
  find = new EventEmitter<string[]>();

  genres: string[];
  filmGenres;

  constructor( ) { }

  update(films: Film[]): void {
    this.filmGenres = [...new Set(films.map( f => f.genre ))];
  }

  apply() {
    this.find.emit(this.genres);
  }
}
