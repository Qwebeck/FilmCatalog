import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable, throwError, from, forkJoin, pipe } from 'rxjs';
import { catchError, retry, tap, switchMap, mergeMap, map } from 'rxjs/operators';
import { Film } from '../interfaces/film';
// import { OktaAuthService } from '@okta/okta-angular';
import { AuthenticationService } from '../menu/authentication.service';
import { FilmImage } from '../interfaces/image';

interface Mark  {
  Mark: number;
  FilmID: number;
}

@Injectable({
  providedIn: 'root'
})
export class FilmService {
  private readonly url: string = "https://localhost:5001/api/films"; 
  private lastFetchedFilm: Film;
  private sampleFilm: Film = {filmID:0, title: '', addedBy: '', description: '', genre: ''}; 
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
  getFilms(withImages: boolean, offset: number = 0, amount: number = 9): Observable<Film[]> {
    let filmUrl=`${this.url}?offset=${offset*amount}&number=${amount}`;
    if ( withImages )
    return this.http.get<Film[]>(filmUrl).pipe(
      mergeMap( films => this.fetchImages(films).pipe(
        map(images => this.mapImages(images, films)),
        tap(this.log),
        catchError(this.handleError<Film[]>('getFilms', films || []))
      )));
    else
    return this.http.get<Film[]>(filmUrl).pipe(
      catchError(this.handleError<Film[]>('getFilms', []))
    );
  }
  /**
   * Get film with given id
   * @param id id of film that should be returned 
   */
  getFilm(id: number): Observable<Film> {
    if ( this.lastFetchedFilm && this.lastFetchedFilm.filmID == id ) return of(this.lastFetchedFilm); 
    const filmUrl = `${this.url}/${id}`;
    return this.http.get<Film>(filmUrl).pipe(
      tap(f => this.lastFetchedFilm = f),
      catchError(this.handleError<Film>('getFilm', {...this.sampleFilm}))
    );
  }

  /**
   * Fetches and assign images for given films
   * @param films films for which images should be fetched
   */
  assignImages(films: Film[]): Observable<Film[]> {
    return this.fetchImages(films).pipe(
              map(images => this.mapImages(images, films)),
              catchError(this.handleError<Film[]>('assignImages', films))
            );
  }

  /**
   * Fetches and assign image for film given as argument 
   * @param film 
   */
  assignImage(film: Film): Observable<Film> {
    if ( this.lastFetchedFilm && this.lastFetchedFilm.filmID == film.filmID && this.lastFetchedFilm.images.length != 0 ) return of(this.lastFetchedFilm);
    const url = `${this.url}/${film.filmID}/image`;
    return this.http.get<FilmImage[]>(url).pipe(
      map(images=>{return {...film, 'images': images.map(im => im.data)}}),
      tap( film => this.lastFetchedFilm = film ),
      catchError(this.handleError<Film>('assignImage', film))
    );
  }

  /**
   * Fetches all existing genres
   */
  getGenres(): Observable<string[]> {
    const url = `${this.url}/genres`;
    return this.http.get<string[]>(url).pipe(
      catchError(this.handleError<string[]>('getGenres', []))
    );
  }

  /**
   * Saves film
   * @param film film, that should be saved
   */
  saveFilm(film: Film): Observable<Film> {
    let obj = {
      Genre: film.genre,
      Images: film.images,
      Title: film.title,
      Description: film.description,
    };
    const headers = { Authorization: this.auth.accessToken };
    return this.http.post<Film>(this.url, obj, {headers: headers}).pipe(
            tap((_) => console.log("Saved: ", film)),
            catchError(this.handleError<Film>('saveFilm', {...this.sampleFilm})))
  }

  /**
   * Updates film
   * @param film 
   */
  updateFilm(film: Film): Observable<Film> {
    let obj = {
      Genre: film.genre,
      Images: film.images,
      Title: film.title,
      Description: film.description,
    };
    const url = `${this.url}/${film.filmID}`;
    const headers = { Authorization: this.auth.accessToken };
    return this.http.put<Film>(url, obj, {headers: headers}).pipe(
          tap((_) => console.log("Updated: ", film)),
          catchError(this.handleError<Film>('updateFilm',{...this.sampleFilm}))
          )

  }

  /**
   * Removes film given as an argument
   * @param film to remove
   */
  deleteFilm(film: Film): Observable<Film> {
    const id = film.filmID;
    const delUrl = `${this.url}/${id}`; 
    return this.http.delete<Film>(delUrl).pipe(
      catchError(this.handleError<Film>('deleteFilm'))
    )
  }

  /**
   * Fetches films that mathcing given title
   * @param title title to which films should match
   */
  findByTitle(title: string, offset: number, amount: number=9): Observable<Film[]> {
    const url = `${this.url}/findByTitle?title=${title}&offset=${offset*amount}&number=${amount}`;
    return this.http.get<Film[]>(url).pipe(
      catchError(this.handleError<Film[]>('findByTitle', []))
    );
  }
  /**
   * Fethes films that have given genres
   * @param genres genres that should be fetched
   * @param amount amount of films to fetch
   */
  findByGenres(genres: string[], offset: number, amount: number=9): Observable<Film[]> {
    const query = genres.map(g => `genre=${g}&`).join("");
    const url = `${this.url}/findByGenres?${query}&offset=${offset*amount}&number=${amount}`;
    return this.http.get<Film[]>(url).pipe(
      catchError(this.handleError<Film[]>('findByGenres', []))
    );
  }

  markFilm(filmID: number, mark: 0 | 1): Observable<Mark> {
    const url = `${this.url}/${filmID}/marks`;
    let filmMark: Mark = {
      Mark: mark,
      FilmID: filmID,
    };
    const headers = { Authorization: this.auth.accessToken };
    return this.http.put<Mark>(url, filmMark, {headers: headers});
  }

  private log(films:Film[]): void {
    this.films = films;
    console.log("Fetched: ", this.films);
  }

  private mapImages(images: FilmImage[], films: Film[]): Film[] {
    console.log("Images: ",images);
    let sortedImages = images.sort((a,b) => a.filmID - b.filmID);
    
    let j = 0;
    for (let i = 0; i < films.length; ++i) {
      if (!films[i].images) films[i].images = [];  
      if ( j < sortedImages.length && sortedImages[j].filmID == films[i].filmID ) {
        films[i].images.push(sortedImages[j].data);
          j += 1; 
      }
    }
    return films;
  }

  private fetchImages(films: Film[]): Observable<FilmImage[]> {
    const query = films.map(f => `filmid=${f.filmID}&`).join("");
    const url = `https://localhost:5001/api/images?${query}`;
    return this.http.get<FilmImage[]>(url).pipe(
      catchError(this.handleError<FilmImage[]>('fetchImages', []))
    )
  }

  private handleError<T>(operation: string, result?: T) {
    return (err: any): Observable<T> => {
      console.error(`Operation ${operation} failed with error: ${err}`);
      return of(result as T);
    }

  }
}
