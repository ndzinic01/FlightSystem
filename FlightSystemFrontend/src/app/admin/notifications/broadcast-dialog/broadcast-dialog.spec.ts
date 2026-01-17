import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BroadcastDialog } from './broadcast-dialog';

describe('BroadcastDialog', () => {
  let component: BroadcastDialog;
  let fixture: ComponentFixture<BroadcastDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BroadcastDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BroadcastDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
