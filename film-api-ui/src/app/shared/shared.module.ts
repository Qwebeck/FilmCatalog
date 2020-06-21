import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TruncateTextPipe } from './truncate-text.pipe';
import { RouterModule } from '@angular/router';
@NgModule({
  declarations: [
    TruncateTextPipe
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    TruncateTextPipe,
    RouterModule
  ]
})
export class SharedModule { }
