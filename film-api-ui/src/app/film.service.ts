import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable, throwError, from, forkJoin } from 'rxjs';
import { catchError, retry, tap, switchMap, mergeMap, map } from 'rxjs/operators';
import { Film } from './film';
// import { OktaAuthService } from '@okta/okta-angular';
import { AuthenticationService } from './menu/authentication.service';
import { FilmImage } from './image';

@Injectable({
  providedIn: 'root'
})
export class FilmService {
  private readonly url: string = "https://localhost:5001/api/films"; 
  
  films: Film[] = []

  constructor(
    private http: HttpClient,
    private auth: AuthenticationService) { }

  getFilms(withImages: boolean, offset: number = 0, amount: number = 30): Observable<Film[]> {
    let filmUrl=`${this.url}?offset=${offset}&number=${amount}`;
    if ( withImages )
    return this.http.get<Film[]>(filmUrl).pipe(
      mergeMap( films => this.fetchImages(films).pipe(
        map(images => this.mapImages(images, films)),
        tap(this.log)
      )));
    else
    return this.http.get<Film[]>(filmUrl);
  }

  assignImages(films: Film[]): Observable<Film[]> {
    return this.fetchImages(films)
            .pipe(
              map(images => this.mapImages(images, films))
            );
  }

  private log(films:Film[]): void {
    this.films = films;
    console.log("Fetched: ", this.films);
  }

  private mapImages(images: FilmImage[], films: Film[]): Film[] {
    let sortedImages = images.sort((a,b) => a.filmID - b.filmID);
    let j = 0;
    for (let i = 0; i < films.length; ++i) {
      if ( sortedImages[j].filmID == films[i].filmID ) {
          films[i].image = sortedImages[j].data;
          j += 1; 
      }
    }
    return films;
  }

  private fetchImages(films: Film[]): Observable<FilmImage[]> {
    const query = films.map(f => `filmid=${f.filmID}&`).join("");
    const url = `https://localhost:5001/api/images?${query}`;
    return this.http.get<FilmImage[]>(url)
  }

  getFilm(id: number): Observable<Film> {
    const filmUrl = `${this.url}/${id}`;
    return this.http.get<Film>(filmUrl);
  }

  saveFilm(film: Film) {
    let obj = {
      Genre: film.genre,
      Image: film.image,
      Title: film.title,
      AddedBy: "1",
      Description: film.description,
      UserID: "1"
    };
    this.http.post<Film>(this.url, obj, {
      headers: { Authorization: 'Bearer ' + this.auth.accessToken }
    }).pipe(
      tap((_) => console.log("Saved: ", film))
    ).subscribe();


    // this.auth.getAccessToken().then(
    //   accessToken => {
    //     return 
    //   }
    // );
  }

  deleteFilm(film: Film) {
    const id = film.filmID;
    const delUrl = `${this.url}/${id}`; 
    return this.http.delete<Film>(delUrl).subscribe(
      (_) => this.films = this.films.filter( f => f !== film)
    );
  }

  findByTitle(title: string): Observable<Film[]> {
    const url = `${this.url}/findByTitle?title=${title}`;
    return this.http.get<Film[]>(url);
  }

  findByGenres(genres: string[]): Observable<Film[]> {
    const query = genres.map(g => `genre=${g}&`).join("");
    const url = `${this.url}/findByGenres?${query}`;
    return this.http.get<Film[]>(url);
  }
}
