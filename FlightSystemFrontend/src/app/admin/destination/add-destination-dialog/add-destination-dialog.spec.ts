import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDestinationDialog } from './add-destination-dialog';

describe('AddDestinationDialog', () => {
  let component: AddDestinationDialog;
  let fixture: ComponentFixture<AddDestinationDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddDestinationDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDestinationDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
