import { Field } from './fields';

export interface BoardConfig {
  politics: string[];
  jailScreen: string;
  winScreen: string;
  bankruptScreen: string;
  chances: string[];
  chests: string[];
  fields: Field[];
}
