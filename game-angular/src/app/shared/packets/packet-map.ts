import {
  Packet,
  TAddMoneyPacket,
  TAuctionResultPacket,
  TAuctionStartPacket,
  TAuctionUpdatePacket,
  TBankurptcyPacket,
  TBoughtFieldPacket,
  TBuyingPriceIncreasePacket,
  TBuyRequestPacket,
  TDrawChancePacket,
  TDrawChestPacket,
  TErrorPacket,
  TGameStatePacket,
  TGoToJailPacket,
  THouseBuiltPacket,
  THouseSoldPacket,
  TMovePlayerPacket,
  TNewPoliticPacket,
  TPayoutSucessPacket,
  TPayPlayerPacket,
  TPlayerJoinedPacket,
  TPlayerLeftPacket,
  TPlayerOutOfDebtPacket,
  TPropertySoldPacket,
  TRemoveMoneyPacket,
  TRentIncreasePacket,
  TRolledDicePacket,
  TRoomsUpdatedPacket,
  TSellPropertiesPacket,
  TStartPacket,
  TStatusPacket,
  TTransferPropertiesPacket,
  TWonPacket,
} from '.';
import { TResetPoliticPacket } from './game/politics/reset-politic';

type PacketParserFunction = (obj: any) => Packet;

const packetTypeMap: { [key: string]: PacketParserFunction } = {
  ROOMS_UPDATED: (obj: any) => obj as TRoomsUpdatedPacket,
  PLAYER_JOINED: (obj: any) => obj as TPlayerJoinedPacket,
  ERROR: (obj: any) => obj as TErrorPacket,
  STATUS: (obj: any) => obj as TStatusPacket,
  PLAYER_LEFT: (obj: any) => obj as TPlayerLeftPacket,
  START: (obj: any) => obj as TStartPacket,
  ROLLED: (obj: any) => obj as TRolledDicePacket,
  PLAYERS_TURN: (obj: any) => obj as TRolledDicePacket,
  GAME_STATE: (obj: any) => obj as TGameStatePacket,
  BUY_REQUEST: (obj: any) => obj as TBuyRequestPacket,
  BOUGHT_FIELD: (obj: any) => obj as TBoughtFieldPacket,
  PAY_PLAYER: (obj: any) => obj as TPayPlayerPacket,
  AUCTION_START: (obj: any) => obj as TAuctionStartPacket,
  AUCTION_UPDATE: (obj: any) => obj as TAuctionUpdatePacket,
  AUCTION_RESULT: (obj: any) => obj as TAuctionResultPacket,
  HOUSE_BUILT: (obj: any) => obj as THouseBuiltPacket,
  GO_TO_JAIL: (obj: any) => obj as TGoToJailPacket,
  PAYOUT_SUCESS: (obj: any) => obj as TPayoutSucessPacket,
  ADD_MONEY: (obj: any) => obj as TAddMoneyPacket,
  REMOVE_MONEY: (obj: any) => obj as TRemoveMoneyPacket,
  BANKRUPTCY: (obj: any) => obj as TBankurptcyPacket,
  SELL_PROPERTIES: (obj: any) => obj as TSellPropertiesPacket,
  PLAYER_OUT_OF_DEBT: (obj: any) => obj as TPlayerOutOfDebtPacket,
  PROPERTY_SOLD: (obj: any) => obj as TPropertySoldPacket,
  HOUSE_SOLD: (obj: any) => obj as THouseSoldPacket,
  TRANSFER_PROPERTIES: (obj: any) => obj as TTransferPropertiesPacket,
  WON: (obj: any) => obj as TWonPacket,
  MOVE_PLAYER: (obj: any) => obj as TMovePlayerPacket,
  DRAW_CHANCE: (obj: any) => obj as TDrawChancePacket,
  DRAW_CHEST: (obj: any) => obj as TDrawChestPacket,
  NEW_POLITIC: (obj: any) => obj as TNewPoliticPacket,
  POLITIC_RESET: (obj: any) => obj as TResetPoliticPacket,
  BUYING_PRICE_INCREASE: (obj: any) => obj as TBuyingPriceIncreasePacket,
  RENT_INCREASE: (obj: any) => obj as TRentIncreasePacket,
};

export { packetTypeMap };
