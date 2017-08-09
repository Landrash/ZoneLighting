import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';

import { MyApp } from './app.component';
import { HomePage } from '../pages/home/home';
import { ListPage } from '../pages/list/list';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { ApiProvider, FormProvider, ZLMFormProvider } from '../providers';
import { HttpModule } from '@angular/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HelperProvider } from '../providers/helper/helper';
import { DynamicInputComponent } from '../components/dynamic-input/dynamic-input';
import { ZlRangeComponent } from '../components/zl-range/zl-range';

@NgModule({
  declarations: [
    MyApp,
    HomePage,
    ListPage,
    DynamicInputComponent,
    ZlRangeComponent
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(MyApp),
    ReactiveFormsModule,
    FormsModule,
    HttpModule,
    SlimLoadingBarModule.forRoot()
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    HomePage,
    ListPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    ApiProvider,
    FormProvider,
    ZLMFormProvider,
    HelperProvider
  ]
})
export class AppModule {}
