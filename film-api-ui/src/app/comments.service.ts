import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FilmComment } from './film-comment';
import { Film } from './film';
import { AuthenticationService } from './menu/authentication.service';
@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private url = 'https://localhost:5001/api/comments';

  constructor(
    private http: HttpClient,
    private auth: AuthenticationService
  ) { }

  /**
   * Publishing comment for film
   */
  publish(comment: string, film: Film): Observable<FilmComment> {
    console.log("In service: ", comment);
    let obj = {
      FilmID: film.filmID,
      Content: comment};
    return this.http.post<FilmComment>(this.url, obj, { headers: { Authorization: this.auth.accessToken }});
  }
}
