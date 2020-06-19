import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilmDescriptionComponent } from './film-description/film-description.component';



@NgModule({
  declarations: [FilmDescriptionComponent],
  imports: [
    CommonModule
  ],
  exports: [
    FilmDescriptionComponent
  ]
})
export class FilmViewModule { }
