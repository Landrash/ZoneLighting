import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ZLM, Zone, ProgramSet } from '../../models';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ApiProvider {

  constructor(private http: Http,
    private fb: FormBuilder) {
  }

  public getZLM(): Observable<ZLM> {
    return this.http.get('http://localhost:9999/ZLM/GetZLM').map(res => <ZLM>res.json());
  }

  public setInputs(programSet: string, value: any[]) {
    return this.http.post('http://localhost:9999/ZLM/SetInputs', [ programSet, value]).map(res => <ZLM>res.json());
  }
}
