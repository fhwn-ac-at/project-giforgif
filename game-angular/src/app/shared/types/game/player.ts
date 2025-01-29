export type Player = {
  name: string;
  currency: number;
  color: 'red' | 'green' | 'yellow' | 'blue' | 'fuchsia';
  currentPosition: number,
  isInJail: boolean,
  owns: number[],
};
