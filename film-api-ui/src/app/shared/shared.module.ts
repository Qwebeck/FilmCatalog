import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TruncateTextPipe } from './truncate-text.pipe';
import { RouterModule } from '@angular/router';
import { MenuModule } from '../menu/menu.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  declarations: [
    TruncateTextPipe,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    TruncateTextPipe,
    RouterModule,
    MenuModule,
    MatProgressBarModule
  ]
})
export class SharedModule { }
