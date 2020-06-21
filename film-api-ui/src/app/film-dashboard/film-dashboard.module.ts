import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '@angular/cdk/layout';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { FilterComponent } from './filter/filter.component';
import { SharedModule } from '../shared/shared.module';
import { DashboardViewComponent } from './dashboard-view/dashboard-view.component';
import { FilmSearchComponent } from './film-search/film-search.component';

@NgModule({
  declarations: [DashboardComponent, FilterComponent, DashboardViewComponent, FilmSearchComponent],
  imports: [
    CommonModule,
    LayoutModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    AppRoutingModule,
    MatSidenavModule,
    MatInputModule,
    FormsModule,
    MatSelectModule,
    SharedModule
  ],
  exports: [
    DashboardComponent
  ]
})
export class FilmDashboardModule { }
