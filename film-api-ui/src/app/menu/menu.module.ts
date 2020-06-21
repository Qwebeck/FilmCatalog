import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import { AppRoutingModule } from '../app-routing.module';

@NgModule({
  declarations: [MenuComponent],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    AppRoutingModule
  ],
  exports: [
    MenuComponent
  ]
})
export class MenuModule { }
