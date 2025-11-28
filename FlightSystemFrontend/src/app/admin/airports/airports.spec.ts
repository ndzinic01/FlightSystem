import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Airports } from './airports';

describe('Airports', () => {
  let component: Airports;
  let fixture: ComponentFixture<Airports>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Airports]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Airports);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
