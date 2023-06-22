import { TestBed } from '@angular/core/testing';

import { SeatConfigurationService } from './seat-configuration.service';

describe('SeatConfigurationService', () => {
  let service: SeatConfigurationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SeatConfigurationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
