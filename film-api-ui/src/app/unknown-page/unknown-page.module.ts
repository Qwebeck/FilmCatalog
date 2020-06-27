import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouteNotFoundViewComponent } from './route-not-found-view/route-not-found-view.component';
import { SharedModule } from '../shared/shared.module'; 
import {MatButtonModule} from '@angular/material/button';

@NgModule({
  declarations: [RouteNotFoundViewComponent],
  imports: [
    CommonModule,
    MatButtonModule,
    SharedModule
  ]
})
export class UnknownPageModule { }
