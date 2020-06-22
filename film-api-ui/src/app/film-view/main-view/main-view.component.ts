import { Component, OnInit } from '@angular/core';
import { Film } from '../../film';
import { FilmService } from '../../film.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss']
})
export class MainViewComponent implements OnInit {

  constructor(
    private filmService: FilmService,
    private route: ActivatedRoute
  ) { }

  film: Film = { title: '', filmID: 0, addedBy:'', genre: '', description: ''};

  ngOnInit(): void {
    this.getFilm();
  }

  getFilm(): void {
      let id = +this.route.snapshot.paramMap.get("id");
      this.filmService.getFilm(id)
        .subscribe( f => this.film = f);
  }

}
