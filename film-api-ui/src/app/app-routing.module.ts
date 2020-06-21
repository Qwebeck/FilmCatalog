import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './film-dashboard/dashboard/dashboard.component';
import { FilmDescriptionComponent } from './film-view/film-description/film-description.component';
import { EditorViewComponent } from './film-editor/editor-view/editor-view.component';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'films/:id', component: FilmDescriptionComponent },
  { path: 'film_editor', component: EditorViewComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
