import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './film-dashboard/dashboard/dashboard.component';
import { FilmDescriptionComponent } from './film-view/film-description/film-description.component';


const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'films/:id', component: FilmDescriptionComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
