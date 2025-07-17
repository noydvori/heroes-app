export interface Hero {
  id: string;
  name: string;
  ability: string;
  startTrainingDate: Date;
  suitColors: string;
  startingPower: number;
  currentPower: number;
  trainingsToday: number;
  lastTrainingDate?: Date;
}

export interface HeroCreateRequest {
  name: string;
  ability: string;
  suitColors: string;
  startingPower: number;
}
