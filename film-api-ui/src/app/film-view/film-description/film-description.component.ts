import { Component, OnInit, Input } from '@angular/core';
import { Film } from '../../film';
import { FilmService } from '../../film.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'app-film-description',
  templateUrl: './film-description.component.html',
  styleUrls: ['./film-description.component.scss']
})
export class FilmDescriptionComponent implements OnInit {

  @Input() film: Film;

  constructor(
    private filmService: FilmService,
    private route: ActivatedRoute,
    private location: Location) { }

  ngOnInit(): void {
    if ( !this.film ) {
      let id = +this.route.snapshot.paramMap.get("id");
      this.filmService.getFilm(id)
        .subscribe( f => this.film = f);
    }
  }
  goBack(): void {
    this.location.back();
  }
}
