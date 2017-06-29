import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ZLM, Zone, ProgramSet } from '../../models';


@Injectable()
export class ApiProvider {

  constructor(private http: Http,
    private fb: FormBuilder) {
  }

  form: FormGroup;

  private getZLMFormGroup(zlm: ZLM): FormGroup {
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
        zones: [programSet.zones],
        programName: [programSet.programName],
        sync: [programSet.sync],
        state: [programSet.state]
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
        isv: [zone.isv]
      }))
    );

    return returnValue;
  }


  public getZLM() {
    this.http.get('http://localhost:9999/ZLM/GetZLM').map(res => <ZLM>res.json()).subscribe(zlm => {
      this.form = this.getZLMFormGroup(zlm);
      console.log(this.form);
    });
  }
}
