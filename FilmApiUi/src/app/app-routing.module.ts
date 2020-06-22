import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FilmCreatorComponent } from './film-creator/film-creator.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FilmDetailsComponent } from './film-details/film-details.component';
const routes: Routes = [
  {path: 'films', component: FilmCreatorComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'description/:id', component: FilmDetailsComponent},
  {path: '', redirectTo: 'dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
