import { 
  Component, Output, EventEmitter, OnInit
} from '@angular/core';
import { Film } from '../../interfaces/film';
import { FilmService } from '../../services/film.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit{
  @Output()
  find = new EventEmitter<string[]>();

  @Output()
  clear = new EventEmitter();
  
  genres: string[];
  filmGenres;

  constructor( 
    private filmsService: FilmService
  ) { }

  ngOnInit() {
    this.update()
  }

  update(): void {
    this.filmsService.getGenres().subscribe(
      genres => this.filmGenres = genres
    )
  }

  apply() {
    this.find.emit(this.genres);
  }

  clearFilters() {
    this.clear.emit();
  }
}
