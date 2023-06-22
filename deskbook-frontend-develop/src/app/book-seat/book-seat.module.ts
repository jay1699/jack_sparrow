import { CommonModule } from '@angular/common';
import { BookSeatRoutingModule } from './book-seat-routing.module';
import { BookSeatComponent } from './book-seat/book-seat.component';
import { BookSeatHeaderComponent } from './book-seat-header/book-seat-header.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import {
  NgbAlertModule,
  NgbDatepickerModule,
  NgbModule,
  NgbPopoverModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { FilterBookseatPipe } from './filter-bookseat.pipe';

@NgModule({
  declarations: [
    BookSeatComponent,
    BookSeatHeaderComponent,
    FilterBookseatPipe,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    BookSeatRoutingModule,
    CommonModule,
    NgbModule,
    NgSelectModule,
    NgbDatepickerModule,
    NgbAlertModule,
    NgbPopoverModule,
  ],
  providers: [],
})
export class BookSeatModule {}
