export interface Hero {
  id: string;
  name: string;
  ability: string;
  startTrainingDate: Date;
  suitColors: string;
  startingPower: number;
  currentPower: number;
  trainerId: string;
}

export interface HeroCreateRequest {
  name: string;
  ability: string;
  suitColors: string;
  startingPower: number;
}

export interface HeroUpdateRequest {
  id: string;
  name: string;
  ability: string;
  suitColors: string;
  startingPower: number;
}
