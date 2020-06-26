import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardViewComponent } from './film-dashboard/dashboard-view/dashboard-view.component';
import { EditorViewComponent } from './film-editor/editor-view/editor-view.component';
import { OktaAuthGuard } from '@okta/okta-angular';
//  OktaCallbackComponent
import { AuthenticationCallbackComponent } from './menu/authentication-callback/authentication-callback.component';
import { MainViewComponent } from './film-view/main-view/main-view.component';

const routes: Routes = [
  { path: 'implicit/callback', component: AuthenticationCallbackComponent },
  { path: 'dashboard', component: DashboardViewComponent },
  { path: 'films/:id', component: MainViewComponent },
  { path: 'film_editor/:id', component: EditorViewComponent, canActivate: [OktaAuthGuard] },
  { path: 'film_editor', component: EditorViewComponent, canActivate: [OktaAuthGuard] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
