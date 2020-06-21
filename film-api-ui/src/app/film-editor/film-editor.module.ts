import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilmEditorComponent } from './film-editor/film-editor.component';
import { EditorViewComponent } from './editor-view/editor-view.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import {MatButtonModule} from '@angular/material/button';
import { FilmViewModule } from '../film-view/film-view.module';
@NgModule({
  declarations: [FilmEditorComponent, EditorViewComponent],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatGridListModule,
    MatSelectModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    FilmViewModule
  ],
  exports: [ 
    EditorViewComponent
  ]
})
export class FilmEditorModule { }
