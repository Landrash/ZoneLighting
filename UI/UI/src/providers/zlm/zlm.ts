import { Injectable } from '@angular/core';
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

        (<FormArray>(this.formProvider.getZLMForm().controls['programSets'])).controls.forEach(programSet => {
          (<FormArray>(<FormGroup>programSet).controls['zones']).controls.forEach(zone => {
            zone.valueChanges.debounceTime(this.debounceTime).subscribe(value => {
              this.setInputs(programSet.value.name, value.inputs).subscribe(response => console.log(response));
            });
          });

          programSet.valueChanges.debounceTime(this.debounceTime).subscribe(value => {

            Object.keys(value.inputs).forEach(inputName => {
              if (value.inputs[inputName].type === "System.Int32" ||
                value.inputs[inputName].type === "System.Double") {
                value.inputs[inputName].value = Number(value.inputs[inputName].value);
              }
            });

            this.setInputs(programSet.value.name, value.inputs).subscribe(response => console.log(response));
          });
        });

        this.slimLoadingBarService.complete();
      },
      err => console.log(err));
  }

  public setInputs(programSet: string, inputs: any) {

    Object.keys(inputs).forEach(key => {
      inputs[key] = inputs[key].value;
    });

    return this.http.post(this.zlmURL + '/SetInputs', [programSet, inputs]).map(res => <ZLM>res.json());
  }
}
