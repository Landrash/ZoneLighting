﻿import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { FormGroup, FormArray, FormControl } from '@angular/forms'
import 'rxjs/add/operator/map';
import { ZLM } from '../../models';
import { FormProvider } from '../../providers'
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';

@Injectable()
export class ZLMFormProvider {

  private zlm: ZLM;
  private zlmURL = "http://localhost:9999/zlm";
  private debounceTime = 1000;

  constructor(private http: Http,
    private formProvider: FormProvider,
    private slimLoadingBarService: SlimLoadingBarService) {

  }

  public getZLMForm(): FormGroup {
    //if (!!this.formProvider.getZLMForm())
    //  return <ZLM>this.formProvider.getZLMForm().value;
    //else
    //  return null;

    return this.formProvider.getZLMForm();
  }

  public loadZLM() {
    this.slimLoadingBarService.start();
    this.http.get(this.zlmURL)
      .map(response => <ZLM>response.json())
      .subscribe(zlm => {
        console.log(zlm);
        this.formProvider.setZLMForm(zlm);
        this.formProvider.getZLMForm().valueChanges.subscribe(value => {
          console.log(value);
        });
        console.log(this.formProvider.getZLMForm());
        this.setupForm();
        this.slimLoadingBarService.complete();
      },
      err => console.log(err));
  }

  private setupForm() {
    (<FormArray>(this.formProvider.getZLMForm().controls['programSets'])).controls.forEach(programSet => {
      (<FormArray>(<FormGroup>programSet).controls['zones']).controls.forEach(zone => {
        (<FormGroup>zone).controls['inputs'].valueChanges.debounceTime(this.debounceTime).subscribe(value => {
          this.adjustAndSetInputs(value, programSet);
        });
      });

      (<FormGroup>programSet).controls['programName'].valueChanges.debounceTime(this.debounceTime).subscribe(value => {
        this.recreateProgramSet((<FormGroup>programSet).controls['name'].value,
          value.toString(),
          (<FormGroup>programSet).controls['zones'].value.map(x => x.name))
          .subscribe(
          response =>
            console.log(response));
      });

      (<FormGroup>programSet).controls['inputs'].valueChanges.debounceTime(this.debounceTime).subscribe(value => {
        this.adjustAndSetInputs(value, programSet);
      });
    });
  }

  public recreateProgramSet(programSetName: string, programName: string, zoneNames: string[], isv: any = null) {
    return this.http.post(this.zlmURL + '/RecreateProgramSet', [programSetName, programName, zoneNames])
      .map(res => <ZLM>res.json());
  }

  private adjustAndSetInputs(value: any, programSet: any) {
    Object.keys(value).forEach(inputName => {
      if (value[inputName].type === "System.Int32" ||
        value[inputName].type === "System.Double") {
        value[inputName].value = Number(value[inputName].value);
      }
    });

    this.setInputs(programSet.value.name, value).subscribe(response => console.log(response));
  }

  public setInputs(programSet: string, inputs: any) {

    Object.keys(inputs).forEach(key => {
      inputs[key] = inputs[key].value;
    });

    return this.http.post(this.zlmURL + '/SetInputs', [programSet, inputs]).map(res => <ZLM>res.json());
  }
}
