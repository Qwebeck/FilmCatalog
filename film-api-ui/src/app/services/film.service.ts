import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable, throwError, from, forkJoin } from 'rxjs';
import { catchError, retry, tap, switchMap, mergeMap, map } from 'rxjs/operators';
import { Film } from '../interfaces/film';
// import { OktaAuthService } from '@okta/okta-angular';
import { AuthenticationService } from '../menu/authentication.service';
import { FilmImage } from '../interfaces/image';

@Injectable({
  providedIn: 'root'
})
export class FilmService {
  private readonly url: string = "https://localhost:5001/api/films"; 
  
  films: Film[] = []

  constructor(
    private http: HttpClient,
    private auth: AuthenticationService) { }
  
  /**
   * Fetches n films from the api, starting from offset
   * @param withImages if False, fetches only JSON describing film, otherwise - json with images
   * @param offset id from which films should be fetched
   * @param amount amount of films to fetch
   */
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
  /**
   * Get film with given id
   * @param id id of film that should be returned 
   */
  getFilm(id: number): Observable<Film> {
    const filmUrl = `${this.url}/${id}`;
    return this.http.get<Film>(filmUrl);
  }

  /**
   * Fetches and assign images for given films
   * @param films films for which images should be fetched
   */
  assignImages(films: Film[]): Observable<Film[]> {
    return this.fetchImages(films)
            .pipe(
              map(images => this.mapImages(images, films))
            );
  }

  /**
   * Fetches and assign image for film given as argument 
   * @param film 
   */
  assignImage(film: Film): Observable<Film> {
    const url = `${this.url}/${film.filmID}/image`;
    return this.http.get<FilmImage>(url).pipe(
      map(image=>{return {...film, 'image': image.data}})
    );
  }

  /**
   * Fetches all existing genres
   */
  getGenres(): Observable<string[]> {
    const url = `${this.url}/genres`;
    return this.http.get<string[]>(url);
  }

  /**
   * Saves film
   * @param film film, that should be saved
   */
  saveFilm(film: Film): Observable<Film> {
    let obj = {
      Genre: film.genre,
      Image: film.image,
      Title: film.title,
      Description: film.description,
    };
    return this.http.post<Film>(this.url, obj, {
      headers: { Authorization: this.auth.accessToken }
    }).pipe(
      tap((_) => console.log("Saved: ", film))
    )
  }

  /**
   * Removes film given as an argument
   * @param film to remove
   */
  deleteFilm(film: Film) {
    const id = film.filmID;
    const delUrl = `${this.url}/${id}`; 
    return this.http.delete<Film>(delUrl).subscribe(
      (_) => this.films = this.films.filter( f => f !== film)
    );
  }

  /**
   * Fetches films that mathcing given title
   * @param title title to which films should match
   */
  findByTitle(title: string): Observable<Film[]> {
    const url = `${this.url}/findByTitle?title=${title}`;
    return this.http.get<Film[]>(url);
  }
  /**
   * Fethes films that have given genres
   * @param genres genres that should be fetched
   * @param amount amount of films to fetch
   */
  findByGenres(genres: string[], amount: number=30): Observable<Film[]> {
    const query = genres.map(g => `genre=${g}&`).join("");
    const url = `${this.url}/findByGenres?${query}`;
    return this.http.get<Film[]>(url);
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

}
