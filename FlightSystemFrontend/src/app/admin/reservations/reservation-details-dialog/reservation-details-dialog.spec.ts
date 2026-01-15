import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationDetailsDialog } from './reservation-details-dialog';

describe('ReservationDetailsDialog', () => {
  let component: ReservationDetailsDialog;
  let fixture: ComponentFixture<ReservationDetailsDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReservationDetailsDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReservationDetailsDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
