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
  private readonly url: string = "https://localhost:5001/api/films"; 
  
  films: Film[] = []

  constructor(
    private http: HttpClient,
    private auth: OktaAuthService) { }

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
    const filmUrl = `${this.url}/${id}`;
    return this.http.get<Film>(filmUrl);
  }

  saveFilm(film: Film): void {
    let obj = {
      Genre: film.genre,
      Image: film.image,
      Title: film.title,
      AddedBy: "1",
      Description: film.description,
      UserID: "1"
    };
    this.auth.getAccessToken().then(
      accessToken => {
        return this.http.post<Film>(this.url, obj, {
          headers: { Authorization: 'Bearer ' + accessToken}
        }).pipe(
          tap((_) => console.log("Saved: ", film))
        ).subscribe();
      }
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
    const query = genres.map(g => `genre=${g}&`).join("");
    const url = `${this.url}/genres?${query}`;
    return this.http.get<Film[]>(url);
  }
}
