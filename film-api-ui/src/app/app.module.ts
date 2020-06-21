import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FilmEditorModule } from './film-editor/film-editor.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FilmDashboardModule } from './film-dashboard/film-dashboard.module';
import { FilmViewModule } from './film-view/film-view.module';
import { MenuModule } from './menu/menu.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FilmDashboardModule,
    FilmViewModule,
    MenuModule,
    HttpClientModule,
    FilmEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
