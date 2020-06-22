import { Injectable } from '@angular/core';
import { InMemoryDbService } from 'angular-in-memory-web-api';
import { Film } from './film';


@Injectable({
  providedIn: 'root',
})
export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const films = [
      { id: 1, title: "Good film"},
      { id: 2, title: "Good film"},
      { id: 3, title: "Good film"},
      { id: 4, title: "Good film"},
      { id: 5, title: "Good film"},
      { id: 6, title: "Good film"},
      { id: 7, title: "Some film"},
      { id: 8, title: "Bad film"},
      { id: 9, title: "Perfect film"},
      { id: 10, title: "Great film"}
    ];
    return {films};
  }

  // Overrides the genId method to ensure that a hero always has an id.
  // If the heroes array is empty,
  // the method below returns the initial number (11).
  // if the heroes array is not empty, the method below returns the highest
  // hero id + 1.
  genId(heroes: Film[]): number {
    return heroes.length > 0 ? Math.max(...heroes.map(hero => hero.id)) + 1 : 11;
  }
}