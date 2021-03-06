﻿export class ProgramSet {
  name: string;
  zones: Zone[];
  programName: string;
  sync: boolean;
  state: ProgramState;
  inputs: any[];
}

export enum ProgramState {
  Started,
  Stopped
}

export class Zone {
  name: string;
  programName: string;
  brightness: number;
  running: boolean;
  lightCount: number;
  inputs: any[];
}

export class ZLM {
  zones: Zone[];
  availableZones: Zone[];
  availablePrograms: string[];
  programSets: ProgramSet[];
}


export enum ControlType {
  Text,
  Number,
  Decimal,
  Range,
  Radio,
  Toggle,
  Dropdown
}

export enum ZLRangeType {
  Decimal,
  Integer
}