import { Component, OnInit } from '@angular/core';
import { Film } from '../../interfaces/film';
import { FilmService } from '../../services/film.service';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../menu/authentication.service';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss']
})
export class MainViewComponent implements OnInit {

  constructor(
    private filmService: FilmService,
    private route: ActivatedRoute,
    public auth: AuthenticationService
  ) { }
  
  loaded: boolean = false;
  film: Film = { title: '', filmID: 0, addedBy:'', genre: '', description: ''};

  ngOnInit(): void {
    this.getFilm();
  }

  getFilm(): void {
      this.loaded = false;
      let id = +this.route.snapshot.paramMap.get("id");
      this.filmService.getFilm(id)
        .subscribe( f => {
          this.film = f
          this.loaded = true;
          this.filmService.assignImage(f).subscribe( f => this.film = f)
        });
      }
}
