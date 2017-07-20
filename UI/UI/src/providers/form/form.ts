import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ZLM, Zone, ProgramSet } from '../../models';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class FormProvider {

  private zlmForm: FormGroup = null;

  constructor(private fb: FormBuilder) {
  }

  public setZLMForm(zlm: ZLM) {
    this.zlmForm = this.populateZLMFormGroup(zlm);
  }

  public getZLMForm(): FormGroup {
    return this.zlmForm;
  }

  public populateZLMFormGroup(zlm: ZLM): FormGroup {
    return this.fb.group({
      availablePrograms: this.fb.array(zlm.availablePrograms),
      availableZones: this.fb.array(this.getZoneGroups(zlm.availableZones)),
      programSets: this.fb.array(this.getProgramSetGroups(zlm.programSets)),
      zones: this.fb.array(this.getZoneGroups(zlm.zones))
    });
  }

  private getProgramSetGroups(programSets: ProgramSet[]): FormGroup[] {
    var returnValue = Array<FormGroup>();

    programSets.forEach(programSet =>
      returnValue.push(this.fb.group({
        name: [programSet.name],
        zones: this.fb.array(this.getZoneGroups(programSet.zones)),
        programName: [programSet.programName],
        sync: [programSet.sync],
        state: [programSet.state],
        inputs: this.getZoneInputGroups(programSet.inputs)
      }))
    );

    return returnValue;
  }

  private getZoneGroups(zones: Zone[]): FormGroup[] {
    var returnValue = Array<FormGroup>();

    zones.forEach(zone =>
      returnValue.push(this.fb.group({
        name: [zone.name],
        zoneProgramName: [zone.zoneProgramName],
        brightness: [zone.brightness],
        running: [zone.running],
        lightCount: [zone.lightCount],
        inputs: this.getZoneInputGroups(zone.inputs)
      }))
    );

    return returnValue;
  }

  private getZoneInputGroups(inputs: any[]): FormGroup {
    var returnValue = new FormGroup({});

    Object.keys(inputs).forEach(key => {
      returnValue.addControl(key, this.getZoneInputGroup(inputs[key]));
    });
    
    return returnValue;
  }

  private getZoneInputGroup(input: any) {
    var returnValue = new FormGroup({});

    returnValue.addControl("value", new FormControl(input.value, Validators.required));
    if (!!input.min)
      returnValue.addControl("min", new FormControl(input.min, Validators.nullValidator));
    if (!!input.max)
      returnValue.addControl("max", new FormControl(input.max, Validators.nullValidator));
    if (!!input.type)
      returnValue.addControl("type", new FormControl(input.type, Validators.nullValidator));
    if (!!input.options)
      returnValue.addControl("options", new FormControl(input.options, Validators.nullValidator));
    
    return returnValue;
  }
}
