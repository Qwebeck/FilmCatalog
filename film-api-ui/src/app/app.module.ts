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
import { OKTA_CONFIG, OktaAuthModule } from '@okta/okta-angular';

const config = {
  clientId: '0oag0zkk4PsS8o62g4x6',
  issuer: 'https://dev-221155.okta.com/oauth2/default',
  redirectUri: 'http://localhost:4200/implicit/callback',
  scopes: ['openid', 'profile', 'email'],
  pkce: true
};


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    OktaAuthModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FilmDashboardModule,
    FilmViewModule,
    MenuModule,
    HttpClientModule,
    FilmEditorModule
  ],
  providers: [
    { provide: OKTA_CONFIG, useValue: config}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
