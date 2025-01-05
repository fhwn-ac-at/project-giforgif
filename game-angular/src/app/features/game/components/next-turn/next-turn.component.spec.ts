import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NextTurnComponent } from './next-turn.component';

describe('NextTurnComponent', () => {
  let component: NextTurnComponent;
  let fixture: ComponentFixture<NextTurnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NextTurnComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NextTurnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
