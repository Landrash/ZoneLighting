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
    console.log(this.controlType);
  }


  @Input() inputForm: FormGroup;
  @Input() controlType: ControlType;
  @Input() name: string;

  private controlTypeEnum = ControlType;

  constructor() {
  }
}
