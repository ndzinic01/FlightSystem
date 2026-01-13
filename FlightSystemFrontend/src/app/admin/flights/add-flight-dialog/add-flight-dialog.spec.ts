import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFlightDialog } from './add-flight-dialog';

describe('AddFlightDialog', () => {
  let component: AddFlightDialog;
  let fixture: ComponentFixture<AddFlightDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddFlightDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddFlightDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
