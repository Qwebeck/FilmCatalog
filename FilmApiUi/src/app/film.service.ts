import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Film } from './film';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class FilmService {
  private filmsUrl = 'api/films';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  constructor(
    private messageService: MessageService,
    private httpClient: HttpClient) { }
  
  private log(message: string) {
    this.messageService.add(message);
  }

  private handleError<T> (methodName: string, result?: T) {
    return (error: any): Observable<T> => {
        console.error(error);
        this.log(`${methodName} failed: ${error.message} `)
        return of(result as T);
      };
  }

  getFilms (): Observable<Film[]> {
    return this.httpClient.get<Film[]>(this.filmsUrl)
            .pipe(
              tap(_ => this.log("Fetching films")),
              catchError(this.handleError<Film[]>('getFilms', []))
            );
  }

  getFilm (id: number): Observable<Film> {
    this.log(`Fetching film with id: ${id}`)
    const url = `${this.filmsUrl}/${id}`;
    return this.httpClient.get<Film>(url)
              .pipe(
                tap(_ => this.log(`Fetching film with id: ${id}`),
                catchError(this.handleError<Film>('getFilm'))
                )
    )
  }

  updateFilm(film: Film) {
    return this.httpClient.put(this.filmsUrl, film, this.httpOptions)
              .pipe(
                tap(_ => this.log("Updated film id=${film.id}")),
                catchError(this.handleError<any>('updateFilm'))
              )
  }

  addFilm( film: Film) {
    return this.httpClient.post<Film>(this.filmsUrl, film, this.httpOptions)
              .pipe(
                tap(_ => this.log(`Added film ${film.title}`)),
                catchError(this.handleError<Film>("addFilm"))
              )
  }

  deleteFilm (film: Film) {
    const id = typeof film === 'number' ? film : film.id;
    const url = `${this.filmsUrl}/${id}`;
  
    return this.httpClient.delete<Film>(url, this.httpOptions).pipe(
      tap(_ => this.log(`deleted hero id=${id}`)),
      catchError(this.handleError<Film>('deleteHero'))
    );
  }


  searchFilms(term: string): Observable<Film[]> {
      if (!term.trim()) return of([]);

      return this.httpClient.get<Film[]>(`${this.filmsUrl}/?name=${term}`).pipe(
        tap(x=> x.length 
          ? this.log("Found matchin films")
          : this.log("No films matching")),
        catchError(this.handleError<Film[]>('searchFilms', []))
      )
  }
}
