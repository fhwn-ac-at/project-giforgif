import { Packet } from '../../packet';

export class TransferPropertiesPacket extends Packet {
  constructor() {
    super('TRANSFER_PROPERTIES');
  }

  public From!: string;
  public To!: string;
}

export type TTransferPropertiesPacket = TransferPropertiesPacket;
