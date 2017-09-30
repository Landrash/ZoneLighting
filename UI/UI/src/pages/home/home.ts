import { Component, Pipe, PipeTransform } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ZLMFormProvider, HelperProvider, FormProvider } from '../../providers'
import { OnInit } from '@angular/core';
import 'rxjs/add/operator/debounceTime';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ControlType } from '../../models'
import { Http } from '@angular/http';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { ZLM, Zone, ProgramSet } from '../../models';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage implements OnInit {
  
  get zlmForm(): FormGroup {
    return this.zlmFormProvider.getZLMForm();
  }

  log(val) { console.log(val); }

  private objectType = Object;

  private controlTypeEnum = ControlType;

  constructor(public navCtrl: NavController,
    private zlmFormProvider: ZLMFormProvider,
    private helperProvider: HelperProvider) {
  }

  ngOnInit() {
    this.zlmFormProvider.loadZLM();
  }

  toggleDetails(item) {
    if (item.hideDetails) {
      item.hideDetails = false;
      //item.icon = 'ios-remove-circle-outline';
    } else {
      item.hideDetails = true;
      //item.icon = 'ios-add-circle-outline';
    }
  }

  navigateHome() {
    this.navCtrl.setRoot(HomePage);
  }
}
