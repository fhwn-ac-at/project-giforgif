import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyTileComponent } from './buy-tile.component';

describe('BuyTileComponent', () => {
  let component: BuyTileComponent;
  let fixture: ComponentFixture<BuyTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuyTileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuyTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
