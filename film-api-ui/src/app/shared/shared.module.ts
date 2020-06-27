import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TruncateTextPipe } from './truncate-text.pipe';
import { RouterModule } from '@angular/router';
import { MenuModule } from '../menu/menu.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

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
    MatProgressSpinnerModule
  ]
})
export class SharedModule { }
