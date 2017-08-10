import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { FormGroup, FormArray, FormControl } from '@angular/forms'
import 'rxjs/add/operator/map';
import { ZLM } from '../../models';
import { FormProvider } from '../../providers'
import { ControlType } from '../../models'

@Injectable()
export class HelperProvider {

  constructor(public http: Http) {

  }

  public hasValue(input: any) {
    return input !== null && input !== undefined;
  }

  public resolveControlTypeForInput(input: any): ControlType {
    if (input.type === "System.String") {
      return ControlType.Text;
    }
    else if ((input.type === "System.Int32" || input.type === "System.Double" || input.type === "System.Decimal") && this.hasValue(input.min) && this.hasValue(input.max)) {
      return ControlType.Range;
    }
    else if (input.type === "System.Int32") {
      return ControlType.Number;
    }
    else if (input.type === "System.Double" || input.Type === "System.Decimal") {
      return ControlType.Decimal;
    }
    else if (input.type === "System.Boolean") {
      return ControlType.Toggle;
    }
    else if (input.type === "System.Enum") {
      return ControlType.Dropdown;
    }
    else {
      return ControlType.Text;
    }
  }
}
