import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, ControlValueAccessor } from '@angular/forms';
import { ControlType, ZLRangeType } from '../../models';
import { List, Item, Range } from 'ionic-angular';

@Component({
  selector: 'zl-range',
  templateUrl: 'zl-range.html'
})
export class ZlRangeComponent implements ControlValueAccessor {

  @Input() formControl: FormGroup;
  @Input() color: string;
  @Input() pin: boolean;
  @Input() type: ZLRangeType;

  private model: number;

  private setModel(value: any) {
    this.model = value / 100.00;
    this.propagateChange(this.model);
    this.propagateTouch(this.model);
  }

  private propagateChange = (_: any) => { };
  private propagateTouch = (_: any) => { };

  writeValue(obj): void {
    if (!!obj && this.type === ZLRangeType.Decimal) {
      this.model = obj * 100.00;
    } else {
      this.model = obj;
    }
  }

  registerOnChange(fn): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn): void {
    this.propagateTouch = fn;
  }
}
