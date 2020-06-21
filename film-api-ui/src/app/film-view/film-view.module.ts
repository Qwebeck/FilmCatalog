import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilmDescriptionComponent } from './film-description/film-description.component';
import { SharedModule } from '../shared/shared.module';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [FilmDescriptionComponent],
  imports: [
    CommonModule,
    SharedModule,
    MatButtonModule
  ],
  exports: [
    FilmDescriptionComponent
  ]
})
export class FilmViewModule { }
