import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeatConfigurationHeaderComponent } from './seat-configuration-header.component';

describe('SeatConfigurationHeaderComponent', () => {
  let component: SeatConfigurationHeaderComponent;
  let fixture: ComponentFixture<SeatConfigurationHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeatConfigurationHeaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeatConfigurationHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
