import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu-component/menu.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import { AppRoutingModule } from '../app-routing.module';
import { SignupFormComponent } from './signup-form/signup-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { AuthenticationCallbackComponent } from './authentication-callback/authentication-callback.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  declarations: [MenuComponent, SignupFormComponent, AuthenticationCallbackComponent],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    AppRoutingModule,
    MatInputModule,
    FormsModule,
    MatDialogModule,
    MatIconModule,
    ReactiveFormsModule,
    MatProgressBarModule
  ],
  exports: [
    MenuComponent
  ]
})
export class MenuModule { }
