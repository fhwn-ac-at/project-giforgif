import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  exports: [FormsModule, ReactiveFormsModule, CommonModule],
})
export class SharedModule {}