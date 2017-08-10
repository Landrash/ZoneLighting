import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ControlType, ZLRangeType } from '../../models';
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
  get type() {
    if (this.inputForm.controls['type'].value === "System.Int32") {
      return ZLRangeType.Integer;
    } else if (this.inputForm.controls['type'].value === "System.Decimal" ||
      this.inputForm.controls['type'].value === "System.Double") {
      return ZLRangeType.Decimal;
    } else {
      throw new Error("Unsupported");
    }
  }

  private getScaledValue(input: any) {
    if (this.inputForm.controls['type'].value === "System.Int32") {
      return input;
    }
    else if (this.inputForm.controls['type'].value === "System.Decimal" ||
      this.inputForm.controls['type'].value === "System.Double") {
      return input * 100;
    } else {
      throw new Error("Unsupported");
    }
  }

  private controlTypeEnum = ControlType;
  private zlRangeTypeEnum = ZLRangeType;


  constructor() {
  }
}
