export class ProgramSet {
  name: string;
  zones: Zone[];
  programName: string;
  sync: boolean;
  state: ProgramState;
}

export enum ProgramState {
  Started,
  Stopped
}

export class Zone {
  name: string;
  zoneProgramName: string;
  brightness: number;
  running: boolean;
  lightCount: number;
  isv: any;
}

export class ZLM {
  zones: Zone[];
  availableZones: Zone[];
  availablePrograms: string[];
  programSets: ProgramSet[];
}
