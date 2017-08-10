import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { DynamicInputComponent } from './dynamic-input';
import { ZlRangeComponentModule } from './../zl-range/zl-range.module';

@NgModule({
  declarations: [
    DynamicInputComponent,
  ],
  imports: [
    IonicModule,
    ZlRangeComponentModule
  ],
  exports: [
    DynamicInputComponent
  ]
})
export class DynamicInputComponentModule {}
