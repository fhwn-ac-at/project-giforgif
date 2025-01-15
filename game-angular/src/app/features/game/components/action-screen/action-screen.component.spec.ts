import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionScreenComponent } from './action-screen.component';

describe('ActionScreenComponent', () => {
  let component: ActionScreenComponent;
  let fixture: ComponentFixture<ActionScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActionScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActionScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
