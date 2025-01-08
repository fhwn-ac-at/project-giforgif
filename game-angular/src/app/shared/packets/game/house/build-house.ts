import { Packet } from '../../packet';

export class BuildHousePacket extends Packet {
  constructor() {
    super('BUILD_HOUSE');
  }

  public FieldId!: number;
}

export type TBuildHousePacket = BuildHousePacket;
