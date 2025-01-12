import { TAddMoneyPacket } from './game/actions/add-money';
import { TRemoveMoneyPacket } from './game/actions/remove-money';
import { TAuctionResultPacket } from './game/auction/auction-result';
import { TAuctionStartPacket } from './game/auction/auction-start';
import { TAuctionUpdatePacket } from './game/auction/auction-update';
import { TRolledDicePacket } from './game/dice/rolled-dice';
import { TGameStatePacket } from './game/game-state';
import { THouseBuiltPacket } from './game/house/house-built';
import { THouseSoldPacket } from './game/house/house-sold';
import { TGoToJailPacket } from './game/jail/go-to-jail';
import { TPayoutSucessPacket } from './game/jail/payout-sucess';
import { TPayPlayerPacket } from './game/player/pay-player';
import { TBoughtFieldPacket } from './game/sites/bought-field';
import { TBuyRequestPacket } from './game/sites/buy-request';
import { TPropertySoldPacket } from './game/sites/property-sold';
import { TTransferPropertiesPacket } from './game/sites/transfer-properties';
import { TBankurptcyPacket } from './game/util/bankruptcy';
import { TPlayerOutOfDebtPacket } from './game/util/out-of-debt';
import { TSellPropertiesPacket } from './game/util/sell-properties';
import { TStartPacket } from './lobby/start';
import { TStatusPacket } from './lobby/status';
import { Packet } from './packet';
import { TPlayerJoinedPacket } from './rooms/player-joined';
import { TPlayerLeftPacket } from './rooms/player-left';
import { TRoomsUpdatedPacket } from './rooms/rooms-updated';
import { TErrorPacket } from './util/error';

type PacketParserFunction = (obj: any) => Packet;

const packetTypeMap: { [key: string]: PacketParserFunction } = {
  // SAMPLE: (obj: any) => obj as SamplePacket,
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
  // Add entries for other packet types
};

export { packetTypeMap };
