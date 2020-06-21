import { Component, OnInit, Input } from '@angular/core';
import { Film } from 'src/app/film';
import { AuthenticationService } from '../../authentication.service';
import { FilmService } from '../../film.service';

@Component({
  selector: 'app-film-editor',
  templateUrl: './film-editor.component.html',
  styleUrls: ['./film-editor.component.scss']
})
export class FilmEditorComponent implements OnInit {

  @Input() film: Film;
  otherGenre: boolean = false;
  genres: string[] = [];

  constructor(
    private auth: AuthenticationService,
    private filmService: FilmService
  ) { }

  ngOnInit(): void {
    this.film.addedBy = this.auth.currentUser();
    this.filmService.getFilms()
      .subscribe(
        films => this.genres = films.map(f => f.genre ) || []
      )
  }

  publish(): void {
    console.log("Saving: ", this.film);
    console.log("Type of image is: ", typeof this.film);
    this.filmService.saveFilm(this.film)
      .subscribe(_ => console.log("ok"));
  }

  cancel(): void {

  }

  upload(files): void {
    if (files.length === 0)
      return;
 
    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      alert("Only images are supported.");
      return;
    }
 
    var reader = new FileReader();
    reader.readAsDataURL(files[0]); 
    reader.onload = (_event) => { 
      this.film.image = reader.result; 
    }
  }
}
