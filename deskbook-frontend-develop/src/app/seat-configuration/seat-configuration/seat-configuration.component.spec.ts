import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeatConfigurationComponent } from './seat-configuration.component';

describe('SeatConfigurationComponent', () => {
  let component: SeatConfigurationComponent;
  let fixture: ComponentFixture<SeatConfigurationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeatConfigurationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeatConfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
