import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { DynamicInputComponent } from './dynamic-input';

@NgModule({
  declarations: [
    DynamicInputComponent,
  ],
  imports: [
    IonicModule,
  ],
  exports: [
    DynamicInputComponent
  ]
})
export class DynamicInputComponentModule {}
