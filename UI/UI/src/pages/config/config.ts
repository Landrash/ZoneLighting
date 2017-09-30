import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { ZLMFormProvider, HelperProvider, FormProvider } from '../../providers'
import { HomePage } from '../home/home';

@Component({
  selector: 'page-config',
  templateUrl: 'config.html',
})
export class ConfigPage {

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public zlmFormProvider: ZLMFormProvider
  ) {}

  get zlmURL() {
    return this.zlmFormProvider.zlmURL;
  }

  set zlmURL(value:string) {
    this.zlmFormProvider.zlmURL = value;
  }

  navigateHome() {
    this.navCtrl.setRoot(HomePage);
  }

}
