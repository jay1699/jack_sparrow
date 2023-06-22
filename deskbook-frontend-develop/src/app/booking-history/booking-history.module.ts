import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BookingHistoryRoutingModule } from './booking-history-routing.module';
import { BookingHistoryComponent } from './booking-history/booking-history.component';
import { NgSelectModule } from '@ng-select/ng-select';
import {
  NgbModule,
  NgbPopoverModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule } from '@angular/forms';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { RestrictSpecialCharactersDirective } from './restrict-special-characters.directive';

@NgModule({
  declarations: [BookingHistoryComponent, RestrictSpecialCharactersDirective],
  imports: [
    CommonModule,
    BookingHistoryRoutingModule,
    NgSelectModule,
    NgbPopoverModule,
    ReactiveFormsModule,
    NgbTooltipModule,
    InfiniteScrollModule,
  ],
})
export class BookingHistoryModule {}
