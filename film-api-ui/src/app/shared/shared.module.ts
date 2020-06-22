import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TruncateTextPipe } from './truncate-text.pipe';
import { RouterModule } from '@angular/router';
import { MenuModule } from '../menu/menu.module';

@NgModule({
  declarations: [
    TruncateTextPipe,
  ],
  imports: [
    CommonModule,
    // MenuModule
  ],
  exports: [
    TruncateTextPipe,
    RouterModule,
    MenuModule
  ]
})
export class SharedModule { }
