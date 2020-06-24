import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Film } from 'src/app/film';
import { FilmService } from '../../film.service';
import { OktaAuthService } from '@okta/okta-angular';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-film-editor',
  templateUrl: './film-editor.component.html',
  styleUrls: ['./film-editor.component.scss']
})
export class FilmEditorComponent implements OnInit {

  @Input() film: Film;
  @Input() editing = false;
  @Output() previewEnabled = new EventEmitter<boolean>();
  _previewEnabled = true;

  otherGenre: boolean = false;
  genres: string[] = [];

  constructor(
    private auth: OktaAuthService,
    private filmService: FilmService,
    private location: Location,
    private router: Router
  ) { }

  async ngOnInit() {
    // this.film.addedBy = this.auth.currentUser();
    const token = await this.auth.getAccessToken();
    let user = await this.auth.getUser();
    this.film.addedBy = this.film.addedBy || user.name;
    this.filmService.getFilms(token)
      .subscribe(
        films => this.genres = films.map(f => f.genre ) || []
      )
  }

  publish(): void {
    console.log("Saving: ", this.film);
    this.filmService.saveFilm(this.film)
  }

  cancel(): void {
    this.location.back();
  }

  remove(): void {
    this.filmService.deleteFilm(this.film);
    this.router.navigate(["dashboard"])
  }

  togglePreview() {
    this._previewEnabled = !this._previewEnabled;
    this.previewEnabled.emit(this._previewEnabled);
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
