import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { Film } from './film';

@Injectable({
  providedIn: 'root'
})
export class FilmService {

  films: Film[] = [
    { id: 1 ,title: 'Interstellar', reviewAuthor: "John", description: "Like the great space epics of the past, Christopher Nolan’s “Interstellar” distills terrestrial anxieties and aspirations into a potent pop parable, a mirror of the mood down here on Earth. Stanley Kubrick’s “2001: A Space Odyssey” blended the technological awe of the Apollo era with the trippy hopes and terrors of the Age of Aquarius. George Lucas’s first “Star Wars” trilogy, set not in the speculative future but in the imaginary past, answered the malaise of the ’70s with swashbuckling nostalgia. “Interstellar,” full of visual dazzle, thematic ambition, geek bait and corn (including the literal kind), is a sweeping, futuristic adventure driven by grief, dread and regret."},
    { id: 2, title: 'Django', reviewAuthor: "John", description: ""},
    { id: 3, title: 'Dunkerk', reviewAuthor: "John", description: ""},
  ]
  constructor() { }
  
  getFilms(): Observable<Film[]> {
    return of(this.films);
  }

  getFilm(id: number): Observable<Film> {
    return of(this.films.find( f => f.id == id));
  }
}
