import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FilmService } from '../film.service';
import { Film } from '../film';

@Component({
  selector: 'app-film-details',
  templateUrl: './film-details.component.html',
  styleUrls: ['./film-details.component.scss']
})
export class FilmDetailsComponent implements OnInit {
    film: Film;

    constructor(
      private filmService: FilmService,
      private route: ActivatedRoute,
      private location: Location
    ) { }

    ngOnInit(): void {
      this.getFilm();
    }
    
    getFilm(): void {
      const id = +this.route.snapshot.paramMap.get('id');
      this.filmService.getFilm(id)
        .subscribe(film => this.film = film);
    }
    
    save(): void {
      this.filmService.updateFilm(this.film)
          .subscribe(() => this.goBack());
    }
    
    goBack(): void {
      this.location.back();
    }
}
