import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilmDescriptionComponent } from './film-description/film-description.component';
import { SharedModule } from '../shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MainViewComponent } from './main-view/main-view.component';

@NgModule({
  declarations: [FilmDescriptionComponent, MainViewComponent],
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
