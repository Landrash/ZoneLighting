import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ControlType } from '../../models';
import { List, Item, Range } from 'ionic-angular';

@Component({
  selector: 'dynamic-input',
  templateUrl: 'dynamic-input.html'
})
export class DynamicInputComponent implements AfterViewInit {
  ngAfterViewInit(): void {
    if (!!this.inputForm.controls['options']) {
      console.log(this.inputForm.controls['options'].value);
    }
  }


  @Input() inputForm: FormGroup;
  @Input() controlType: ControlType;
  @Input() name: string;

  private controlTypeEnum = ControlType;

  constructor() {
  }
}
