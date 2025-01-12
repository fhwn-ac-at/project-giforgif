import { FieldType } from './field-types';

interface BaseField {
  type: FieldType;
  name: string;
  buyingPrice?: number;
  buildingPrice?: number;
  rentPrices?: number[];
  amount?: number;
}

interface GoField extends BaseField {
  type: 'Go';
}

interface CommunityChestField extends BaseField {
  type: 'CommunityChest';
}

interface ChanceField extends BaseField {
  type: 'Chance';
}

interface JailField extends BaseField {
  type: 'Jail';
}

interface FreeParkingField extends BaseField {
  type: 'FreeParking';
}

interface GoToJailField extends BaseField {
  type: 'GoToJail';
}

interface TaxField extends BaseField {
  type: 'Tax';
}

interface StationField extends BaseField {
  type: 'Station';
}

interface UtilityField extends BaseField {
  type: 'Utility';
}

interface SiteField extends BaseField {
  type: 'Site';
}

export type Field =
  | GoField
  | CommunityChestField
  | ChanceField
  | JailField
  | FreeParkingField
  | GoToJailField
  | TaxField
  | StationField
  | UtilityField
  | SiteField;
