import { Component,Pipe, PipeTransform  } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ZLMFormProvider, HelperProvider } from '../../providers'
import { OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import 'rxjs/add/operator/debounceTime';
import { ControlType } from '../../models'

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage implements OnInit {

  get zlmForm(): FormGroup {
    return this.zlmFormProvider.getZLMForm();
  }

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
    if (item.showDetails) {
      item.showDetails = true;
      item.icon = 'ios-add-circle-outline';
    } else {
      item.showDetails = true;
      item.icon = 'ios-remove-circle-outline';
    }
  }
}
