import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable, throwError } from 'rxjs';
import { catchError, retry,tap } from 'rxjs/operators';
import { Film } from './film';
import { OktaAuthService } from '@okta/okta-angular';


@Injectable({
  providedIn: 'root'
})
export class FilmService {
  private readonly url: string = "https://localhost:5001/films"; 
  films: Film[] = [ ]
  constructor(private http: HttpClient) { }

  getFilms(accessToken): Observable<Film[]> {
    return this.http.get<Film[]>(this.url, 
      { 
        headers: { Authorization: 'Bearer ' + accessToken},  
        observe: 'body', 
        responseType: 'json' }
      ).pipe(
      tap( response => {
        console.log("Fetched: ", response);
        this.films = response;
      })
    );
  }

  getFilm(id: number): Observable<Film> {
    return of(this.films.find( f => f.filmID == id));
  }

  saveFilm(film: Film): Observable<Film> {
    let obj = {
      Genre: film.genre,
      Image: film.image,
      Title: film.title,
      AddedBy: "1",
      Description: film.description,
      UserID: 1
    };
    return this.http.post<Film>(this.url, obj).pipe(
      tap((_) => console.log("Saved: ", film))
    );
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
    const url = `${this.url}/findByGenres?genres=${genres.join()}`;
    return this.http.get<Film[]>(url);
  }
}
