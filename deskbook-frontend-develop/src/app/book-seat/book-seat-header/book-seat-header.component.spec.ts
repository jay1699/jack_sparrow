import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSeatHeaderComponent } from './book-seat-header.component';

describe('BookSeatHeaderComponent', () => {
  let component: BookSeatHeaderComponent;
  let fixture: ComponentFixture<BookSeatHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookSeatHeaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookSeatHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
