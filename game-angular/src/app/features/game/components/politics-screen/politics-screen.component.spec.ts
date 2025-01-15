import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PoliticsScreenComponent } from './politics-screen.component';

describe('PoliticsScreenComponent', () => {
  let component: PoliticsScreenComponent;
  let fixture: ComponentFixture<PoliticsScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PoliticsScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PoliticsScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
