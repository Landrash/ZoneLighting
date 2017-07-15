import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { FormGroup, FormArray, FormControl } from '@angular/forms'
import 'rxjs/add/operator/map';
import { ZLM } from '../../models';
import { FormProvider } from '../../providers'
import {ControlType} from '../../models'

@Injectable()
export class HelperProvider {

  constructor(public http: Http) {
    
  }

  public resolveControlTypeForInput(input: any) : ControlType {
    if (input.type === "System.String") {
      return ControlType.Text;
    }
    else if (input.type === "System.Int32" && !!input.min && !!input.max) {
      return ControlType.Range;
    }
    else if (input.type === "System.Int32") {
      return ControlType.Number;
    }
    else if (input.type === "System.Double") {
      return ControlType.Number;
    }

  }
}


