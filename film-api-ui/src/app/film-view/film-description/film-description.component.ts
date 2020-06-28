import { Component, OnInit, Input } from '@angular/core';
import { Film } from '../../interfaces/film';
import { AuthenticationService } from '../../menu/authentication.service';
import { FilmService } from '../../services/film.service';

@Component({
  selector: 'app-film-description',
  templateUrl: './film-description.component.html',
  styleUrls: ['./film-description.component.scss']
})
export class FilmDescriptionComponent implements OnInit {

  @Input() 
  film?: Film;
  
  @Input()
  readMode: boolean; 

  couldEdit: boolean = false;

  constructor(
    public auth: AuthenticationService,
    private filmService: FilmService
  ) { }

  ngOnInit(): void { 
    if ( !this.film ) {
      this.film = { title: '', filmID: 0, addedBy:'', genre: '', description: ''};
    }
    else {
      this.couldEdit = this.readMode && this.auth.currentUser && (this.auth.currentUser.name == this.film.addedBy || this.auth.currentUser.groups.includes("Administrators"))
    }
  }

  vote(mark: 0 | 1): void {
    this.filmService.markFilm(this.film.filmID, mark).subscribe(
      _ => this.filmService.getFilm(this.film.filmID).subscribe(
        f => this.film = f
      )
    );
  }
}
