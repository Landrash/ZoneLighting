import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ZLMFormProvider } from '../../providers'
import { OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage implements OnInit {

  get zlmForm(): FormGroup {
    return this.zlmFormProvider.getZLMForm();
  }

  constructor(public navCtrl: NavController,
    private zlmFormProvider: ZLMFormProvider) {
  }

  ngOnInit() {
    this.zlmFormProvider.loadZLM();
  }

  toggleDetails(item) {
    if (item.showDetails) {
      //item.showDetails = false;
      item.showDetails = true;
      item.icon = 'ios-add-circle-outline';
    } else {
      item.showDetails = true;
      item.icon = 'ios-remove-circle-outline';
    }
  }
}
