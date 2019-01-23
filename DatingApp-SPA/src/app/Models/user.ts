import { Photo } from './Photo';

export interface User {
  id: string;
  userName: string;
  knownAs: string;
  age: number;
  gander: string;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  intrests?: string;
  introduction?: string;
  lookingFor?: string;
  photos?: Photo[];
}
