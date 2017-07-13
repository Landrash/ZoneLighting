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

        (<FormArray>(this.formProvider.getZLMForm().controls['programSets'])).controls.forEach(programSet => {
          (<FormArray>(<FormGroup>programSet).controls['zones']).controls.forEach(zone => {

            zone.valueChanges.debounceTime(200).subscribe(value => {
              //value.field.replace()
              this.setInputs(programSet.value.name, value.inputs).subscribe(response => console.log(response));
            });

            //(<FormArray>(<FormGroup>zone).controls['inputs']).controls.forEach(input => {
            //  input.valueChanges.subscribe(value => {
            //    this
            //  });
            //});
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
    
    return this.http.post('http://localhost:9999/ZLM/SetInputs', [programSet, inputs]).map(res => <ZLM>res.json());
  }
}
