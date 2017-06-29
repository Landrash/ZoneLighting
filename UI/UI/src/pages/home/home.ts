import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ApiProvider } from '../../providers'
import { ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage implements OnInit {
  ngOnInit() {
    this.api.getZLM();
  }

  constructor(public navCtrl: NavController,
  private api: ApiProvider) {

  }
}
