import { TestBed } from '@angular/core/testing';

import { BookSeatService } from './book-seat.service';

describe('BookSeatService', () => {
  let service: BookSeatService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookSeatService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
