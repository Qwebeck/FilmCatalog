import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Film } from 'src/app/interfaces/film';
import { FilmService } from '../../services/film.service';
import { AuthenticationService } from '../../menu/authentication.service';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

interface UploadedImage {
  name: string,
  data: string | ArrayBuffer
}

@Component({
  selector: 'app-film-editor',
  templateUrl: './film-editor.component.html',
  styleUrls: ['./film-editor.component.scss']
})
export class FilmEditorComponent implements OnInit {

  @Input() 
  film: Film;
  
  @Input()
  editing = false;
  
  @Output()
  previewEnabled = new EventEmitter<boolean>();
  
  _previewEnabled = true;

  selectGenre: string;
  inputGenre: string;
  uploaded_images: UploadedImage[] = [];

  @Output()
  sending = new EventEmitter<boolean>();
  genres: Observable<string[]>;

  constructor(
    private auth: AuthenticationService,
    private filmService: FilmService,
    private location: Location,
    private router: Router
  ) { }

  ngOnInit() {
    let user = this.auth.currentUser;
    if ( user )
      this.film.addedBy = this.film.addedBy || user.name;
    this.selectGenre = this.film.genre;
    this.genres = this.filmService.getGenres();
    if ( !this.film.images )
      this.film.images = []
    else 
      this.uploaded_images = this.film.images.map((img, ind) => {return {name: ` image ${ind+1}`, data: img}});
  }

  publish(): void {
    this.film.genre = this.selectGenre === "other" 
                      ? this.inputGenre
                      : this.selectGenre;
    let senderFunction = this.editing 
                     ? (film: Film) => this.filmService.updateFilm(film)
                     : (film: Film) => this.filmService.saveFilm(film);
    this.sending.emit(true);
    console.log("Saving: ", this.film);
    senderFunction({...this.film, 'comments': []}).subscribe(
      _ => { 
        this.sending.emit(false);
        this.router.navigate(["dashboard"]);
      });
    }

  cancel(): void {
    this.location.back();
  }

  remove(): void {
    this.filmService.deleteFilm(this.film)
      .subscribe(_ => this.router.navigate(["dashboard"]));
  }

  togglePreview() {
    this._previewEnabled = !this._previewEnabled;
    this.previewEnabled.emit(this._previewEnabled);
  }
  
  removeImage(data: string): void {
    var index  = this.film.images.indexOf(data);
    this.film.images.splice(index, 1);
    this.uploaded_images.splice(index, 1);
  }

  upload(files): void {
    console.log(files)
    if (files.length === 0)
      return;
    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      alert("Only images are supported.");
      return;
    }
    var reader = new FileReader();
    let i = this.uploaded_images.length;
    reader.readAsDataURL(files[i%files.length]); 
    reader.onload = (_event) => {
      this.film.images.push(reader.result)
      this.uploaded_images.push({name: files[i%files.length].name, data: reader.result});
      i += 1;
      if ( i < files.length )  reader.readAsDataURL(files[i%files.length]); 
    }
  }
}
