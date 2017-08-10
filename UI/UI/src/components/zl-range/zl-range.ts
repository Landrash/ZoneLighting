import { Component, OnInit, Input, AfterViewInit, forwardRef } from '@angular/core';
import { FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';
import { ControlType, ZLRangeType } from '../../models';
import { List, Item, Range } from 'ionic-angular';

@Component({
  selector: 'zl-range',
  templateUrl: 'zl-range.html'
})
export class ZlRangeComponent {

  @Input() control: FormControl;
  @Input() max: number;
  @Input() min: number;
  @Input() type: ZLRangeType;

  set selectedValue(inValue: any) {

    if (inValue === this.selectedValue)
      return;

    if (this.type === ZLRangeType.Decimal)
      this.control.setValue(inValue / 100.00);
    else {
      this.control.setValue(inValue);
    }
  }

  get selectedValue(): any {
    if (this.control == null)
      return null;
    else {
      if (this.type === ZLRangeType.Decimal) {
        return this.control.value * 100.00;
      } else {
        return this.control.value;
      }
    }
  }
}
